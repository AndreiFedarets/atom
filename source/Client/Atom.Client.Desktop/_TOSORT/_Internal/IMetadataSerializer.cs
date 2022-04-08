namespace Atom.Design
{
    internal interface IMetadataSerializer
    {
        ProjectMetadata DeserializeProject(string projectFileFullName);
    }
}
