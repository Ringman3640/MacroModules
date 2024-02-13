using MacroModules.MacroLibrary;
using MacroModules.MacroLibrary.Types;
using MacroModules.Model.Execution.Events;
using MacroModules.Model.GolbalSystems;
using MacroModules.Model.Modules.Concrete;

namespace MacroModules.Model.Execution
{
    /// <summary>
    /// Represents a coordinator for macro execution that executes macros on separate
    /// <see cref="MacroExecutor"/> instances.
    /// </summary>
    public class MacroDispatcher
    {
        /// <summary>
        /// Gets whether the <see cref="MacroDispatcher"/> is running or not.
        /// </summary>
        /// <remarks>
        /// A running <see cref="MacroDispatcher"/> is actively serving macro executions in response
        /// to input trigger events.
        /// </remarks>
        public bool Running { get; private set; } = false;

        /// <summary>
        /// Indicates the maximum number of executor threads that can be initialized on
        /// <see cref="Startup"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     <see cref="MacroDispatcher"/> maintains a pool of <see cref="MacroExecutor"/>
        ///     instances. This value effectively sets the maximum initial count of executors when
        ///     <see cref="Startup"/> is called. This does not limit the amount of total executors
        ///     when running.
        /// </para>
        /// <para>
        ///     By default, this property is set to <c>5</c>.
        /// </para>
        /// </remarks>
        public int MaxExecutorsOnStartup { get; set; } = 5;

        /// <summary>
        /// Indicates a <see cref="InputTrigger"/> that will terminate the
        /// <see cref="MacroDispatcher"/> when clicked.
        /// </summary>
        /// <remarks>
        /// This trigger takes precedence over any macro triggers. If a macro has the same trigger
        /// as this terminating trigger, the macro will not be activated.
        /// </remarks>
        public InputTrigger? TerminateTrigger { get; set; } = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroDispatcher"/> class with default
        /// properties.
        /// </summary>
        public MacroDispatcher() { }

        public void SetStartupMacro(StartupEntryModule? entryModule)
        {
            lock (dispatcherLock)
            {
                if (entryModule == null)
                {
                    startupMacro = null;
                    return;
                }

                MacroExecutionInfo executionInfo = new(entryModule);
                startupMacro = executionInfo;
            }
        }

        /// <summary>
        /// Adds a macro to the <see cref="MacroDispatcher"/> given a
        /// <see cref="TriggerEntryModule"/>.
        /// </summary>
        /// /// <remarks>
        /// Macros are mapped for execution based on their <see cref="InputTrigger"/> value. As
        /// such, each <see cref="InputTrigger"/> value can correspond to only one macro.
        /// </remarks>
        /// <param name="entryModule">
        /// A <see cref="TriggerEntryModule"/> that defines the macro to add.
        /// </param>
        /// <returns>
        /// <c>true</c> if the macro was successfully added. Otherwise <c>false</c>. This method
        /// will return <c>false</c> if the <see cref="MacroDispatcher"/> already has a macro
        /// mapped to the given <see cref="InputTrigger"/> value or if the
        /// <see cref="InputTrigger"/> defined in <paramref name="entryModule"/> was null.
        /// </returns>
        public bool AddMacro(TriggerEntryModule entryModule)
        {
            if (entryModule.Trigger == null)
            {
                return false;
            }

            lock (dispatcherLock)
            {
                if (triggerMap.ContainsKey(entryModule.Trigger))
                {
                    return false;
                }

                MacroExecutionInfo macroInfo = new(entryModule);
                triggerMap.Add(entryModule.Trigger, macroInfo);
                return true;
            }
        }

        /// <summary>
        /// Removes a macro from the <see cref="MacroDispatcher"/>.
        /// </summary>
        /// <param name="trigger">
        /// The <see cref="InputTrigger"/> object that corresponds to the macro to remove.
        /// </param>
        /// <returns>
        /// <c>true</c> if the macro was successfully removed. Otherwise <c>false</c>. This method
        /// will return <c>false</c> if <paramref name="trigger"/> was not found.
        /// </returns>
        public bool RemoveMacro(InputTrigger trigger)
        {
            lock (dispatcherLock)
            {
                return triggerMap.Remove(trigger);
            }
        }

        /// <summary>
        /// Removes all macros from the <see cref="MacroDispatcher"/>.
        /// </summary>
        public void ClearMacros()
        {
            lock (dispatcherLock)
            {
                triggerMap.Clear();
                startupMacro = null;
            }
        }

