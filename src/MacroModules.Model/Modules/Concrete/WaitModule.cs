using MacroModules.Model.Modules.Responses;

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
        public TimeSpan Time { get; set; } = TimeSpan.FromMilliseconds(500);

        public override ModuleType Type { get; } = ModuleType.Wait;

        public override void Initialize(out object? processData)
        {
            processData = false;
        }

        public override IResponse Execute(ref object? processData)
        {
            if (!(bool)processData!)
            {
                processData = true;
                return new WaitRepeatResponse(Time);
            }
            return new ContinueResponse();
        }
    }
}
