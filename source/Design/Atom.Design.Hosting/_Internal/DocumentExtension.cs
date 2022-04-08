using System;

namespace Atom.Design.Hosting
{
    public static class DocumentExtension
    {
        private const string CSharpCodeExtension = ".cs";
        private const string VisualBasicCodeExtension = ".vb";

        public static string GetCodeExtension(CodeLanguage language)
        {
            switch (language)
            {
                case CodeLanguage.CSharp:
                    return CSharpCodeExtension;
                case CodeLanguage.VisualBasic:
                    return VisualBasicCodeExtension;
                default:
                    throw new NotSupportedException($"Code language {language} is not supported");
            }
        }

        public static DocumentType GetDocumentType(IDocument document)
        {
            CodeLanguage language = document.Project.Language;
            switch (language)
            {
                case CodeLanguage.CSharp:
                    return document.FullName.EndsWith(CSharpCodeExtension, StringComparison.OrdinalIgnoreCase) ? DocumentType.Code : DocumentType.Other;
                case CodeLanguage.VisualBasic:
                    return document.FullName.EndsWith(VisualBasicCodeExtension, StringComparison.OrdinalIgnoreCase) ? DocumentType.Code : DocumentType.Other;
                default:
                    throw new NotSupportedException($"Code language {language} is not supported");
            }
        }

        public static CodeLanguage GetProjectLanguage(string language)
        {
            if (string.Equals(language, "C#", StringComparison.OrdinalIgnoreCase))
            {
                return CodeLanguage.CSharp;
            }
            throw new NotImplementedException();
        }
    }
}
