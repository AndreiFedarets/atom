using System.Runtime.InteropServices;

namespace Atom.Office.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
        public int Height
        {
            get { return Bottom - Top; }
        }
        public int Width
        {
            get { return Right - Left; }
        }
    }
}
