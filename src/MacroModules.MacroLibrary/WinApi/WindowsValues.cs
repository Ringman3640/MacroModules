using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacroModules.MacroLibrary.WinApi
{
    internal enum KeyboardMessage : int
    {
        KeyDown = 0x0100,
        KeyUp = 0x0101,
        SysKeyDown = 0x0104,
        SysKeyUp = 0x0105
    }

    internal enum MouseMessage : int
    {
        Move = 0x0200,
        LeftButtonDown = 0x0201,
        LeftButtonUp = 0x0202,
        RightButtonDown = 0x0204,
        RightButtonUp = 0x0205,
        MiddleButtonDown = 0x0207,
        MiddleButtonUp = 0x0208,
        VerticalWheel = 0x020A,
        XButtonDown = 0x020B,
        XButtonUp = 0x020C,
        HorizontalWheel = 0x020E
    }

    internal enum VirtualKey : ushort
    {
        LeftMouseButton = 0x01,
        RightMouseButton = 0x02,
        MiddleMouseButton = 0x03,
        MouseButtonX1 = 0x05,
        MouseButtonX2 = 0x06
    }
}
