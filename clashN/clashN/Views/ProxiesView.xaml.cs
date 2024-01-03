using System.Reactive.Disposables;
using System.Windows.Input;
using ClashN.ViewModels;
using ReactiveUI;
using Splat;

namespace ClashN.Views
{
    /// <summary>
    /// Interaction logic for ProxiesView.xaml
    /// </summary>
    public partial class ProxiesView
    {
        public ProxiesView()
        {
            InitializeComponent();
            ViewModel = new ProxiesViewModel();
            Locator.CurrentMutable.RegisterLazySingleton(() => ViewModel, typeof(ProxiesViewModel));
            lstProxyDetails.PreviewMouseDoubleClick += lstProxyDetails_PreviewMouseDoubleClick;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.ProxyGroups, v => v.lstProxyGroups.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedGroup, v => v.lstProxyGroups.SelectedItem).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.ProxyDetails, v => v.lstProxyDetails.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedDetail, v => v.lstProxyDetails.SelectedItem).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.ProxiesReloadCmd, v => v.menuProxiesReload).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ProxiesDelaytestCmd, v => v.menuProxiesDelaytest).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.ProxiesDelaytestPartCmd, v => v.menuProxiesDelaytestPart).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.ProxiesSelectActivityCmd, v => v.menuProxiesSelectActivity).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SystemProxySelected, v => v.cmbSystemProxy.SelectedIndex).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.RuleModeSelected, v => v.cmbRulemode.SelectedIndex).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SortingSelected, v => v.cmbSorting.SelectedIndex).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AutoRefresh, v => v.togAutoRefresh.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableTun, v => v.togEnableTun.IsChecked).DisposeWith(disposables);
            });
        }

        private void ProxiesView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F5:
                    ViewModel?.ProxiesReload();
                    break;

                case Key.Enter:
                    ViewModel?.SetActiveProxy();
                    break;
            }
        }

        private void lstProxyDetails_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewModel?.SetActiveProxy();
        }
    }
}