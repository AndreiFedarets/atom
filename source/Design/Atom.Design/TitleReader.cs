using System.Collections;
using System.Text.RegularExpressions;

namespace Atom.Design
{
    public sealed class TitleReader
    {
        private static readonly Regex Regex;
        private readonly IEnumerator _titleBlocks;

        static TitleReader()
        {
            Regex = new Regex(@"({.+?})");
        }

        public TitleReader(string title)
        {
            _titleBlocks = Regex.Split(title).GetEnumerator();
        }

        public string Content { get; private set; }

        public bool IsParameter { get; private set; }

        public bool MoveNext()
        {
            bool result = _titleBlocks.MoveNext();
            if (!result)
            {
                return false;
            }
            string titleBlock = (string)_titleBlocks.Current;
            if (Regex.IsMatch(titleBlock))
            {
                Content = titleBlock.Trim('{', '}');
                IsParameter = true;
            }
            else
            {
                Content = titleBlock;
                IsParameter = false;
            }
            return true;
        }
    }
}
