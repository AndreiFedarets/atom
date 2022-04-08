using Atom.Client.Desktop.ViewModels;
using Atom.Client.ViewModels;
using Atom.Design;
using System.Windows.Input;

namespace Atom.Client.Desktop
{
    internal sealed class KeyboardCommands
    {
        private readonly KeyboardProcessor _keyboardProcessor;
        private readonly IViewManager _viewManager;

        public KeyboardCommands(KeyboardProcessor keyboardProcessor, IViewManager viewManager)
        {
            _keyboardProcessor = keyboardProcessor;
            _viewManager = viewManager;
        }

        public void Initialize()
        {
            _keyboardProcessor.Initialize();
            _keyboardProcessor.RegisterCommand(ModifierKeys.Control, Key.S, SaveActiveDocument);
        }

        private void SaveActiveDocument()
        {
            DocumentCollectionViewModel documentsViewModel = _viewManager.GetDocuments();
            DesignerViewModel designerViewModel = documentsViewModel.ActiveItem as DesignerViewModel;
            if (designerViewModel != null)
            {
                designerViewModel.Save();
            }
        }
    }
}
