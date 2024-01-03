using System.Reactive.Disposables;
using ClashN.ViewModels;
using ReactiveUI;

namespace ClashN.Views
{
    /// <summary>
    /// Interaction logic for HelpView.xaml
    /// </summary>
    public partial class HelpView
    {
        public HelpView()
        {
            InitializeComponent();
            ViewModel = new HelpViewModel();

            this.WhenActivated(disposables =>
            {
                this.BindCommand(ViewModel, vm => vm.CheckUpdateCmd, v => v.btnCheckUpdateN).DisposeWith(disposables);
                //this.BindCommand(ViewModel, vm => vm.CheckUpdateClashCoreCmd, v => v.btnCheckUpdateClashCore).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.CheckUpdateMihomoCoreCmd, v => v.btnCheckUpdateMihomoCore).DisposeWith(disposables);
                //this.BindCommand(ViewModel, vm => vm.CheckUpdateGeoDataCmd, v => v.btnCheckUpdateGeo).DisposeWith(disposables);
            });
        }

        private void btnAbout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utils.ProcessStart(Global.AboutUrl);
        }
    }
}