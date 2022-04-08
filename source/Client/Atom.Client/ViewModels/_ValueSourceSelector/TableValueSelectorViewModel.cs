using Atom.Design;
using Atom.Design.Services;
using Atom.Design.Hosting;
using Atom.Design.Reflection;
using Atom.Design.Reflection.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Client.ViewModels
{
    public sealed class TableValueSelectorViewModel : ValueSourceSelectorViewModel
    {
        private readonly IObjectExplorer _objectExplorer;
        private readonly InputArgument _inputArgument;
        private readonly IProject _project;
        private IEnumerable<ITable> _availableTables;
        private ITable _selectedTable;
        private IEnumerable<ITableValue> _availableValues;

        public TableValueSelectorViewModel(InputArgumentEditorViewModel parentViewModel, InputArgument inputArgument, IProject project, IObjectExplorer objectExplorer)
            : base(parentViewModel)
        {
            DisplayName = "Data Table";
            _inputArgument = inputArgument;
            _project = project;
            _objectExplorer = objectExplorer;
        }

        public IEnumerable<ITable> AvailableTables
        {
            get
            {
                if (_availableTables == null)
                {
                    _availableTables = _objectExplorer.GetAvailableTables(_project);
                    SelectedTable = _availableTables.FirstOrDefault();
                }
                return _availableTables;
            }
        }

        public ITable SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                if (_selectedTable != null)
                {
                    TypeReference valueType = _inputArgument.Parameter.ParameterType;
                    //AvailableValues = _selectedTable.SelectValues(valueType);
                }
                NotifyOfPropertyChange(() => SelectedTable);
            }
        }

        public IEnumerable<ITableValue> AvailableValues
        {
            get { return _availableValues; }
            private set
            {
                _availableValues = value;
                NotifyOfPropertyChange(() => AvailableValues);
            }
        }

        internal override void SelectCurrentValueSource()
        {
            //ITableValue tableValue = (ITableValue)_inputArgument.Source;
            //SelectedTable = AvailableTables.FirstOrDefault(x => x.Type.Equals(tableValue.Property.DeclaringType));
            //SelectedValue = AvailableValues?.FirstOrDefault(x => x.Equals(tableValue));
        }
    }
}
