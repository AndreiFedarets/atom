using Atom.Design.Hosting;
using System;
using System.CodeDom;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class NUnitFrameworkAdapter : ITestFrameworkAdapter
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
            CodeAttributeDeclaration testClassAttribute = new CodeAttributeDeclaration("NUnit.Framework.TestFixtureAttribute");
            type.CustomAttributes.Add(testClassAttribute);
            CodeAttributeDeclaration testMethodAttribute = new CodeAttributeDeclaration("NUnit.Framework.TestAttribute");
            method.CustomAttributes.Add(testMethodAttribute);
        }

        public bool IsApplicable(IProject project)
        {
            return project.References.Any(x => string.Equals(x.AssemblyName.Name, "NUnit.Framework", StringComparison.OrdinalIgnoreCase));
        }
    }
}
