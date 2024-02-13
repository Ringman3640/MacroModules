using MacroModules.Model.GolbalSystems;
using MacroModules.Model.Modules.Responses;

namespace MacroModules.Model.Modules.Concrete
{
    /// <summary>
    /// Represents a <see cref="Module"/> that plays an audio file.
    /// </summary>
    public class PlaySoundModule : Module
    {
        /// <summary>
        /// Indicates the relative or absolue path of the audio file to play.
        /// </summary>
        public string SoundFile { get; set; } = "";

        /// <summary>
        /// Indicates the playback volume of the audio file from a value of 1 to 0.
        /// </summary>
        /// <remarks>
        /// A value of 1 represents normal playback volume. A value of 0 represents no volume.
        /// </remarks>
        public double Volume
        {
            get { return playbackVolume; }
            set { playbackVolume = Math.Clamp(value, 0, 1); }
        }

        public override ModuleType Type { get; } = ModuleType.PlaySound;

        public override bool IsConnectable { get; } = true;

        public override IResponse Execute(ref object? processData)
        {
            if (SoundFile != "")
            {
                Uri soundUri = new(SoundFile, UriKind.RelativeOrAbsolute);
                SoundManager.DispatchPlay(soundUri, Volume);
            }

            return new ContinueResponse();
        }

        private double playbackVolume = 1;
    }
}
