using System;

namespace Atom.Design.Reflection.Metadata
{
    [Flags]
    public enum ParameterDirection : byte
    {
        Input = 0x1,
        Output = 0x2,
        Reference = Input | Output
    }
}
