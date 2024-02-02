using MacroModules.MacroLibrary;
using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;
using System.Diagnostics;

namespace MacroModules.Model.Modules.Concrete
{
    public class CloseProgramModule : ValuedModule
    {
        /// <summary>
        /// Indicates the search term used to find matching programs to close.
        /// </summary>
        public string SearchTerm { get; set; } = "";

        /// <summary>
        /// Indicates the program name component to match against using <see cref="SearchTerm"/>.
        /// </summary>
        public ProgramSearchTarget SearchComponent { get; set; } = ProgramSearchTarget.ProcessName;

        /// <summary>
        /// Indicates if the <see cref="CloseProgramModule"/> should close all programs that match
        /// <see cref="SearchTerm"/> or just the first instance found.
        /// </summary>
        public bool CloseAllMatches { get; set; } = false;

        /// <summary>
        /// Indicates if the child processes of a program should be closed.
        /// </summary>
        public bool CloseChildren { get; set; } = false;

        public override ValueDataType ReturnValueType { get; } = ValueDataType.Bool;

        public override ModuleType Type { get; } = ModuleType.CloseProgram;

        public override void Initialize(out object? processData)
        {
            processData = new CloseProgramModuleData();
        }

        public override IResponse Execute(ref object? processData)
        {
            // Exit if search term is empty
            if (SearchTerm == "")
            {
                return new ContinueResponse(new BoolValue(false));
            }

            var data = (CloseProgramModuleData)processData!;

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
                bool match = searchTarget.IndexOf(SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0;

                if (match && process.MainWindowHandle != data.CurrentProcess.MainWindowHandle)
                {
                    data.ClosingQueue.Enqueue(process);

                    if (!CloseAllMatches)
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

            // Close all matching
            timer.Restart();
            while (data.ClosingQueue.Count > 0)
            {
                try
                {
                    data.ClosingQueue.Dequeue().Kill(CloseChildren);
                    data.CloseSuccess = true;
                }
                catch { }

                if (timer.ElapsedMilliseconds > 10)
                {
                    return new RepeatResponse();
                }
            }

            return new ContinueResponse(new BoolValue(data.CloseSuccess));
        }

        private class CloseProgramModuleData
        {
            /// <summary>
            /// A reference to the current process. Used to ensure that this program doesn't close
            /// itself.
            /// </summary>
            public Process CurrentProcess { get; set; } = Process.GetCurrentProcess();

            /// <summary>
            /// List of all active processes on the system.
            /// </summary>
            public Process[]? ProcessList { get; set; } = null;

            /// <summary>
            /// Queue of processes on the system what will be closed.
            /// </summary>
            public Queue<Process> ClosingQueue { get; } = new();

            /// <summary>
            /// The current index of <see cref="ProcessList"/> that is being searched.
            /// </summary>
            public int SearchIndex { get; set; } = 0;

            /// <summary>
            /// Indicates if a close execution was successful.
            /// </summary>
            public bool CloseSuccess { get; set; } = false;
        }
    }
}
