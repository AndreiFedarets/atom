using System;

namespace Atom.Design
{
    internal sealed class Application : IApplication
    {
        private readonly object _solutionLock;

        public Application()
        {
            _solutionLock = new object();
        }

        public ISolution CurrentSolution { get; private set; }

        public IAssemblyCollection Assemblies
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<SolutionEventArgs> SolutionOpened;

        public event EventHandler<SolutionEventArgs> SolutionClosed;

        public void OpenSolution(string fullName)
        {
            CloseSolution();
            ISolution solution;
            lock (_solutionLock)
            {
                solution = Solution.Open(fullName);
                CurrentSolution = solution;
            }
            SolutionEventArgs.RaiseEvent(SolutionOpened, this, solution);
        }

        public void CreateSolution(string path, string name)
        {
            CloseSolution();
            ISolution solution;
            lock (_solutionLock)
            {
                solution = Solution.Create(path, name);
                CurrentSolution = solution;
                SaveSolution();
            }
            SolutionEventArgs.RaiseEvent(SolutionOpened, this, solution);
        }

        public void SaveSolution()
        {

        }

        public void CloseSolution()
        {
            ISolution solution = null;
            lock (_solutionLock)
            {
                if (CurrentSolution != null)
                {
                    solution = CurrentSolution;
                    CurrentSolution = null;
                }
            }
            if (solution != null)
            {
                SolutionEventArgs.RaiseEvent(SolutionClosed, this, solution);
            }
        }

        internal void Run()
        {
            //_assemblies.Load(_assemblyManager);
        }

        //private readonly ApplicationAssemblyCollection _assemblies;
        //private readonly Directories _directoies;
        //private readonly IAssemblyLoader _assemblyLoader;
        //private readonly IAssemblyManager _assemblyManager;
        //private Solution _currentSolution;

        //public Application()
        //{
        //    _lock = new object();
        //    IConfigurationProvider configurationProvider = ConfigurationProvider.Current;
        //    _directoies = new Directories(configurationProvider);
        //    IReflectionFacade reflection = new ReflectionFacade();
        //    _assemblyLoader = new CommonAssemblyLoader(reflection);
        //    _assemblyManager = new AssemblyManager(reflection);
        //    _assemblies = new ApplicationAssemblyCollection();
        //    //ArgumentBinder = new ActionArgumentBinder(reflection);
        //    _treeWalker = new ActionTreeWalker();
        //    _applicationScope = new Scope();
        //}

        //public IDirectories Directories
        //{
        //    get { return _directoies; }
        //}

        //public IAssemblyCollection Assemblies
        //{
        //    get { return _assemblies; }
        //}

        //private void ConfigureContainer()
        //{

        //}

        //public IActionArgumentBinder ArgumentBinder { get; private set; }
    }
}