        /// <summary>
        /// Starts the <see cref="MacroDispatcher"/>.
        /// </summary>
        /// <remarks>
        /// Calling <see cref="Startup"/> installs hooks on the Windows system.
        /// <see cref="Terminate"/> must be called later to uninstall these hooks.
        /// </remarks>
        public void Startup()
        {
            lock (dispatcherLock)
            {
                if (Running)
                {
                    return;
                }

                int executorCount = Math.Min(MaxExecutorsOnStartup, triggerMap.Count);
                executorCount = Math.Max(executorCount, 0);

                for (int i = 0; i < executorCount; ++i)
                {
                    idleExecutors.Push(CreateMacroExecutor());
                }

                InputMonitor.SetInputHandler(OnInputReceived);
                InputMonitor.CollectInput = true;
                InputMonitor.FilterMouseMovements = true;
                InputMonitor.Install();
                Running = true;

                if (startupMacro != null)
                {
                    StartMacro(startupMacro);
                }
            }
        }

        /// <summary>
        /// Stops all running macros.
        /// </summary>
        public void StopAll()
        {
            lock (dispatcherLock)
            {
                foreach (var (executor, executionInfo) in runningExecutors)
                {
                    executionInfo.Executor = null;
                    executionInfo.ToggledOn = false;
                    executor.Stop();
                    idleExecutors.Push(executor);
                }
                runningExecutors.Clear();
            }
        }

        /// <summary>
        /// Terminates the <see cref="MacroDispatcher"/> and all running macros.
        /// </summary>
        public void Terminate()
        {
            lock (dispatcherLock)
            {
                if (!Running)
                {
                    return;
                }

                InputMonitor.CollectInput = false;
                InputMonitor.Uninstall();

                foreach (var (executor, executionInfo) in runningExecutors)
                {
                    executionInfo.Executor = null;
                    executionInfo.ToggledOn = false;
                    executor.Terminate();
                }
                foreach (var executor in idleExecutors)
                {
                    executor.Terminate();
                }
                runningExecutors.Clear();
                idleExecutors.Clear();

                GloalSystemsCleanup();
                Running = false;
            }
        }

        /// <summary>
        /// Lock that controls all access to the <see cref="MacroDispatcher"/> fields.
        /// </summary>
        /// <remarks>
        /// This lock must be obtained before accessing any fields to ensure thread safety.
        /// </remarks>
        private readonly object dispatcherLock = new();

        /// <summary>
        /// The macro that executes on <see cref="Startup"/> that is defined by a
        /// <see cref="MacroExecutionInfo"/> instance.
        /// </summary>
        private MacroExecutionInfo? startupMacro = null;

        /// <summary>
        /// A collection of all running <see cref="MacroExecutor"/> instances that are executing a
        /// macro. Each <see cref="MacroExecutor"/> is mapped to its corresponding
        /// <see cref="MacroExecutionInfo"/>.
        /// </summary>
        private readonly Dictionary<MacroExecutor, MacroExecutionInfo> runningExecutors = [];

        /// <summary>
        /// A collection of all idle <see cref="MacroExecutor"/> instances.
        /// </summary>
        private readonly Stack<MacroExecutor> idleExecutors = new();

        /// <summary>
        /// A mapping of all unique <see cref="InputTrigger"/> instances to their corresponding
        /// <see cref="MacroExecutionInfo"/> macro definition.
        /// </summary>
        private readonly Dictionary<InputTrigger, MacroExecutionInfo> triggerMap = [];

        /// <summary>
        /// Input handler for <see cref="InputMonitor"/>. Responsible for executing dispatching
        /// macros to a <see cref="MacroExecutor"/> when the corresponding
        /// <see cref="InputTrigger"/> is clicked.
        /// </summary>
        /// <param name="input">
        /// A <see cref="InputData"/> instance generated from <see cref="InputMonitor"/>.
        /// </param>
        /// <returns>
        /// <c>false</c> if <paramref name="input"/> should be suppressed from the system. Otherwise
        /// returns <c>true</c> if <paramref name="input"/> should be processed normally by the
        /// system.
        /// </returns>
        private bool OnInputReceived(InputData input)
        {
            InputTrigger? trigger = InputTrigger.CreateFrom(input);
            if (trigger == null)
            {
                // The input is not a trigger input, return with input unsuppressed
                return true;
            }

            lock (dispatcherLock)
            {
                // Check if input is the terminating trigger
                if (trigger.Equals(TerminateTrigger))
                {
                    Terminate();
                    return true;
                }

                if (!triggerMap.TryGetValue(trigger, out MacroExecutionInfo? executionInfo))
                {
                    // No corresponding trigger found, return with input unsuppressed
                    return true;
                }

                if (executionInfo.MacroEntryModule is not TriggerEntryModule triggerEntry)
                {
                    // The target macro execution does not correspond to a trigger, return with
                    // input unsuppressed
                    return true;
                }

                switch (triggerEntry.ExecutionType)
                {
                    case MacroExecutionType.InterruptOnReclick:
                        if (executionInfo.Executor == null)
                        {
                            StartMacro(executionInfo);
                        }
                        else
                        {
                            executionInfo.Executor.Restart();
                        }
                        break;

                    case MacroExecutionType.ToggleLoop:
                        executionInfo.ToggledOn = !executionInfo.ToggledOn;
                        if (executionInfo.ToggledOn)
                        {
                            StartMacro(executionInfo);
                        }
                        break;

                    case MacroExecutionType.IgnoreOnReclick:
                    case MacroExecutionType.LoopOnHold:
                    default:
                        if (executionInfo.Executor == null)
                        {
                            StartMacro(executionInfo);
                        }
                        break;
                }

                // Return and suppress input if specified
                return !triggerEntry.SuppressInput;
            }
        }

