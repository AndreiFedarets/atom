namespace Atom
{
    internal class ActionArgumentBinder : IActionArgumentBinder
    {
        private readonly IReflectionFacade _reflection;

        public ActionArgumentBinder(IReflectionFacade reflection)
        {
            _reflection = reflection;
        }

        public void AutoBindInputArguments(IInstance targetAction, IAction source)
        {
            int index = source.IndexOf(targetAction);
            for (int i = index - 1; i >= 0; i--)
            {
                IActionInstance sourceAction = source[i];
                foreach (IArgument sourceArgument in sourceAction.Arguments)
                {
                    //in source action we need output arguments only, skip the rest
                    if (sourceArgument.Direction != Direction.Output)
                    {
                        continue;
                    }
                    foreach (IArgument targetArgument in targetAction.Arguments)
                    {
                        //in target argument we need input arguments only that are not binded yet, skip the rest
                        if (targetArgument.Direction != Direction.Input || targetArgument.ValueBinding.IsBinded)
                        {
                            continue;
                        }
                        if (_reflection.IsTypeAssignableFrom(sourceArgument.Type, targetArgument.Type))
                        {
                            targetArgument.ValueBinding = CreateBindingFrom(sourceAction, sourceArgument);
                        }
                    }
                }
            }
        }

        private IValueBinding CreateBindingFrom(IActionInstance action, IArgument argument)
        {
            IDataSourceValueBinding valueBinding = new DataSourceValueBinding(action.Uid, argument.Name);
            return valueBinding;

        }
    }
}
