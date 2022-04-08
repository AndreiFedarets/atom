using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Client.Desktop.ViewModels.SolutionTree
{
    public abstract class ContainerItemViewModel : ItemViewModel
    {
        protected readonly ISolutionItemFilter ItemFilter;
        private readonly Lazy<ObservableCollection<ItemViewModel>> _items;

        public ContainerItemViewModel(ItemViewModel parent, ISolutionItemFilter itemFilter)
            : base(parent)
        {
            ItemFilter = itemFilter;
            _items = new Lazy<ObservableCollection<ItemViewModel>>(GetFilteredItems);
        }

        public IEnumerable<ItemViewModel> Items
        {
            get { return _items.Value; }
        }

        public bool IsExpanded { get; set; }

        private ObservableCollection<ItemViewModel> GetFilteredItems()
        {
            ObservableCollection<ItemViewModel> items = new ObservableCollection<ItemViewModel>();
            foreach (ItemViewModel item in GetItems())
            {
                if (ItemFilter.Display(item))
                {
                    items.Add(item);
                }
            }
            return items;
        }

        protected abstract IEnumerable<ItemViewModel> GetItems();
    }
}
