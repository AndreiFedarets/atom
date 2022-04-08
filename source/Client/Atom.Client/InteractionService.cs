using Atom.Client.ViewModels;
using Atom.Design;
using Atom.Design.Services;

namespace Atom.Client
{
    public class InteractionService : IInteractionService
    {
        protected readonly Layex.ViewModels.IViewModelManager ViewModelManager;

        public InteractionService(Layex.ViewModels.IViewModelManager viewModelManager)
        {
            ViewModelManager = viewModelManager;
        }

        //public void ManageArgument(Argument argument)
        //{
        //    if (argument is InputArgument)
        //    {
        //        ViewModelManager.Activate<InputArgumentEditorViewModel, InputArgument>(argument as InputArgument);
        //    }
        //    else if (argument is OutputArgument)
        //    {

        //    }
        //}

        //public void Initialize()
        //{
        //    _eventRouter.InsertInstruction += (s, e) => InsertInstruction((Method)s);
        //    _eventRouter.ManageArgumentSource += (s, e) => ManageArgument((InputArgument)s);
        //    _eventRouter.InsertTableValue += (s, e) => InsertTableValue((Table)s);
        //}

        //private void InsertTableValue(Table table)
        //{
        //    ViewModels.TypeExtensionSelectorViewModel viewModel = ViewModelManager.Activate<ViewModels.TypeExtensionSelectorViewModel>();
        //    if (viewModel.DialogResult.HasValue && viewModel.DialogResult.Value)
        //    {
        //        ITypeExtension extension = viewModel.Extension;
        //        extension.InsertInteractive(table);
        //    }
        //}

        //private void ManageArgument(InputArgument argument)
        //{
        //    ViewModelManager.Activate<ViewModels.InputArgumentEditorViewModel, InputArgument>(argument);
        //}

        //private void InsertInstruction(Method method)
        //{
        //    //ViewModels.ActionExplorerViewModel viewModel = ViewModelManager.Activate<ViewModels.ActionExplorerViewModel, IProject>(method.Document.Project);
        //    //if (viewModel.DialogResult.HasValue && viewModel.DialogResult.Value)
        //    //{
        //    //    IAction action = viewModel.Action;
        //    //    if (action != null)
        //    //    {
        //    //        //InvokeInstruction invoke = DesignerHelpers.InsertAction(method, action);
        //    //        //_eventRouter.Subscribe(invoke);
        //    //    }
        //    //}
        //}
    }
}
