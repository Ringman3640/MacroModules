using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Values;
using System.Diagnostics;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that opens a program given its name and file location.
    /// </summary>
    public class OpenProgramModule : ValuedModule
    {
        /// <summary>
        /// Indicates the name and file location of the program to open.
        /// </summary>
        public string ProgramPath { get; set; } = "";

        /// <summary>
        /// Indicates the text arguments to pass to the program.
        /// </summary>
        public string Arguments { get; set; } = "";

        public override ModuleType Type { get; } = ModuleType.OpenProgram;

        public override ValueDataType ReturnValueType { get; } = ValueDataType.Bool;

        /// <remarks>
        /// On completion, the method returns a <see cref="ValuedContinueResponse"/> object. The
        /// returned <see cref="Value"/> is a <c>bool</c> that indicates if the program was
        /// successfully opened.
        /// </remarks>
        /// <inheritdoc/>
        public override IResponse Execute(ref object? processData)
        {
            bool failed = false;
            try
            {
                Process.Start(ProgramPath, Arguments);
            }
            catch
            {
                failed = true;
            }

            Value returnValue = new BoolValue(!failed);
            SetStoreVariable(returnValue);
            return new ContinueResponse(returnValue);
        }
    }
}
