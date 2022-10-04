using clashN.Resx;
using clashN.ViewModels;
using ReactiveUI;
using Splat;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Windows;

namespace clashN.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
            App.Current.SessionEnding += Current_SessionEnding;

            ViewModel = new MainWindowViewModel(MainSnackbar.MessageQueue!);
            Locator.CurrentMutable.RegisterLazySingleton(() => ViewModel, typeof(MainWindowViewModel));

            this.WhenActivated(disposables =>
            {
                //this.OneWayBind(ViewModel, vm => vm.GetDashboardView, v => v.dashboardTabItem.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetProxyView, v => v.proxiesTabItem.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetProfilesView, v => v.profilesTabItem.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetConnectionsView, v => v.connectionsTabItem.Content).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.GetSettingsView, v => v.settingsTabItem.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetHelpView, v => v.helpTabItem.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.GetPromotionView, v => v.promotionTabItem.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SpeedUpload, v => v.txtSpeedUpload.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SpeedDownload, v => v.txtSpeedDownload.Text).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.BlSystemProxyClear, v => v.menuSystemProxyClear.IsChecked).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BlSystemProxySet, v => v.menuSystemProxySet.IsChecked).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BlSystemProxyNothing, v => v.menuSystemProxyNothing.IsChecked).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SystemProxyClearCmd, v => v.menuSystemProxyClear).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SystemProxySetCmd, v => v.menuSystemProxySet).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SystemProxyNothingCmd, v => v.menuSystemProxyNothing).DisposeWith(disposables);


                this.OneWayBind(ViewModel, vm => vm.BlModeRule, v => v.menuModeRule.IsChecked).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BlModeGlobal, v => v.menuModeGlobal.IsChecked).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BlModeDirect, v => v.menuModeDirect.IsChecked).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.BlModeNothing, v => v.menuModeNothing.IsChecked).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ModeRuleCmd, v => v.menuModeRule).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ModeGlobalCmd, v => v.menuModeGlobal).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ModeDirectCmd, v => v.menuModeDirect).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ModeNothingCmd, v => v.menuModeNothing).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.AddProfileViaScanCmd, v => v.menuAddProfileViaScan).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SubUpdateCmd, v => v.menuSubUpdate).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SubUpdateViaProxyCmd, v => v.menuSubUpdateViaProxy).DisposeWith(disposables);

                //this.BindCommand(ViewModel, vm => vm.ExitCmd, v => v.menuExit).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ReloadCmd, v => v.btnReload).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.NotifyIcon, v => v.tbNotify.Icon).DisposeWith(disposables);
            });


            this.Title = $"{Utils.GetVersion()} - {(Utils.IsAdministrator() ? ResUI.RunAsAdmin : ResUI.NotRunAsAdmin)}";
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ViewModel?.ShowHideWindow(false);
        }

        private void tbNotify_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.ShowHideWindow(true);
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            tbNotify.Dispose();
            ViewModel?.MyAppExit(false);
        }

        private void Current_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            Utils.SaveLog("Current_SessionEnding");
            ViewModel?.MyAppExit(true);
        }
    }
}
