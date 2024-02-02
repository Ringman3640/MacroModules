using MacroModules.MacroLibrary;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;
using System.Diagnostics;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that sets current foreground window.
    /// </summary>
    public class FocusWindowModule : ValuedModule
    {
        /// <summary>
        /// Indicates the search term used to find a matching window to focus.
        /// </summary>
        public string SearchTerm { get; set; } = "";

        /// <summary>
        /// Indicates the window name component to match against using <see cref="SearchTerm"/>.
        /// </summary>
        public ProgramSearchTarget SearchComponent { get; set; } = ProgramSearchTarget.ProcessName;

        /// <summary>
        /// Indicates if the <see cref="FocusWindowModule"/> should focus all windows that match
        /// <see cref="SearchTerm"/> or just the first instance found.
        /// </summary>
        public bool FocusAllMatches { get; set; } = false;

        public override ModuleType Type { get; } = ModuleType.FocusWindow;

        public override ValueDataType ReturnValueType { get; } = ValueDataType.Bool;

        public override void Initialize(out object? processData)
        {
            processData = new FocusWindowModuleData();
        }

        /// <remarks>
        /// On completion, the method returns a <see cref="ValuedContinueResponse"/> object. The
        /// returned <see cref="Value"/> is a <c>bool</c> that indicates if a successful window
        /// focus took place.
        /// </remarks>
        /// <inheritdoc/>
        public override IResponse Execute(ref object? processData)
        {
            // Exit if search term is empty
            if (SearchTerm == "")
            {
                return new ValuedContinueResponse(ExitPorts[0].Destination)
                {
                    ReturnValue = new BoolValue(false)
                };
            }

            var data = (FocusWindowModuleData)processData!;

            // Get list of all processes if not gotten yet
            if (data.ProcessList == null)
            {
                data.ProcessList = Process.GetProcesses();
                return new RepeatResponse();
            }

            // Search over list of processes for matching
            Stopwatch timer = Stopwatch.StartNew();
            while (data.SearchIndex < data.ProcessList.Length)
            {
                Process process = data.ProcessList[data.SearchIndex];
                string searchTarget = (SearchComponent == ProgramSearchTarget.WindowTitle) ? process.MainWindowTitle : process.ProcessName;

                if (searchTarget.Contains(SearchTerm) && !data.QueuedHandles.Contains(process.MainWindowHandle))
                {
                    data.FocusHandleQueue.Enqueue(process.MainWindowHandle);
                    data.QueuedHandles.Add(process.MainWindowHandle);

                    if (!FocusAllMatches)
                    {
                        break;
                    }
                }

                ++data.SearchIndex;
                if (timer.ElapsedMilliseconds > 10)
                {
                    return new RepeatResponse();
                }
            }

            // Focus all matching
            timer.Restart();
            while (data.FocusHandleQueue.Count > 0)
            {
                if (WindowControl.SetFocusWindow(data.FocusHandleQueue.Dequeue()))
                {
                    data.FocusSuccess = true;
                }
                if (timer.ElapsedMilliseconds > 10)
                {
                    return new RepeatResponse();
                }
            }

            return new ValuedContinueResponse(ExitPorts[0].Destination)
            {
                ReturnValue = new BoolValue(data.FocusSuccess)
            };
        }

        private class FocusWindowModuleData
        {
            /// <summary>
            /// List of all active processes on the system.
            /// </summary>
            public Process[]? ProcessList { get; set; } = null;

            /// <summary>
            /// List of window handles that will be brought to the foreground
            /// </summary>
            public Queue<IntPtr> FocusHandleQueue { get; } = new();

            /// <summary>
            /// List of window handles that are currently in <see cref="FocusHandleQueue"/>. This is
            /// used for quick lookup to prevent adding repeats to the queue.
            /// </summary>
            public HashSet<IntPtr> QueuedHandles { get; } = new();

            /// <summary>
            /// The current index of <see cref="ProcessList"/> that is being searched.
            /// </summary>
            public int SearchIndex { get; set; } = 0;

            /// <summary>
            /// Indicates if a focus execution was successfull.
            /// </summary>
            public bool FocusSuccess { get; set; } = false;
        }
    }
}
