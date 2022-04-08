using Atom.Design.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    [Guid("EEA5BDD5-1AC2-4A32-9A40-CD547BEC45A9")]
    public class TableDesignerVisualBasicGenerator : BaseCodeGenerator
    {
        public override string GetDefaultExtension()
        {
            return Design.Constants.TableDesignerDocumentExtension + DocumentExtension.GetCodeExtension(CodeLanguage.VisualBasic);
        }
    }
}
