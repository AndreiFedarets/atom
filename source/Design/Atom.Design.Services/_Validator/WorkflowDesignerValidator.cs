namespace Atom.Design.Services
{
    public sealed class WorkflowDesignerValidator : MethodDesignerValidator
    {
        public override bool Validate(IObjectDesigner designer)
        {
            Workflow workflowDesigner = designer as Workflow;
            if (workflowDesigner == null)
            {
                return false;
            }
            return Validate(workflowDesigner);
        }
    }
}
