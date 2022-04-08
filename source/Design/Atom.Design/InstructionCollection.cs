using Atom.Design.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Atom.Design
{
    public class InstructionCollection : ItemsControl, IEnumerable<Instruction>, IValueScopeCollection
    {
        private static readonly DependencyPropertyKey SelectionPropertyKey;
        public static readonly DependencyProperty SelectionProperty;

        static InstructionCollection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InstructionCollection), new FrameworkPropertyMetadata(typeof(InstructionCollection)));
            SelectionPropertyKey = DependencyProperty.RegisterReadOnly("Selection", typeof(Instruction), typeof(InstructionCollection), new PropertyMetadata(null));
            SelectionProperty = SelectionPropertyKey.DependencyProperty;
        }

        public Instruction Selection
        {
            get { return (Instruction)GetValue(SelectionProperty); }
            private set { SetValue(SelectionPropertyKey, value); }
        }

        public IEnumerable<IValueScope> Scopes
        {
            get { return Items.OfType<IValueScope>(); }
        }

        public IEnumerable<IValueSource> Sources
        {
            get { return Scopes.SelectMany(x => x.Sources); }
        }

        public IEnumerable<IValueConsumer> Consumers
        {
            get { return Scopes.SelectMany(x => x.Consumers); }
        }

        public void Add(Instruction item)
        {
            if (item != null && !Items.Contains(item))
            {
                int index = Items.Count;
                //if (Selection != null)
                //{
                //    index = Items.IndexOf(Selection) + 1;
                //}
                Insert(index, item);
            }
        }

        public void Insert(int index, Instruction item)
        {
            if (item != null && !Items.Contains(item))
            {
                Items.Insert(index, item);
                Select(item);
                DesignerEvents.RaiseDesignerChanged(this);
            }
        }

        public void Remove(Instruction item)
        {
            if (item != null && Items.Contains(item))
            {
                Items.Remove(item);
                DesignerHelpers.UnbindLocalValueSources(item, this);
                DesignerEvents.RaiseDesignerChanged(this);
            }
        }

        public void MoveUp(Instruction item)
        {

        }

        public void MoveDown(Instruction item)
        {

        }

        public void Select(Instruction item)
        {
            if (Items.Contains(item) && !ReferenceEquals(Selection, item))
            {
                if (Selection != null)
                {
                    Selection.IsSelected = false;
                }
                Selection = item;
                if (Selection != null)
                {
                    Selection.IsSelected = true;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            return Items.OfType<Instruction>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                case Key.Up:
                    //int index = Items.Count;
                    //if (Selection != null)
                    //{
                    //    index = Items.IndexOf(Selection);
                    //    index--;
                    //    Instruction selection = (Instruction)Items.GetItemAt(index);
                    //    Select(selection);
                    //}
                    break;
                case Key.Down:
                    break;
                case Key.Delete:
                    if (Selection != null)
                    {
                        Remove(Selection);
                    }
                    break;
            }
        }
    }
}
