using System;
using System.Drawing;

namespace Interop.Native.Accessible
{
    public class RectangleProperty : AccessiblePropertyBase<Rectangle>
    {
        public RectangleProperty(Func<Rectangle> valueExtractor, bool cache, Rectangle defaultValue)
            : base(valueExtractor, cache, defaultValue)
        {
        }
    }
}
