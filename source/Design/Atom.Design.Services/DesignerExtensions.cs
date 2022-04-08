using Atom.Design.Hosting;
using Atom.Design.Reflection.Metadata;
using System.IO;

namespace Atom.Design.Services
{
    public static class DesignerExtensions
    {
        public static TypeReference GetTypeReference(this IObjectDesigner designer)
        {
            IDocument document = designer.Document;
            string namespaceName = document.Namespace;
            string documentName = Path.GetFileNameWithoutExtension(document.Name);
            string className = string.Concat(documentName, "Class");
            AssemblyReference assembly = new AssemblyReference(document.Project.AssemblyName);
            TypeReference type = new TypeReference(className, namespaceName, assembly);
            return type;
        }

        public static string GetMethodName(this Method method)
        {
            string methodName = Path.GetFileNameWithoutExtension(method.Document.Name);
            return methodName;
        }
    }
}
