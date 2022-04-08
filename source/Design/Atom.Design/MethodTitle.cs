using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Atom.Design
{
    public sealed class MethodTitle : ItemsControl, IEnumerable<UIElement>
    {
        private readonly ParameterCollection _parameters;

        static MethodTitle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MethodTitle), new FrameworkPropertyMetadata(typeof(MethodTitle)));
        }

        public MethodTitle(ParameterCollection parameters)
        {
            _parameters = parameters;
            //TitleTextBlock - Parameter - TitleTextBlock  - Parameter - TitleTextBlock...
            //Insert initial block
            Items.Add(new TitleText());
            foreach (Parameter parameter in _parameters)
            {
                Items.Add(parameter);
                Items.Add(new TitleText());
            }
            //((INotifyCollectionChanged)_parameters).CollectionChanged += OnParameterCollectionChanged;
        }
        
        private void OnParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Parameter parameter in e.NewItems)
                    {
                        InsertParameter(parameter, e.NewStartingIndex);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Parameter parameter in e.OldItems)
                    {
                        RemoveParameter(parameter);
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void InsertParameter(Parameter parameter, int parameterIndex)
        {
            //TitleTextBlock - Parameter - TitleTextBlock  - Parameter - TitleTextBlock...
            int insertIndex = parameterIndex * 2 + 1;
            TitleText nextBlock = new TitleText();
            Items.Insert(insertIndex, nextBlock);
            Items.Insert(insertIndex, parameter);
        }

        private void RemoveParameter(Parameter parameter)
        {
            //TitleTextBlock - Parameter - TitleTextBlock  - Parameter - TitleTextBlock...
            //Merge TitleTextBlock's text and remove Parameter and next TitleTextBlock
            int parameterIndex = Items.IndexOf(parameter);
            TitleText previousBlock = (TitleText)Items[parameterIndex - 1];
            TitleText nextBlock = (TitleText)Items[parameterIndex + 1];
            previousBlock.Text = string.Concat(previousBlock.Text, " ", nextBlock.Text);
            Items.Remove(parameter);
            Items.Remove(nextBlock);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (object block in Items)
            {
                if (block is TitleText)
                {
                    TitleText titleBlock = (TitleText)block;
                    builder.Append(titleBlock.Text);
                }
                else if (block is InputParameter)
                {
                    InputParameter inputParameter = (InputParameter)block;
                    builder.Append(" {");
                    builder.Append(inputParameter.ValueName);
                    builder.Append("} ");
                }
                else if (block is OutputParameter)
                {
                    OutputParameter outputParameter = (OutputParameter)block;
                    builder.Append(" {");
                    builder.Append(outputParameter.ValueName);
                    builder.Append("} ");
                }
            }
            return builder.ToString();
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return Items.OfType<UIElement>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
