using ClashN.Handler;
using ClashN.Mode;
using ClashN.Resx;
using ClashN.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;
using Splat;
using System.IO;
using System.Reactive;
using System.Windows.Forms;

namespace ClashN.ViewModels
{
    public class ProfileEditViewModel : ReactiveValidationObject
    {
        private static Config _config;
        private NoticeHandler? _noticeHandler;
        private PorfileEditWindow _view;

        [Reactive]
        public ProfileItem SelectedSource { get; set; }

        [Reactive]
        public string CoreType { get; set; }

        public ReactiveCommand<Unit, Unit> BrowseProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> EditProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> SaveProfileCmd { get; }

        public ProfileEditViewModel(ProfileItem profileItem, PorfileEditWindow view)
        {
            _noticeHandler = Locator.Current.GetService<NoticeHandler>();
            _config = LazyConfig.Instance.Config;

            if (string.IsNullOrEmpty(profileItem.indexId))
            {
                SelectedSource = profileItem;
            }
            else
            {
                SelectedSource = Utils.DeepCopy(profileItem);
            }

            _view = view;
            CoreType = (SelectedSource.coreType ?? CoreKind.Clash).ToString();

            BrowseProfileCmd = ReactiveCommand.Create(() =>
            {
                BrowseProfile();
            });

            EditProfileCmd = ReactiveCommand.Create(() =>
            {
                EditProfile();
            });

            SaveProfileCmd = ReactiveCommand.Create(() =>
            {
                SaveProfile();
            });

            Utils.SetDarkBorder(view, _config.UiItem.colorModeDark);
        }

        private void SaveProfile()
        {
            string remarks = SelectedSource.remarks;
            if (string.IsNullOrEmpty(remarks))
            {
                _noticeHandler?.Enqueue(ResUI.PleaseFillRemarks);
                return;
            }

            if (string.IsNullOrEmpty(CoreType))
            {
                SelectedSource.coreType = null;
            }
            else
            {
                SelectedSource.coreType = (CoreKind)Enum.Parse(typeof(CoreKind), CoreType);
            }

            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                item = SelectedSource;
            }
            else
            {
                item.remarks = SelectedSource.remarks;
                item.url = SelectedSource.url;
                item.address = SelectedSource.address;
                item.userAgent = SelectedSource.userAgent;
                item.coreType = SelectedSource.coreType;
                item.enabled = SelectedSource.enabled;
                item.enableConvert = SelectedSource.enableConvert;
            }

            if (ConfigProc.EditProfile(ref _config, item) == 0)
            {
                Locator.Current.GetService<ProfilesViewModel>()?.RefreshProfiles();
                _noticeHandler?.Enqueue(ResUI.OperationSuccess);
                _view?.Close();
            }
            else
            {
                _noticeHandler?.Enqueue(ResUI.OperationFailed);
            }
        }

        private void BrowseProfile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "YAML|*.yaml;*.yml|All|*.*"
            };

            IWin32Window parent = App.Current.MainWindow.WpfWindow2WinFormWin32Window();
            if (fileDialog.ShowDialog(parent) != DialogResult.OK)
            {
                return;
            }
            if (UI.ShowYesNo(ResUI.MsgSureContinue) == DialogResult.No)
            {
                return;
            }
            string fileName = fileDialog.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                item = SelectedSource;
            }
            if (ConfigProc.AddProfileViaPath(ref _config, item, fileName) == 0)
            {
                _noticeHandler?.Enqueue(ResUI.SuccessfullyImportedCustomProfile);
                Locator.Current.GetService<ProfilesViewModel>()?.RefreshProfiles();
                _view?.Close();
            }
            else
            {
                _noticeHandler?.Enqueue(ResUI.FailedImportedCustomProfile);
            }
        }

        private void EditProfile()
        {
            var address = SelectedSource.address;
            if (string.IsNullOrEmpty(address))
            {
                _noticeHandler?.Enqueue(ResUI.FillProfileAddressCustom);
                return;
            }

            address = Path.Combine(Utils.GetConfigPath(), address);
            if (File.Exists(address))
            {
                Utils.ProcessStart(address);
            }
            else
            {
                _noticeHandler?.Enqueue(ResUI.FailedReadConfiguration);
            }
        }
    }
}