using MacroModules.Model.Modules.Responses;
using MacroModules.Model.Types;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that pauses macro execution for a specified length of
    /// time.
    /// </summary>
    public class WaitModule : Module
    {
        /// <summary>
        /// Indicates the amount of time to wait for.
        /// </summary>
        public TimeDuration Time { get; set; } = new(500, DurationGranularity.Ms);

        public override ModuleType Type { get; } = ModuleType.Wait;

        public override bool IsConnectable { get; } = true;

        public override void Initialize(out object? processData)
        {
            processData = false;
        }

        public override IResponse Execute(ref object? processData)
        {
            if (!(bool)processData!)
            {
                processData = true;
                return new WaitRepeatResponse(Time.TimeSpan);
            }
            return new ContinueResponse();
        }
    }
}
