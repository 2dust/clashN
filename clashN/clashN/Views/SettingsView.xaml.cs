using clashN.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

namespace clashN.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        public SettingsView()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();

            Global.SubConvertUrls.ForEach(it =>
            {
                cmbSubConvertUrl.Items.Add(it);
            });
            Global.Languages.ForEach(it =>
            {
                cmbCurrentLanguage.Items.Add(it);
            });
            Global.IEProxyProtocols.ForEach(it =>
            {
                cmbsystemProxyAdvancedProtocol.Items.Add(it);
            });
            Global.LogLevel.ForEach(it =>
            {
                cmbLogLevel.Items.Add(it);
            });

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.MixedPort, v => v.txtMixedPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SocksPort, v => v.txtSocksPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.HttpPort, v => v.txtHttpPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.APIPort, v => v.txtAPIPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AllowLANConn, v => v.togAllowLANConn.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableIpv6, v => v.togEnableIpv6.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.LogLevel, v => v.cmbLogLevel.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableMixinContent, v => v.togEnableMixinContent.IsChecked).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.EditMixinContentCmd, v => v.btnEditMixinContent).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Swatches, v => v.cmbSwatches.ItemsSource);
                this.Bind(ViewModel, vm => vm.SelectedSwatch, v => v.cmbSwatches.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.ColorModeDark, v => v.togDarkMode.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.CurrentLanguage, v => v.cmbCurrentLanguage.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AutoRun, v => v.togAutoRun.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableStatistics, v => v.togEnableStatistics.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableSecurityProtocolTls13, v => v.togEnableSecurityProtocolTls13.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.autoUpdateSubInterval, v => v.txtautoUpdateSubInterval.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.autoDelayTestInterval, v => v.txtautoDelayTestInterval.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SubConvertUrl, v => v.cmbSubConvertUrl.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SetLoopbackCmd, v => v.btnSetLoopback).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SetGlobalHotkeyCmd, v => v.btnSetGlobalHotkey).DisposeWith(disposables);


                this.Bind(ViewModel, vm => vm.systemProxyExceptions, v => v.txtsystemProxyExceptions.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.systemProxyAdvancedProtocol, v => v.cmbsystemProxyAdvancedProtocol.Text).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.btnSave).DisposeWith(disposables);

            });
        }
    }
}
