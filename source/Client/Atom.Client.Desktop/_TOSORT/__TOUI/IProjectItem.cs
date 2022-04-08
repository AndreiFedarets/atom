namespace Atom.Design
{
    public interface IProjectItem
    {
        string Name { get; set; }

        string Path { get; }

        string FullName { get; }

        void Delete();
    }
}
