using Atom.Design.Hosting;
using System;
using System.CodeDom;

namespace Atom.Design.Services
{
    public class UnknownFrameworkAdapter : ITestFrameworkAdapter
    {
        public void Debug(Workflow workflow)
        {
            
        }

        public void Execute(Workflow workflow)
        {
            
        }

        public void GenerateAttributes(CodeTypeDeclaration type, CodeMemberMethod method, Workflow workflow)
        {
            
        }

        public bool IsApplicable(IProject project)
        {
            return true;
        }
    }
}
