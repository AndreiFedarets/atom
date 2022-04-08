using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("AFEB936A-6D53-419F-A7DD-EB4AE306F4A3")]
    public class TableDesignerCSharpGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.TableDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.CSharp);
        }
    }
}
