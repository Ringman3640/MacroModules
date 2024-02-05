using System.Windows.Media;

namespace MacroModules.Model.GolbalSystems
{
    /// <summary>
    /// Represents a manager system for playing audio files asynchronously.
    /// </summary>
    public static class SoundManager
    {
        /// <summary>
        /// Indicates the maximum number of simultaneous audio playback. This value is 5 by default.
        /// </summary>
        public static int MaxSimultaneousAudio
        {
            get { return maxSimultaneousAudio; }
            set
            {
                lock (accessLock)
                {
                    maxSimultaneousAudio = Math.Clamp(value, 1, 20);
                    while (playerQueue.Count > maxSimultaneousAudio)
                    {
                        playerQueue.Dequeue();
                    }
                }
            }
        }

        /// <summary>
        /// Plays an audio file given a <see cref="Uri"/> to its file location.
        /// </summary>
        /// <param name="soundUri">
        /// The <see cref="Uri"/> that specifies the sound file's location.
        /// </param>
        /// <param name="volume">
        /// A value between 0 and 1 that indicates the playback volume. 1 is the default volume and
        /// 0 is completely quiet.
        /// </param>
        public static void Play(Uri soundUri, double volume = 1)
        {
            // For some reason, the volume of any played sound seems to be the most lound on
            // volume 0.99 instead of 1. There might be some weird edge case that clamps a value of
            // 1 to 0.5 (which it seems to become). This manually clamps the max volume amount to
            // 0.99 to ensure that a value of 1 results in the loudest sound.
            volume = Math.Clamp(volume, 0, 0.99);

            lock (accessLock)
            {
                MediaPlayer player;
                if (playerQueue.Count >= maxSimultaneousAudio)
                {
                    player = playerQueue.Dequeue();
                    player.Stop();
                }
                else
                {
                    player = new();
                }
                player.Volume = volume;
                player.Open(soundUri);
                player.Play();
                playerQueue.Enqueue(player);
            }
        }

        private static object accessLock = new();
        private static int maxSimultaneousAudio = 5;
        private static readonly Queue<MediaPlayer> playerQueue = new();
    }
}
