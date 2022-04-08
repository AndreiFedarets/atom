namespace Atom.Design
{
    public interface IProjectFile : IProjectItem
    {
        new ProjectFileMetadata Metadata { get; }
    }
}
