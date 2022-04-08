namespace Atom.Design
{
    public interface IRelativePathResolver
    {
        string ToAbsolute(string relativePath);

        string ToRelative(string absolutePath);
    }
}
