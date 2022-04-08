using Atom.Design.Hosting;
using System;
using System.CodeDom;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class MSTestFrameworkAdapter : ITestFrameworkAdapter
    {
        public void Debug(Workflow workflow)
        {
            throw new NotImplementedException();
        }

        public void Execute(Workflow workflow)
        {
            throw new NotImplementedException();
        }

        public void GenerateAttributes(CodeTypeDeclaration type, CodeMemberMethod method, Workflow workflow)
        {
            CodeAttributeDeclaration testClassAttribute = new CodeAttributeDeclaration("Microsoft.VisualStudio.TestTools.UnitTesting.TestClass");
            type.CustomAttributes.Add(testClassAttribute);
            CodeAttributeDeclaration testMethodAttribute = new CodeAttributeDeclaration("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod");
            method.CustomAttributes.Add(testMethodAttribute);
        }

        public bool IsApplicable(IProject project)
        {
            return project.References.Any(x => string.Equals(x.AssemblyName.Name, "Microsoft.VisualStudio.TestPlatform.TestFramework", StringComparison.OrdinalIgnoreCase));
        }
    }
}
