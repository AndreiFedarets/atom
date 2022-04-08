using Atom.Design;
using Atom.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Client.Win.ViewModels
{
    public sealed class SelectAssemblyViewModel : ViewModel
    {
        public SelectAssemblyViewModel(IApplication application, IProject originator)
        {
            GlobalAssemblies = application.Assemblies.Select(x => new AssemblyWrapper(x)).ToArray();
            SolutionAssemblies = application.CurrentSolution.Projects.Where(x => x != originator).Select(x => new AssemblyWrapper(x.ShadowAssembly)).ToArray();
        }

        public IEnumerable<AssemblyWrapper> GlobalAssemblies { get; private set;}

        public IEnumerable<AssemblyWrapper> SolutionAssemblies { get; private set; }

        public IEnumerable<IAssembly> SelectedAssemblies
        {
            get
            {
                List<IAssembly> selectedAssemblies = new List<IAssembly>();
                selectedAssemblies.AddRange(GlobalAssemblies.Where(x => x.IsChecked).Select(x => x.Assembly));
                selectedAssemblies.AddRange(SolutionAssemblies.Where(x => x.IsChecked).Select(x => x.Assembly));
                return selectedAssemblies;
            }
        }

        public void Submit()
        {
            TryClose(true);
        }
    }

    public class AssemblyWrapper
    {
        public AssemblyWrapper(IAssembly assembly)
        {
            AssemblyInfo assemblyInfo = assembly.AssemblyInfo;
            Name = assemblyInfo.Name;
            Version = assemblyInfo.Version.ToString();
            Type = assemblyInfo.Type;
            Assembly = assembly;
        }

        public bool IsChecked { get; set; }

        public string Name { get; private set; }

        public string Version { get; private set; }

        public AssemblyType Type { get; private set; }

        public IAssembly Assembly { get; private set; }
    }
}
