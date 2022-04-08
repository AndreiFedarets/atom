using Atom.Design;

namespace Atom.Client.Win.ViewModels
{
    public class CreateProjectViewModel : ViewModel
    {
        private readonly ISolution _solution;
        private string _projectName;

        public CreateProjectViewModel(ISolution solution)
        {
            _solution = solution;
            _projectName = Properties.Resources.ProjectDefaultName;
        }

        public override string DisplayName
        {
            get { return "Create Project"; }
            set { }
        }

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                NotifyOfPropertyChange(() => ProjectName);
            }
        }

        public void CreateProject()
        {
            _solution.Projects.Create(_projectName);
            TryClose(true);
        }
    }
}
