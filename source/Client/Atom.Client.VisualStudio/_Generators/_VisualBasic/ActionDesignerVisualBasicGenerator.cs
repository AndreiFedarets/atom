using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("51EBBF7F-A13B-424F-ACC4-1C0A33F65EA1")]
    public class ActionDesignerVisualBasicGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.ActionDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.VisualBasic);
        }
    }
}
