using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Atom.Rendering
{
    internal sealed class ActionMessageRendering : IActionMessageRendering
    {
        public IBlockCollection RenderActionMessage(IActionInstance action)
        {
            List<Block> blocks = new List<Block>();
            //split text parts and arguments positions
            Regex splitRegex = new Regex("{\\d{1,}}|\\s{0,}\\w+\\s{0,}");
            Regex argumentRegex = new Regex("{\\d{1,}}");
            Regex argumentIndexRegex = new Regex("\\d{1,}");
            MatchCollection matches = splitRegex.Matches(action.GetMetadata().Message);
            foreach (Match match in matches)
            {
                string value = match.Value;
                Block block;
                //this is argument placeholder
                if (argumentRegex.IsMatch(value))
                {
                    string argumentIndexString = argumentIndexRegex.Match(value).Value;
                    int argumentIndex = int.Parse(argumentIndexString);
                    IArgument argument = action.Arguments[argumentIndex];
                    block = RenderBlockByArgument(argument);
                }
                //this is usual text
                else
                {
                    block = new TextBlock(value);
                }
                blocks.Add(block);
            }
            return new BlockCollection(blocks);
        }

        private Block RenderBlockByArgument(IArgument argument)
        {
            if (argument.Direction == Direction.Input)
            {
                return new InputArgumentBlock(argument);
            }
            if (argument.Direction == Direction.Output)
            {
                return new OutputArgumentBlock(argument);
            }
            throw Fail.Design.TempException();
        }
    }
}
