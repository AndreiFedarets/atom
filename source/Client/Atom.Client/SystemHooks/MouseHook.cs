using System;
using System.Runtime.InteropServices;

namespace Atom.Client.SystemHooks
{
    public delegate bool MouseEventCallback(MouseEventType eventType, MouseEventData eventData);

    public sealed class MouseHook : BaseHook
    {
        private const int WH_MOUSE_LL = 14;
        private readonly MouseEventCallback _handler;

        public MouseHook(MouseEventCallback handler)
            : base(WH_MOUSE_LL)
        {
            _handler = handler;
        }

        protected override bool OnEvent(int code, IntPtr wParam, IntPtr lParam)
        {
            MouseEventType eventType = (MouseEventType)wParam;
            MouseEventData eventData = (MouseEventData)Marshal.PtrToStructure(lParam, typeof(MouseEventData));
            return _handler(eventType, eventData);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MouseEventData
    {
        public SystemPoint Point;

        public uint MouseData;

        public uint Flags;

        public uint Time;

        public IntPtr ExtraInfo;
    }

    public enum MouseEventType
    {
        LeftButtonDown = 0x0201,
        LeftButtonUp = 0x0202,
        MouseMove = 0x0200,
        MouseWheel = 0x020A,
        RightButtonDown = 0x0204,
        RightButtonUp = 0x0205
    }
}
