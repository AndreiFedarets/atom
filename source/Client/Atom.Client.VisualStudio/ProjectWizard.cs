using Microsoft.VisualStudio.TemplateWizard;
using System.Collections.Generic;

namespace Atom.Client.VisualStudio
{
    public sealed class ProjectWizard : IWizard
    {
        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
        }
        
        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            throw new System.NotImplementedException();
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            replacementsDictionary[VisualStudioConstants.TemplateVariables.AtomDirVariableName] = Design.Common.Environment.BasePath;
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
