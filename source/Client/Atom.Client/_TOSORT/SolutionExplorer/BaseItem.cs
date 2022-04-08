using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Atom.Client.Win.SolutionExplorer
{
    public abstract class BaseItem : PropertyChangedBase, INotifyCollectionChanged, IEnumerable<BaseItem>
    {
        private readonly BaseItem _parent;
        private ObservableCollection<BaseItem> _children;
        private List<ICommand> _commands;
        private bool _isExpanded;
        private bool _expandAdded;

        protected BaseItem(object underlyingObject, BaseItem parent)
        {
            UnderlyingObject = underlyingObject;
            _parent = parent;
        }

        internal object UnderlyingObject { get; private set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (_isExpanded && _parent != null)
                {
                    _parent.IsExpanded = _isExpanded;
                }
                NotifyOfPropertyChange(() => IsExpanded);
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public virtual ImageSource Icon
        {
            get
            {
                BitmapImage bitmapImage = null;
                try
                {
                    string uriString = string.Format("../Resources/SolutionExplorer/{0}.png", GetType().Name);
                    Uri uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
                    bitmapImage = new BitmapImage(uri);
                }
                catch (Exception exception)
                {
                    
                }
                return bitmapImage;
            }
        }

        public abstract string DisplayName { get; }

        public IEnumerable<ICommand> Commands
        {
            get
            {
                InitializeCommands();
                return _commands;
            }
        }

        protected void AddItem(BaseItem item)
        {
            InitializeChildren();
            _children.Add(item);
            if (_expandAdded)
            {
                item.IsExpanded = true;
            }
        }

        protected BaseItem Find(Predicate<BaseItem> match)
        {
            InitializeChildren();
            BaseItem item = _children.FirstOrDefault(x => match(x));
            return item;
        }

        protected void RemoveItem(BaseItem item)
        {
            InitializeChildren();
            _children.Remove(item);
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void FillChildren()
        {
        }

        protected virtual void FillCommands(List<ICommand> commands)
        {
        }

        private void InitializeChildren()
        {
            if (_children == null)
            {
                _children = new ObservableCollection<BaseItem>();
                _children.CollectionChanged += OnItemsCollectionChanged;
                _expandAdded = false;
                FillChildren();
                _expandAdded = true;
            }
        }

        private void InitializeCommands()
        {
            if (_commands == null)
            {
                _commands = new List<ICommand>();
                FillCommands(_commands);
            }
        }

        public IEnumerator<BaseItem> GetEnumerator()
        {
            InitializeChildren();
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
