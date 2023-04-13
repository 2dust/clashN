using ClashN.Mode;
using ClashN.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace ClashN.Views
{
    /// <summary>
    /// PorfileEditWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PorfileEditWindow
    {
        public PorfileEditWindow(ProfileItem profileItem)
        {
            InitializeComponent();
            ViewModel = new ProfileEditViewModel(profileItem, this);
            Global.coreTypes.ForEach(it =>
            {
                cmbCoreType.Items.Add(it);
            });
            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.SelectedSource.remarks, v => v.txtRemarks.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.url, v => v.txtUrl.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.address, v => v.txtAddress.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.userAgent, v => v.txtUserAgent.Text).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.CoreType, v => v.cmbCoreType.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.enabled, v => v.togEnabled.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSource.enableConvert, v => v.togEnableConvert.IsChecked).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.BrowseProfileCmd, v => v.btnBrowse).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.EditProfileCmd, v => v.btnEdit).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SaveProfileCmd, v => v.btnSave).DisposeWith(disposables);
            });
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void PorfileEditWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}