using ClashN.Mode;
using ClashN.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClashN.Views
{
    /// <summary>
    /// Interaction logic for ProfilesView.xaml
    /// </summary>
    public partial class ProfilesView
    {
        public ProfilesView()
        {
            InitializeComponent();
            ViewModel = new ProfilesViewModel();
            Locator.CurrentMutable.RegisterLazySingleton(() => ViewModel, typeof(ProfilesViewModel));

            lstProfiles.PreviewMouseDoubleClick += lstProfiles_PreviewMouseDoubleClick;
            lstProfiles.PreviewMouseLeftButtonDown += LstProfiles_PreviewMouseLeftButtonDown;
            lstProfiles.MouseMove += LstProfiles_MouseMove;
            lstProfiles.DragEnter += LstProfiles_DragEnter;
            lstProfiles.Drop += LstProfiles_Drop;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.ProfileItems, v => v.lstProfiles.ItemsSource).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedSource, v => v.lstProfiles.SelectedItem).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.EditLocalFileCmd, v => v.menuEditLocalFile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.EditProfileCmd, v => v.menuEditProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddProfileCmd, v => v.menuAddProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddProfileViaScanCmd, v => v.menuAddProfileViaScan).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddProfileViaClipboardCmd, v => v.menuAddProfileViaClipboard).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ExportProfileCmd, v => v.menuExportProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ProfileQrcodeCmd, v => v.menuProfileQrcode).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SubUpdateCmd, v => v.menuSubUpdate).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SubUpdateSelectedCmd, v => v.menuSubUpdateSelected).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SubUpdateViaProxyCmd, v => v.menuSubUpdateViaProxy).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SubUpdateSelectedViaProxyCmd, v => v.menuSubUpdateSelectedViaProxy).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.RemoveProfileCmd, v => v.menuRemoveProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.CloneProfileCmd, v => v.menuCloneProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SetDefaultProfileCmd, v => v.menuSetDefaultProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.EditLocalFileCmd, v => v.menuEditLocalFile).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.ClearStatisticCmd, v => v.menuClearStatistic).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ProfileReloadCmd, v => v.menuProfileReload).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.AddProfileCmd, v => v.btnAddProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.AddProfileViaClipboardCmd, v => v.btnAddProfileViaClipboard).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SubUpdateViaProxyCmd, v => v.btnSubUpdateViaProxy).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.EditProfileCmd, v => v.btnEditProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SetDefaultProfileCmd, v => v.btnSetDefaultProfile).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ProfileQrcodeCmd, v => v.btnProfileQrcode).DisposeWith(disposables);
            });
        }

        private void ProfilesView_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.C)
                {
                    ViewModel?.ExportProfile2Clipboard();
                }
                else if (e.Key == Key.V)
                {
                    ViewModel?.AddProfilesViaClipboard(false);
                }
            }
            else
            {
                if (Keyboard.IsKeyDown(Key.F5))
                {
                    ViewModel?.RefreshProfiles();
                }
                else if (Keyboard.IsKeyDown(Key.Delete))
                {
                    ViewModel?.RemoveProfile();
                }
                else if (Keyboard.IsKeyDown(Key.Enter))
                {
                    ViewModel?.SetDefaultProfile();
                }
            }
        }

        private void lstProfiles_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel?.SetDefaultProfile();
        }

        #region Drag and Drop

        private Point startPoint = new Point();
        private int startIndex = -1;
        private string formatData = "ProfileItemModel";

        /// <summary>
        /// Helper to search up the VisualTree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="current"></param>
        /// <returns></returns>
        private static T? FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void LstProfiles_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get current mouse position
            startPoint = e.GetPosition(null);
        }

        private void LstProfiles_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                       Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                var listView = sender as ListView;
                if (listView == null) return;
                var listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null) return;           // Abort
                                                            // Find the data behind the ListViewItem
                ProfileItemModel item = (ProfileItemModel)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                if (item == null) return;                   // Abort
                                                            // Initialize the drag & drop operation
                startIndex = lstProfiles.SelectedIndex;
                DataObject dragData = new DataObject(formatData, item);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void LstProfiles_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(formatData) || sender != e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void LstProfiles_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(formatData) && sender == e.Source)
            {
                // Get the drop ListViewItem destination
                var listView = sender as ListView;
                if (listView == null) return;
                var listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null)
                {
                    // Abort
                    e.Effects = DragDropEffects.None;
                    return;
                }
                // Find the data behind the ListViewItem
                ProfileItemModel item = (ProfileItemModel)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                if (item == null) return;
                // Move item into observable collection
                // (this will be automatically reflected to lstView.ItemsSource)
                e.Effects = DragDropEffects.Move;

                ViewModel?.MoveProfile(startIndex, item);

                startIndex = -1;
            }
        }

        #endregion Drag and Drop
    }
}