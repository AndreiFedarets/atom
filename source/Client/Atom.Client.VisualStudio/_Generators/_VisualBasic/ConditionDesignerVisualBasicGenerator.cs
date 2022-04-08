using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("15A3954D-FDBF-45A8-8593-575D17A3555B")]
    public class ConditionDesignerVisualBasicGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.ConditionDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.VisualBasic);
        }
    }
}
