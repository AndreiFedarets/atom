using Layex.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design.Extension.Desktop.ViewModels
{
    public class BrowseViewModel : ViewModel
    {
        private readonly ObservableCollection<ElementViewModel> _checkedElements;
        private readonly Table _table;
        private readonly ElementLocator _elementLocator;
        private readonly MouseTracker _mouseTracker;
        private IEnumerable<ElementViewModel> _windows;
        private ElementViewModel _selectedElement;

        public BrowseViewModel(Table table)
        {
            _table = table;
            _checkedElements = new ObservableCollection<ElementViewModel>();
            _elementLocator = new ElementLocator();
            _mouseTracker = new MouseTracker();
            _mouseTracker.InsertElement += OnElementDetailsTrackerInsertElement;
            _mouseTracker.SyncronizeElement += OnElementDetailsTrackerSyncronizeElement;
            RefreshWindows();
        }

        public IEnumerable<ElementViewModel> CheckedElements
        {
            get { return _checkedElements; }
        }

        public ElementViewModel SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                //if (_selectedElement != null)
                //{
                //    _selectedElement.Unhighlight();
                //}
                _selectedElement = value;
                NotifyOfPropertyChange(() => SelectedElement);
                //if (_selectedElement != null)
                //{
                //    _selectedElement.Highlight();
                //}
            }
        }

        public IEnumerable<ElementViewModel> Windows
        {
            get { return _windows; }
            private set
            {
                _windows = value;
                NotifyOfPropertyChange(() => Windows);
            }
        }

        public override void TryClose(bool? dialogResult = null)
        {
            SelectedElement = null;
            base.TryClose(dialogResult);
        }

        public void RefreshWindows()
        {
            List<ElementViewModel> viewModels = new List<ElementViewModel>();
            IEnumerable<Element> elements = _elementLocator.GetElements();
            foreach (Element element in elements)
            {
                ElementViewModel viewModel = new ElementViewModel(element, null, _checkedElements);
                viewModels.Add(viewModel);
            }
            Windows = viewModels;
        }

        public void Submit()
        {
            foreach (ElementViewModel elementViewModel in _checkedElements)
            {
                TableValue value = ControlFactory.CreateValue(elementViewModel.Name, elementViewModel.Element);
                _table.Add(value);
            }
            TryClose(true);
        }

        public override void Dispose()
        {
            base.Dispose();
            _mouseTracker.Dispose();
        }

        private void OnElementDetailsTrackerSyncronizeElement(object sender, Controls.ElementEventArgs e)
        {
            RefreshWindows();
            SelectedElement = new ElementViewModel(e.Element, _checkedElements);
        }

        private void OnElementDetailsTrackerInsertElement(object sender, Controls.ElementEventArgs e)
        {
            ElementViewModel elementViewModel = new ElementViewModel(e.Element, _checkedElements);
            _checkedElements.Add(elementViewModel);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
