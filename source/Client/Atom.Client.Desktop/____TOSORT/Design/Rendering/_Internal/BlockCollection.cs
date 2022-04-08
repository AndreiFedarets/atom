using System.Collections.Generic;

namespace Atom.Rendering
{
    internal sealed class BlockCollection : ReadOnlyCollection<Block>, IBlockCollection
    {
        public BlockCollection(IEnumerable<Block> blocks)
            : base(blocks)
        {
            
        }
    }
}
