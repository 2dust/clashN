using clashN.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace clashN.Views
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

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.ProfileItems, v => v.lstProfiles.ItemsSource);

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
    }
}
