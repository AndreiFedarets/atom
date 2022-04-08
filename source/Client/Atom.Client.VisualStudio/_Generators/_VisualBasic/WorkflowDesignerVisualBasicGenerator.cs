using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("DB713A98-5088-4A43-87B6-08CBB0BB54E6")]
    public class WorkflowDesignerVisualBasicGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.WorkflowDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.VisualBasic);
        }
    }
}
