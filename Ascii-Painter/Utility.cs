using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ascii_Painter
{
    public class Utility
    {

        static byte[] keyboardState = new byte[256];
        // https://stackoverflow.com/a/38787314/3908409
        public static string KeyCodeToUnicode(Keys key)
        {
            Array.Clear(keyboardState, 0, 256);

            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)key;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            IntPtr inputLocaleIdentifier = GetKeyboardLayout(0);

            StringBuilder result = new StringBuilder();

            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, inputLocaleIdentifier);

            return result.ToString();
        }

        [DllImport("user32.dll")]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        public static bool IsBlank (char c)
        {
            return c <= ' ';
        }

        public static char GetLineChar()
        {
            return '+';
        }

        public static char GetLineChar(double deg)
        {
            if (deg < 22.5)
                return '-';
            else if (deg < 67.5)
                return '\\';
            else if (deg < 112.5)
                return '|';
            else if (deg < 157.5)
                return '/';
            else if (deg < 202.5)
                return '-';
            else if (deg < 247.5)
                return '\\';
            else if (deg < 295.5)
                return '|';
            else if (deg < 337.5)
                return '/';
            else
                return '-';
        }
        public static char GetLineChar (Point start, Point end)
        {
            if (start == end)
                return GetLineChar();

            return GetLineChar((Math.Atan2(end.Y - start.Y, end.X - start.X) 
                * 180.0 / Math.PI + 360.0) % 360.0);       
        }

        /// <summary>
        /// Ramanujan's ellipse perimeter approx from ellipse radius
        /// </summary>
        public static double EllipsePerimeter(double a, double b)
        {
            return Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));
        }

        static public RegistryKey GetRegPath()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

            key.CreateSubKey("AsciiPainter");
            return key.OpenSubKey("AsciiPainter", true);
        }

    }
}
