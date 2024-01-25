using System.Text;
using static MacroModules.MacroLibrary.WinApi.WindowFocusApi;

namespace MacroModules.MacroLibrary
{
    /// <summary>
    /// Provides methods for controlling windows.
    /// </summary>
    public static class WindowControl
    {
        /// <summary>
        /// Set the current focus to a specific window given its window title name.
        /// </summary>
        /// <param name="windowName">The title name of the window to focus on.</param>
        /// <remarks>
        /// <para>
        ///     All top-level windows will be searched until a window is found that contains the
        ///     search name in its title. This search occurs in no specific order.
        /// </para>
        /// <para>
        ///     Matching <c>windowName</c> to the enumerated window titles is case insensitive.
        /// </para>
        /// </remarks>
        /// <returns>
        /// True if the window was found and if it was sucessfully focused. Otherwise, false.
        /// </returns>
        public static bool SetFocusWindow(string windowName)
        {
            WindowSearchData searchData = new(windowName);
            EnumWindows(EnumWindowHandler, searchData);

            return (searchData.FoundWindow == IntPtr.Zero) ? false : SetForegroundWindow(searchData.FoundWindow);
        }

        /// <summary>
        /// Set the current focus to a specific window given its window handle.
        /// </summary>
        /// <param name="windowHandle">Handle to the window to focus.</param>
        /// <inheritdoc cref="SetFocusWindow(string)" path="/returns"/>
        public static bool SetFocusWindow(IntPtr windowHandle)
        {
            return SetForegroundWindow(windowHandle);
        }

        /// <summary>
        /// Class for sending and receiving search data for <c>EnumWindowHandler</c>.
        /// </summary>
        /// <seealso cref="EnumWindowHandler(nint, object)"/>
        private class WindowSearchData
        {
            public string SearchTitle { get; }
            public IntPtr FoundWindow { get; set; } = IntPtr.Zero;

            public WindowSearchData(string searchTitle)
            {
                SearchTitle = searchTitle;
            }
        }

        /// <summary>
        /// The event handler used in the <c>EnumWindows</c> WinApi call. Sets the
        /// <c>FoundWindow</c> field in the provided <c>WindowSearchData</c> object if a matching
        /// window title is found.
        /// </summary>
        /// <param name="winHandle">Windows handle that is received by the system.</param>
        /// <param name="data">
        /// <c>WindowSearchData</c> object that is used to send and receive search data.
        /// </param>
        /// <returns>
        /// False if the current window contains the provided search term. Otherwise, true.
        /// </returns>
        /// <seealso cref="SetFocusWindow(string)"/>
        /// <seealso cref="WindowSearchData"/>
        private static bool EnumWindowHandler(IntPtr winHandle, object data)
        {
            WindowSearchData searchData = (WindowSearchData)data;
            StringBuilder buffer = new StringBuilder(255);
            GetWindowTextA(winHandle, buffer, 255);
            string windowTitle = buffer.ToString();

            // Check if window title contains the search title (case insensitive)
            if (windowTitle.IndexOf(searchData.SearchTitle, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                searchData.FoundWindow = winHandle;
                return false;
            }

            return true;
        }
    }
}
