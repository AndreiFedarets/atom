using Atom.Design.Services;
using System;
using System.Collections.Generic;

namespace Atom.Design.Extension.Desktop
{
    public sealed class ControlTypeExtension : ITypeExtension
    {
        private readonly Layex.ViewModels.IViewModelManager _viewModelManager;

        public ControlTypeExtension(Layex.ViewModels.IViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
            Types = new TypeAdapter[] { new ButtonTypeAdapter(), new TextBoxTypeAdapter(), new WindowTypeAdapter() };
        }

        public string DisplayName
        {
            get { return "Desktop Element"; }
        }

        public IEnumerable<TypeAdapter> Types { get; private set; }

        public object CreateInteractive()
        {
            throw new NotImplementedException();
        }

        public object EditInteractive(object value)
        {
            throw new NotImplementedException();
        }

        public void InsertInteractive(Table table)
        {
            _viewModelManager.Activate<ViewModels.BrowseViewModel, Table>(table);
        }
    }
}
