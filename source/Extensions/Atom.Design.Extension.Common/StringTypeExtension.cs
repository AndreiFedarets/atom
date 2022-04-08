using Atom.Design.Services;
using System;
using System.Collections.Generic;

namespace Atom.Design.Extension.Common
{
    public class StringTypeExtension : ITypeExtension
    {
        private readonly Layex.ViewModels.IViewModelManager _viewModelManager;

        public StringTypeExtension(Layex.ViewModels.IViewModelManager viewModelManager)
        {
            _viewModelManager = viewModelManager;
            Types = new[] { new StringTypeAdapter() };
        }

        public string DisplayName
        {
            get { return "Text"; }
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
            _viewModelManager.Activate<ViewModels.EditViewModel, Table>(table);
        }
    }
}
