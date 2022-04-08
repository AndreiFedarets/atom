using System;

namespace Atom.Client.Views
{
    public partial class ActionExplorerView
    {
        public ActionExplorerView()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            SearchTextBox.Focus();
        }

        private void Close(bool? dialogResult = null)
        {
            Caliburn.Micro.PlatformProvider.Current.GetViewCloseAction(DataContext, new[] { this }, dialogResult)();
        }

        private void OnViewPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Escape:
                    Close();
                    break;
            }
        }

        private void OnSearchTextBoxPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    if (ActionsListView.SelectedIndex > 0)
                    {
                        ActionsListView.SelectedIndex--;
                    }
                    break;
                case System.Windows.Input.Key.Down:
                    if (ActionsListView.SelectedIndex < ActionsListView.Items.Count)
                    {
                        ActionsListView.SelectedIndex++;
                    }
                    break;
                case System.Windows.Input.Key.Enter:
                    SubmitInternal();
                    break;
                case System.Windows.Input.Key.Escape:
                    Close();
                    e.Handled = true;
                    break;
            }
        }

        private void SubmitInternal()
        {
            dynamic viewModel = DataContext;
            viewModel.Submit();
        }

        private void OnActionsListViewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SubmitInternal();
        }
    }
}
