using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Atom.Client
{
    public static class TreeViewExtensions
    {
        public static void SelectItem(this TreeView treeView, object item)
        {
            if (ReferenceEquals(treeView.SelectedItem, item))
            {
                return;
            }
            ItemContainerGenerator currentContainer = treeView.ItemContainerGenerator;
            if (currentContainer.Status != GeneratorStatus.ContainersGenerated)
            {
                EventHandler handler = null;
                handler = delegate
                {
                    if (currentContainer.Status == GeneratorStatus.ContainersGenerated)
                    {
                        ExpandAndSelectItem(treeView, item);
                        currentContainer.StatusChanged -= handler;
                    }
                };

                currentContainer.StatusChanged += handler;
            }
            else
            {
                ExpandAndSelectItem(treeView, item);
            }
        }

        private static bool ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect)
        {
            ITreeViewItem treeViewItem = itemToSelect as ITreeViewItem;
            if (treeViewItem != null)
            {
                return ExpandAndSelectTreeViewItem(parentContainer, treeViewItem);
            }
            return ExpandAndSelectObjectItem(parentContainer, itemToSelect);
        }

        private static bool ExpandAndSelectTreeViewItem(ItemsControl parentContainer, ITreeViewItem itemToSelect)
        {
            List<ITreeViewItem> itemsToSelect = new List<ITreeViewItem>();
            ITreeViewItem tempItem = itemToSelect;
            while (tempItem != null)
            {
                itemsToSelect.Insert(0, tempItem);
                tempItem = tempItem.Parent;
            }
            foreach (ITreeViewItem treeViewItem in itemsToSelect)
            {
                bool treeViewItemSelected = false;
                foreach (object currentItem in parentContainer.Items)
                {
                    TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(currentItem) as TreeViewItem;

                    //if the data item matches the item we want to select, set the corresponding
                    //TreeViewItem IsSelected to true
                    if (currentItem.Equals(treeViewItem) && currentContainer != null)
                    {
                        if (currentItem.Equals(itemToSelect))
                        {
                            currentContainer.IsSelected = true;
                            currentContainer.BringIntoView();
                            currentContainer.Focus();
                        }
                        else
                        {
                            currentContainer.IsExpanded = true;
                            parentContainer = currentContainer;
                            if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                            {
                                DispatcherExtensions.DoEvents();
                            }
                        }
                        //the item was found
                        treeViewItemSelected = true;
                        break;
                    }
                }
                if (!treeViewItemSelected)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Finds the provided object in an ItemsControl's children and selects it
        /// </summary>
        /// <param name="parentContainer">The parent container whose children will be searched for the selected item</param>
        /// <param name="itemToSelect">The item to select</param>
        /// <returns>True if the item is found and selected, false otherwise</returns>
        private static bool ExpandAndSelectObjectItem(ItemsControl parentContainer, object itemToSelect)
        {
            //check all items at the current level
            foreach (object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if the data item matches the item we want to select, set the corresponding
                //TreeViewItem IsSelected to true
                if (item == itemToSelect && currentContainer != null)
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();

                    //the item was found
                    return true;
                }
            }

            //if we get to this point, the selected item was not found at the current level, so we must check the children
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if children exist
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //keep track of if the TreeViewItem was expanded or not
                    bool wasExpanded = currentContainer.IsExpanded;

                    //expand the current TreeViewItem so we can check its child TreeViewItems
                    currentContainer.IsExpanded = true;

                    //if the TreeViewItem child containers have not been generated, we must listen to
                    //the StatusChanged event until they are
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                if (ExpandAndSelectObjectItem(currentContainer, itemToSelect) == false)
                                {
                                    //The assumption is that code executing in this EventHandler is the result of the parent not
                                    //being expanded since the containers were not generated.
                                    //since the itemToSelect was not found in the children, collapse the parent since it was previously collapsed
                                    currentContainer.IsExpanded = false;
                                }

                                //remove the StatusChanged event handler since we just handled it (we only needed it once)
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        };
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the containers have been generated, so look for item to select in the children
                    {
                        if (ExpandAndSelectObjectItem(currentContainer, itemToSelect) == false)
                        {
                            //restore the current TreeViewItem's expanded state
                            currentContainer.IsExpanded = wasExpanded;
                        }
                        else //otherwise the node was found and selected, so return true
                        {
                            return true;
                        }
                    }
                }
            }

            //no item was found
            return false;
        }
    }
}
