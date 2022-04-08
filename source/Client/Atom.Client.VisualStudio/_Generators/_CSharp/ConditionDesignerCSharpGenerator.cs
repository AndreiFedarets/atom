using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("048E5AD9-3531-46D6-9B09-9155CEB91884")]
    public class ActionDesignerCSharpGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.ActionDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.CSharp);
        }
    }
}
