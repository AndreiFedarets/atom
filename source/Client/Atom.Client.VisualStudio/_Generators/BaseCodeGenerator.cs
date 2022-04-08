using Atom.Design;
using Atom.Design.Services;
using Atom.Design.Hosting;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System.Text;

namespace Atom.Client.VisualStudio
{
    public abstract class BaseCodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            string outputFileContent = string.Empty;
            ISolution solution = Services.Workspace.Solution;
            if (solution != null)
            {
                IDocument document = solution.FindDocument(inputFileName);
                if (document != null)
                {
                    IObjectDesigner designer = Services.Serializer.Read(document);
                    if (Services.Validator.Validate(designer))
                    {
                        outputFileContent = Services.CodeGenerator.Generate(designer);
                    }
                }
            }
            return Encoding.UTF8.GetBytes(outputFileContent);
        }
    }
}
