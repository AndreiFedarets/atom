namespace Atom.Rendering
{
    public sealed class TextBlock : Block
    {
        public TextBlock(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }

        public override bool IsEditable
        {
            get { return false; }
        }

        public override bool IsEnabled
        {
            get { return false; }
        }

        public override string DisplayValue
        {
            get { return Text; }
            set {  }
        }
    }
}
