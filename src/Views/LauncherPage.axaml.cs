using System;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SourceGit.Views
{
    public partial class LauncherPage : UserControl
    {
        public LauncherPage()
        {
            InitializeComponent();
        }

        private void OnPopupSure(object _, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LauncherPage page)
                page.ProcessPopup();

            e.Handled = true;
        }

        private void OnPopupCancel(object _, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.LauncherPage page)
                page.CancelPopup();

            e.Handled = true;
        }

        private void OnMaskClicked(object sender, PointerPressedEventArgs e)
        {
            OnPopupCancel(sender, e);
        }

        private void OnCopyNotification(object sender, RoutedEventArgs e)
        {
            if (sender is Button { DataContext: Models.Notification notice })
                App.CopyText(notice.Message);

            e.Handled = true;
        }

        private void OnDismissNotification(object sender, RoutedEventArgs e)
        {
            if (sender is Button { DataContext: Models.Notification notice } &&
                DataContext is ViewModels.LauncherPage page)
                page.Notifications.Remove(notice);

            e.Handled = true;
        }

        private void OnPopupDataContextChanged(object sender, EventArgs e)
        {
            if (sender is ContentPresenter presenter)
            {
                if (presenter.DataContext is not ViewModels.Popup)
                {
                    presenter.Content = null;
                    return;
                }

                var dataTypeName = presenter.DataContext.GetType().FullName;
                if (string.IsNullOrEmpty(dataTypeName))
                {
                    presenter.Content = null;
                    return;
                }

                var viewTypeName = dataTypeName.Replace(".ViewModels.", ".Views.");
                var viewType = Type.GetType(viewTypeName);
                if (viewType == null)
                {
                    presenter.Content = null;
                    return;
                }

                var view = Activator.CreateInstance(viewType);
                presenter.Content = view;
            }
        }
    }
}
