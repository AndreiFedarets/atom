using Atom.Design.Hosting;
using System.CodeDom;

namespace Atom.Design.Services
{
    public interface ITestFrameworkAdapter
    {
        void Debug(Workflow workflow);

        void Execute(Workflow workflow);

        bool IsApplicable(IProject project);

        void GenerateAttributes(CodeTypeDeclaration type, CodeMemberMethod method, Workflow workflow);
    }
}
