using MacroModules.Model.Execution.Events;
using MacroModules.Model.Modules;
using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Execution
{
    /// <summary>
    /// Provides methods for executing a macro chain asynchronously.
    /// </summary>
    public class MacroExecutor : INotifyExecutionFinished
    {
        /// <summary>
        /// Gets the current state of the execution thread.
        /// </summary>
        public MacroExecutorState State { get; private set; } = MacroExecutorState.Idle;

        /// <inheritdoc/>
        public event ExecutionFinishedEventHandler? ExecutionFinished;

        public MacroExecutor()
        {
            new Thread(new ThreadStart(ExecutionProcess)).Start();
        }

        /// <summary>
        /// Start the executor.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This method will not restart the executor if it is already running. If this is the
        ///     desired behavior, call <see cref="Restart"/> or first call <see cref="Stop"/> then
        ///     <see cref="Start(Module)"/>.
        /// </para>
        /// <para>
        ///     This method will have no effect if the executor has been terminated.
        /// </para>
        /// </remarks>
        /// <param name="startingModule">The first module to start executing on.</param>
        public void Start(Module startingModule)
        {
            lock (requestLock)
            {
                if (State == MacroExecutorState.Running)
                {
                    return;
                }
                startupModule = startingModule;
                requestQueue.Enqueue(MacroExecutorRequest.Start);
                Monitor.Pulse(requestLock);
            }
        }

        /// <summary>
        /// Stop any macro execution and return the executor to the
        /// <see cref="MacroExecutorState.Idle"/> state.
        /// </summary>
        /// <remarks>
        /// This method will have no effect if the executor has been terminated.
        /// </remarks>
        public void Stop()
        {
            QueueRequest(MacroExecutorRequest.Stop);
        }

        /// <summary>
        /// Restart execution of the macro with the starting <see cref="Module"/> indicated from the
        /// last <see cref="Start(Module)"/> call.
        /// </summary>
        /// <remarks>
        /// This method will have no effect if the executor has been terminated.
        /// </remarks>
        public void Restart()
        {
            QueueRequest(MacroExecutorRequest.Restart);
        }

        /// <summary>
        /// Terminate the execution thread.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This method will put the executor in the <see cref="MacroExecutorState.Terminated"/>
        ///     state. In this state, the executor will no longer respond to any requests. This
        ///     cannot be undone.
        /// </para>
        /// <para>
        ///     This method should be called during shutdown.
        /// </para>
        /// </remarks>
        public void Terminate()
        {
            QueueRequest(MacroExecutorRequest.Terminate);
        }

        /// <summary>
        /// Mutex that controls access to <see cref="requestQueue"/>. This ensures that only one
        /// thread accesses the queue at any given time. The execution thread will also use this
        /// mutex to wait. When waiting, it should be notified of any additions to the queue.
        /// </summary>
        private readonly object requestLock = new();

        private Module? startupModule = null;

        /// <summary>
        /// Queue that contains all waiting <see cref="MacroExecutorRequest"/> instances. A thread
        /// must obtain <see cref="requestLock"/> before accessing the queue.
        /// </summary>
        private readonly Queue<MacroExecutorRequest> requestQueue = new();

        /// <summary>
        /// Add a <see cref="MacroExecutorRequest"/> to <see cref="requestQueue"/> safely and pulse
        /// the execution thread if it is waiting.
        /// </summary>
        /// <param name="request">The request to queue.</param>
        private void QueueRequest(MacroExecutorRequest request)
        {
            lock (requestLock)
            {
                requestQueue.Enqueue(request);
                Monitor.Pulse(requestLock);
            }
        }

        /// <summary>
        /// Main execution thread that facilitates module execution.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This method should be initialized as a separate thread on construction.
        /// </para>
        /// <para>
        ///     To notify this thread or to act on it, send a <see cref="MacroExecutorRequest"/>
        ///     message to <see cref="requestQueue"/>. The execution thread will periodically check
        ///     for requests and execute them in the order they arrive. You must obtain
        ///     <see cref="requestLock"/> before accessing <see cref="requestQueue"/>.
        /// </para>
        /// <para>
        ///     The execution thread will not respond to any requests when in the
        ///     <see cref="MacroExecutorState.Terminated"/> state. The thread will enter this state
        ///     if it receives a <see cref="MacroExecutorRequest.Terminate"/> request.
        /// </para>
        /// </remarks>
        private void ExecutionProcess()
        {
            Module? currentModule = null;
            object? currentModuleData = null;
            bool moduleInitialized = false;

            while (true)
            {
                // Action based on the state of the executor
                switch (State)
                {
                    case MacroExecutorState.Idle:
                        lock (requestLock)
                        {
                            if (requestQueue.Count == 0)
                            {
                                Monitor.Wait(requestLock);
                            }
                        }
                        break;

                    case MacroExecutorState.Running:
                        HandleModuleExecution(
                            ref currentModule,
                            ref currentModuleData, 
                            ref moduleInitialized);
                        break;
                }

                // Check and respond to requests
                lock (requestLock)
                {
                    while (requestQueue.Count > 0)
                    {
                        var request = requestQueue.Dequeue();
                        switch (request)
                        {
                            case MacroExecutorRequest.Start:
                                currentModule = startupModule;
                                currentModuleData = null;
                                moduleInitialized = false;
                                State = MacroExecutorState.Running;
                                break;

                            case MacroExecutorRequest.Stop:
                                currentModule?.Restore(ref currentModuleData);
                                State = MacroExecutorState.Idle;
                                break;

                            case MacroExecutorRequest.Restart:
                                currentModule?.Restore(ref currentModuleData);
                                currentModule = startupModule;
                                moduleInitialized = false;
                                State = MacroExecutorState.Running;
                                break;

                            case MacroExecutorRequest.Terminate:
                                currentModule?.Restore(ref currentModuleData);
                                State = MacroExecutorState.Terminated;
                                return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles module initialization, execution, and responses. Helper method for
        /// <see cref="ExecutionProcess"/>.
        /// </summary>
        /// <param name="module">The <see cref="Module"/> to execute on.</param>
        /// <param name="moduleData">The data object used to store module data.</param>
        /// <param name="initialized">Indicates if the module has been initialized.</param>
        private void HandleModuleExecution(ref Module? module, ref object? moduleData, ref bool initialized)
        {
            if (module is null)
            {
                State = MacroExecutorState.Idle;
                return;
            }

            if (!initialized)
            {
                module.Initialize(out moduleData);
                initialized = true;
            }

            IResponse response = module.Execute(ref moduleData);
            switch (response.Type)
            {
                case ResponseType.Continue:
                    // Prepare execution of next module
                    var responseContinue = (ContinueResponse)response;
                    module = responseContinue.NextModule;
                    moduleData = null;
                    initialized = false;
                    if (module == null)
                    {
                        // If the next module is null, invoke the ExecutionFinished listeners
                        // Restart macro if requested
                        // Otherwise transition to idle state
                        ExecutionFinishedArgs args = new();
                        ExecutionFinished?.Invoke(this, args);
                        if (args.RestartExecution)
                        {
                            module = startupModule;
                        }
                        else
                        {
                            State = MacroExecutorState.Idle;
                        }
                    }
                    break;

                case ResponseType.Repeat:
                    // Do nothing else, execution should repeat automatically
                    break;

                case ResponseType.WaitRepeat:
                    // Wait on thread if request queue is empty
                    var responseWaitRepeat = (WaitRepeatResponse)response;
                    int waitMs = (int)responseWaitRepeat.WaitTime.TotalMilliseconds;
                    lock (requestLock)
                    {
                        if (requestQueue.Count == 0)
                        {
                            Monitor.Wait(requestLock, waitMs);
                        }
                    }
                    break;
            }
        }
    }
}
