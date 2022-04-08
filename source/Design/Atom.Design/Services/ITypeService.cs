using Atom.Design.Reflection.Metadata;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public interface ITypeService
    {
        TypeAdapter FindAdapter(TypeReference systemType);

        bool RegisterExtension(ITypeExtension extension);

        object ReadValue(TypeReference type, XElement parentElement);

        void WriteValue(TypeReference type, object value, XElement parentElement);

        IEnumerable<CodeStatement> GenerateCode(TypeReference type, object value);

        IEnumerable<ITypeExtension> GetExtensions();
    }
}
