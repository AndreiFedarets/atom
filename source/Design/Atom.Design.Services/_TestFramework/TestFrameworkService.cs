using Atom.Design.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class TestFrameworkService : ITestFrameworkService
    {
        private readonly List<ITestFrameworkAdapter> _adapters;

        public TestFrameworkService()
        {
            _adapters = new List<ITestFrameworkAdapter>()
            {
                new MSTestFrameworkAdapter(),
                new NUnitFrameworkAdapter(),
                new UnknownFrameworkAdapter()
            };
        }

        public ITestFrameworkAdapter GetAdapter(IProject project)
        {
            return _adapters.FirstOrDefault(x => x.IsApplicable(project));
        }
    }
}
