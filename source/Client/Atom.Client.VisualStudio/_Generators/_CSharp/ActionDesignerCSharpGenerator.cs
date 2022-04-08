using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("F68017D0-197D-4E14-9261-DC4FAAD1A197")]
    public class ConditionDesignerCSharpGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.ConditionDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.CSharp);
        }
    }
}
