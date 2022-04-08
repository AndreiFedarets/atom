namespace Atom.Runtime.Extension.Desktop
{
    public interface IWindow : IControl
    {
        Application Application { get; }

        void Close();

        void Maximize();

        void Minimize();
    }
}
