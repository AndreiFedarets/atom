using Atom.Design.Hosting;

namespace Atom.Design.Services
{
    public interface ITestFrameworkService
    {
        ITestFrameworkAdapter GetAdapter(IProject project);
    }
}
