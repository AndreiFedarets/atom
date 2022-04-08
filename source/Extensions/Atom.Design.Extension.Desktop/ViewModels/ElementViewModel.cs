using Atom.Client;
using Layex.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design.Extension.Desktop.ViewModels
{
    public sealed class ElementViewModel : ViewModel, ITreeViewItem, IEquatable<ElementViewModel>
    {
        private readonly ObservableCollection<ElementViewModel> _checkedElements;
        private readonly Lazy<List<ElementViewModel>> _children;
        private readonly ElementViewModel _parentViewModel;

        public ElementViewModel(Element element, ElementViewModel parent, ObservableCollection<ElementViewModel> checkedElements)
            : this(element, parent, true, checkedElements)
        {   
        }

        public ElementViewModel(Element element, ObservableCollection<ElementViewModel> checkedElements)
            : this(element, null, false, checkedElements)
        {
        }

        private ElementViewModel(Element element, ElementViewModel parentViewModel, bool parentSpecified, ObservableCollection<ElementViewModel> checkedElements)
        {
            Element = element;
            _checkedElements = checkedElements;
            _children = new Lazy<List<ElementViewModel>>(InitializeChildren);
            Name = ControlFactory.GenerateSafeName(Element);
            if (parentSpecified)
            {
                _parentViewModel = parentViewModel;
            }
            else
            {
                if (element.Parent != null)
                {
                    _parentViewModel = new ElementViewModel(element.Parent, checkedElements);
                }
            }
        }

        public Element Element { get; private set; }

        public bool IsChecked
        {
            get { return _checkedElements.Contains(this); }
            set
            {
                if (value)
                {
                    _checkedElements.Add(this);
                }
                else
                {
                    _checkedElements.Remove(this);
                }
                NotifyOfPropertyChange(() => IsChecked);
            }
        }
        
        public new string Name { get; set; }

        public IEnumerable<ElementViewModel> Children
        {
            get { return _children.Value; }
        }

        ITreeViewItem ITreeViewItem.Parent
        {
            get { return _parentViewModel; }
        }

        private List<ElementViewModel> InitializeChildren()
        {
            List<ElementViewModel> collection = new List<ElementViewModel>();
            foreach (Element childElement in Element)
            {
                ElementViewModel childViewModel = new ElementViewModel(childElement, this, _checkedElements);
                collection.Add(childViewModel);
            }
            return collection;
        }

        public bool Equals(ElementViewModel other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Element.Equals(other.Element);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ElementViewModel);
        }

        public override int GetHashCode()
        {
            return Element.GetHashCode();
        }
    }
}
