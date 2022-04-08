namespace Atom.Design.Services
{
    public sealed class ActionDesignerValidator : MethodDesignerValidator
    {
        public override bool Validate(IObjectDesigner designer)
        {
            Action actionDesigner = designer as Action;
            if (actionDesigner == null)
            {
                return false;
            }
            return Validate(actionDesigner);
        }
    }
}
