using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("34476C31-DB20-4E0B-9D8E-AA0CB857E643")]
    public class WorkflowDesignerCSharpGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.WorkflowDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.CSharp);
        }
    }
}