        /// <summary>
        /// Execution finished handler for <see cref="MacroExecutor"/>. Responsible for repeating a
        /// macro if necessary and returning the executor to the <see cref="idleExecutors"/> pool if
        /// the macro has ended.
        /// </summary>
        /// <param name="sender">
        /// The <see cref="MacroExecutor"/> instance that is finished executing.
        /// </param>
        /// <param name="e">A <see cref="ExecutionFinishedArgs"/> object.</param>
        private void OnExecutionFinished(object sender, ExecutionFinishedArgs e)
        {
            var executor = (MacroExecutor)sender;

            lock (dispatcherLock)
            {
                if (!runningExecutors.TryGetValue(executor, out MacroExecutionInfo? executionInfo))
                {
                    return;
                }

                if (executionInfo.MacroEntryModule is TriggerEntryModule triggerEntry)
                {
                    // Restart execution if trigger is toggled on
                    if (triggerEntry.ExecutionType == MacroExecutionType.ToggleLoop && executionInfo.ToggledOn)
                    {
                        e.RestartExecution = true;
                        return;
                    }

                    // Restart execution if trigger input is held
                    if (triggerEntry.ExecutionType == MacroExecutionType.LoopOnHold && TriggerHeld(triggerEntry.Trigger!))
                    {
                        e.RestartExecution = true;
                        return;
                    }
                }

                // End execution
                executionInfo.Executor = null;
                runningExecutors.Remove(executor);
                idleExecutors.Push(executor);
            }
        }

        /// <summary>
        /// Gets whether a trigger input is currently being held. Helper method for
        /// <see cref="OnExecutionFinished(object, ExecutionFinishedArgs)"/>.
        /// </summary>
        /// <param name="trigger">The <see cref="InputTrigger"/> input sequence to check.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="trigger"/> is being held. Otherwise <c>false</c>.
        /// </returns>
        private bool TriggerHeld(InputTrigger trigger)
        {
            if (!InputMonitor.InputHeld(trigger.InputKeyCode))
            {
                return false;
            }
            if (trigger.Modifiers.HasFlag(InputModifiers.CtrlHeld) && !InputMonitor.InputHeld((ushort)InputCode.Ctrl))
            {
                return false;
            }
            if (trigger.Modifiers.HasFlag(InputModifiers.ShiftHeld) && !InputMonitor.InputHeld((ushort)InputCode.Shift))
            {
                return false;
            }
            if (trigger.Modifiers.HasFlag(InputModifiers.AltHeld) && !InputMonitor.InputHeld((ushort)InputCode.Alt))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a new <see cref="MacroExecutor"/> instance with the
        /// <see cref="MacroExecutor.ExecutionFinished"/> event being listened to.
        /// </summary>
        /// <returns>The created <see cref="MacroExecutor"/> instance.</returns>
        private MacroExecutor CreateMacroExecutor()
        {
            MacroExecutor executor = new();
            executor.ExecutionFinished += OnExecutionFinished;
            return executor;
        }

        /// <summary>
        /// Starts a macro on a <see cref="MacroExecutor"/> instance.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     This method starts a macro by attempting to obtain a <see cref="MacroExecutor"/>
        ///     from the <see cref="idleExecutors"/> pool. If the pool is empty, a new
        ///     <see cref="MacroExecutor"/> instance will be created and used to execute the macro.
        /// </para>
        /// <para>
        ///     The <see cref="MacroExecutor"/> instance that is used to execute the macro will be
        ///     put into the <see cref="runningExecutors"/> pool.
        /// </para>
        /// </remarks>
        /// <param name="executionInfo">
        /// A <see cref="MacroExecutionInfo"/> instance that defines the macro.
        /// </param>
        private void StartMacro(MacroExecutionInfo executionInfo)
        {
            lock (dispatcherLock)
            {
                if (idleExecutors.Count == 0)
                {
                    // Idle executor pool is empty, create a new executor
                    executionInfo.Executor = CreateMacroExecutor();
                    executionInfo.Executor.ExecutionFinished += OnExecutionFinished;
                }
                else
                {
                    // Take an executor from the executor pool
                    executionInfo.Executor = idleExecutors.Pop();
                }
                runningExecutors.Add(executionInfo.Executor, executionInfo);
                executionInfo.Executor.Start(executionInfo.MacroEntryModule);
            }
        }

        /// <summary>
        /// Cleans up all global systems that may have been running during execution
        /// </summary>
        private void GloalSystemsCleanup()
        {
            SoundManager.DispatchStopAll();
        }
    }
}
