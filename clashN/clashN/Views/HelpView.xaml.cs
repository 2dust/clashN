using ClashN.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;

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
                this.BindCommand(ViewModel, vm => vm.CheckUpdateClashCoreCmd, v => v.btnCheckUpdateClashCore).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.CheckUpdateClashMetaCoreCmd, v => v.btnCheckUpdateClashMetaCore).DisposeWith(disposables);
            });
        }

        private void btnAbout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Utils.ProcessStart(Global.AboutUrl);
        }
    }
}