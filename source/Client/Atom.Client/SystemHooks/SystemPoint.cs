using System.Runtime.InteropServices;

namespace Atom.Client.SystemHooks
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemPoint
    {
        public int X;

        public int Y;
    }
}
