using System.Runtime.InteropServices;
using MacroModules.MacroLibrary.Types;

namespace MacroModules.MacroLibrary
{
    /// <summary>
    /// Provides methods for translating the Position coordinates of one system to another system
    /// with different displays.
    /// </summary>
    /// <remarks>
    /// This class is provided to solve the issue of macros being created and executed in different
    /// systems. A coordinate position in one system may not exist in another due to display
    /// mismatches, such as different positioning or a different number of displays. Translating a
    /// Position coordinate will map it from any display in one system to the primary display of
    /// another system.
    /// </remarks>
    public static class DisplayPositionTranslator
    {
        /// <summary>
        /// The display data of the native system. 
        /// </summary>
        /// <remarks>
        /// Shorthand for calling <see cref="SetNativeSystem(SystemDisplayData)"/> and
        /// <see cref="RemoveNativeSystem"/>.
        /// </remarks>
        public static SystemDisplayData? NativeSystem
        {
            get { return nativeSystemData; }
            set
            {
                if (value == null)
                {
                    RemoveNativeSystem();
                }
                else
                {
                    SetNativeSystem(value);
                }
            }
        }

        /// <summary>
        /// Set the native system data that will be used to translate positions to the current
        /// system.
        /// </summary>
        /// <param name="systemData">
        /// The display data of the native system to translate from.
        /// </param>
        public static void SetNativeSystem(SystemDisplayData systemData)
        {
            nativeSystemData = systemData;
            translationNeeded = currentSystemData != nativeSystemData;
        }

        /// <summary>
        /// Remove the native system data. This will disable all translations.
        /// </summary>
        public static void RemoveNativeSystem()
        {
            nativeSystemData = null;
            translationNeeded = false;
        }

        /// <inheritdoc cref="Translate(ref Position)" path="/summary"/>
        /// <inheritdoc cref="Translate(ref Position)" path="/remarks"/>
        /// <param name="position">Coordinate position to translate.</param>
        /// <returns>
        /// The resulting Position from the translation. If no translation occurs, the original
        /// Position is returned.
        /// </returns>
        public static Position Translate(Position position)
        {
            Translate(ref position);
            return position;
        }

        /// <summary>
        /// Translate a given Position's coordinates from the native system to the current system.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Translation will only occur if the a native system is provided and if the current
        ///     system is different from the native system. 
        /// </para>
        /// <para>
        ///     Translations will map a position from any display in the native system to the
        ///     primary display of the current system. The position is also scaled to account for
        ///     resolution differences.
        /// </para>
        /// </remarks>
        /// <param name="position">Coordinate position to translate in-place.</param>
        public static void Translate(ref Position position)
        {
            if (!translationNeeded)
            {
                return;
            }

            // Get the containing display of the point from the native sytsem
            // This just iterates through the displays in the native system data until it finds one
            // that contains the coordinates of the position.
            // This may be an inefficient solution, but works for now. A quadtree may be a better
            // solution, but I'm assuming that most users will only have at most two or three
            // displays. 
            DisplayData? containingDisplay = null;
            foreach (DisplayData display in nativeSystemData!.Displays)
            {
                if (display.Contains(position))
                {
                    containingDisplay = display;
                    break;
                }
            }

            // If the position exists outside the bounds of the diplays in the native system, just
            // mod the position by the current system's primary width and height
            if (containingDisplay == null)
            {
                position.X %= currentSystemData.PrimaryScreenWidth;
                position.Y %= currentSystemData.PrimaryScreenHeight;
                return;
            }

            // Remap position to corresponding scaled position in the primary display
            // Steps:
            //   1. Remap position to primary screen assuming the same resolution
            //   2. Adjust to resolution differences by scaling position values.
            float xScale = (float)currentSystemData.PrimaryScreenWidth / (float)containingDisplay.Width;
            float yScale = (float)currentSystemData.PrimaryScreenHeight / (float)containingDisplay.Height;
            position.X -= containingDisplay.Origin.X;
            position.X = (int)((float)position.X * xScale);
            position.Y -= containingDisplay.Origin.Y;
            position.Y = (int)((float)position.Y * yScale);
        }

        /// <summary>
        /// Set the current system data to a custom system for testing.
        /// </summary>
        /// <remarks>
        /// This method is only for testing purposes. DO NOT USE FOR LIBRARY IMPLEMENTATION.
        /// </remarks>
        internal static void SetCurrentSystemForTesting(SystemDisplayData systemData)
        {
            currentSystemData = systemData;
            translationNeeded = currentSystemData == nativeSystemData;
        }

        private static SystemDisplayData currentSystemData = new();
        private static SystemDisplayData? nativeSystemData = null;
        private static bool translationNeeded = false;
    }
}
