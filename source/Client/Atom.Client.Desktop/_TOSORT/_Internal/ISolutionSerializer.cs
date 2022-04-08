using System.IO;

namespace Atom.Design
{
    internal interface ISolutionSerializer
    {
        SolutionMetadata Deserialize(Stream stream);

        SolutionMetadata Deserialize(StreamReader reader);

        void Serialize(SolutionMetadata metadata, Stream stream);

        void Serialize(SolutionMetadata metadata, StreamWriter writer);
    }
}
