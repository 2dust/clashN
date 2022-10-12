using clashN.Base;
using clashN.Handler;
using clashN.Mode;
using clashN.Resx;
using clashN.Views;
using DynamicData;
using DynamicData.Binding;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.IO;
using System.Reactive;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace clashN.ViewModels
{
    public class ProfilesViewModel : ReactiveObject
    {
        private static Config _config;
        private NoticeHandler? _noticeHandler;

        private IObservableCollection<ProfileItemModel> _profileItems = new ObservableCollectionExtended<ProfileItemModel>();

        public IObservableCollection<ProfileItemModel> ProfileItems => _profileItems;

        public ReactiveCommand<Unit, Unit> EditLocalFileCmd { get; }
        public ReactiveCommand<Unit, Unit> EditProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> AddProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> AddProfileViaScanCmd { get; }
        public ReactiveCommand<Unit, Unit> AddProfileViaClipboardCmd { get; }
        public ReactiveCommand<Unit, Unit> ExportProfileCmd { get; }

        public ReactiveCommand<Unit, Unit> SubUpdateCmd { get; }
        public ReactiveCommand<Unit, Unit> SubUpdateSelectedCmd { get; }
        public ReactiveCommand<Unit, Unit> SubUpdateViaProxyCmd { get; }
        public ReactiveCommand<Unit, Unit> SubUpdateSelectedViaProxyCmd { get; }

        public ReactiveCommand<Unit, Unit> RemoveProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> CloneProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> SetDefaultProfileCmd { get; }
        public ReactiveCommand<Unit, Unit> ClearStatisticCmd { get; }
        public ReactiveCommand<Unit, Unit> ProfileReloadCmd { get; }
        public ReactiveCommand<Unit, Unit> ProfileQrcodeCmd { get; }


        [Reactive]
        public ProfileItemModel SelectedSource { get; set; }

        public ProfilesViewModel()
        {
            _noticeHandler = Locator.Current.GetService<NoticeHandler>();
            _config = LazyConfig.Instance.GetConfig();

            SelectedSource = new();

            RefreshProfiles();

            var canEditRemove = this.WhenAnyValue(
               x => x.SelectedSource,
               selectedSource => selectedSource != null && !selectedSource.indexId.IsNullOrEmpty());

            //Profile
            EditLocalFileCmd = ReactiveCommand.Create(() =>
            {
                EditLocalFile();
            }, canEditRemove);

            EditProfileCmd = ReactiveCommand.Create(() =>
            {
                EditProfile(false);
            }, canEditRemove);

            AddProfileCmd = ReactiveCommand.Create(() =>
            {
                EditProfile(true);
            });
            AddProfileViaScanCmd = ReactiveCommand.CreateFromTask(() =>
            {
                return ScanScreenTaskAsync();
            });
            AddProfileViaClipboardCmd = ReactiveCommand.Create(() =>
            {
                AddProfilesViaClipboard(false);
            });

            ExportProfileCmd = ReactiveCommand.Create(() =>
            {
                ExportProfile2Clipboard();
            }, canEditRemove);

            //Subscription
            SubUpdateCmd = ReactiveCommand.Create(() =>
            {
                UpdateSubscriptionProcess(false, false);

            });
            SubUpdateSelectedCmd = ReactiveCommand.Create(() =>
            {
                UpdateSubscriptionProcess(false, true);

            }, canEditRemove);
            SubUpdateViaProxyCmd = ReactiveCommand.Create(() =>
            {
                UpdateSubscriptionProcess(true, false);

            });
            SubUpdateSelectedViaProxyCmd = ReactiveCommand.Create(() =>
            {
                UpdateSubscriptionProcess(true, true);

            }, canEditRemove);

            //Profile other
            RemoveProfileCmd = ReactiveCommand.Create(() =>
            {
                RemoveProfile();
            }, canEditRemove);
            CloneProfileCmd = ReactiveCommand.Create(() =>
            {
                CloneProfile();
            }, canEditRemove);
            SetDefaultProfileCmd = ReactiveCommand.Create(() =>
            {
                SetDefaultProfile();
            }, canEditRemove);

            ClearStatisticCmd = ReactiveCommand.Create(() =>
            {
                ConfigHandler.ClearAllServerStatistics(ref _config);
                RefreshProfiles();
            });
            ProfileReloadCmd = ReactiveCommand.Create(() =>
            {
                RefreshProfiles();
            });
            ProfileQrcodeCmd = ReactiveCommand.Create(() =>
            {
                ProfileQrcode();
            }, canEditRemove);
        }

        private void EditLocalFile()
        {
            var address = SelectedSource.address;
            if (Utils.IsNullOrEmpty(address))
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

        private void EditProfile(bool blNew)
        {
            ProfileItem item;
            if (blNew)
            {
                item = new();
            }
            else
            {
                item = _config.GetProfileItem(SelectedSource.indexId);
                if (item is null)
                {
                    return;
                }
            }
            if ((new PorfileEditWindow(item)).ShowDialog() == true)
            {
                RefreshProfiles();
            }
        }

        public async Task ScanScreenTaskAsync()
        {
            Locator.Current.GetService<MainWindowViewModel>()?.ShowHideWindow(false);

            string result = await Task.Run(() =>
            {
                return Utils.ScanScreen();
            });

            Locator.Current.GetService<MainWindowViewModel>()?.ShowHideWindow(true);

            if (Utils.IsNullOrEmpty(result))
            {
                _noticeHandler?.Enqueue(ResUI.NoValidQRcodeFound);
            }
            else
            {
                int ret = ConfigHandler.AddBatchProfiles(ref _config, result, "", "");
                if (ret == 0)
                {
                    RefreshProfiles();
                    _noticeHandler?.Enqueue(ResUI.SuccessfullyImportedProfileViaScan);
                }
            }
        }
        public void AddProfilesViaClipboard(bool bClear)
        {
            string clipboardData = Utils.GetClipboardData();
            if (Utils.IsNullOrEmpty(clipboardData))
            {
                return;
            }
            int ret = ConfigHandler.AddBatchProfiles(ref _config, clipboardData, "", "");
            if (ret == 0)
            {
                if (bClear)
                {
                    Utils.SetClipboardData(String.Empty);
                }
                RefreshProfiles();
                _noticeHandler?.Enqueue(ResUI.SuccessfullyImportedProfileViaClipboard);
            }
        }

        public void ExportProfile2Clipboard()
        {
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                return;
            }
            var content = ConfigHandler.GetProfileContent(item);
            if (Utils.IsNullOrEmpty(content))
            {
                content = item.url;
            }
            Utils.SetClipboardData(content);

            _noticeHandler?.Enqueue(ResUI.BatchExportSuccessfully);
        }

        public void UpdateSubscriptionProcess(bool blProxy, bool blSelected)
        {
            List<ProfileItem> profileItems = null;
            if (blSelected)
            {
                var item = _config.GetProfileItem(SelectedSource.indexId);
                profileItems = new List<ProfileItem>() { item };
            }

            void _updateUI(bool success, string msg)
            {
                _noticeHandler?.SendMessage(msg);

                if (success)
                {
                    RefreshProfiles();
                }
            };

            (new UpdateHandle()).UpdateSubscriptionProcess(_config, blProxy, profileItems, _updateUI);
        }

        public void RemoveProfile()
        {
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                return;
            }
            if (UI.ShowYesNo(ResUI.RemoveProfile) == DialogResult.No)
            {
                return;
            }

            ConfigHandler.RemoveProfile(_config, new List<ProfileItem>() { item });

            _noticeHandler?.Enqueue(ResUI.OperationSuccess);

            RefreshProfiles();

            Locator.Current.GetService<MainWindowViewModel>()?.LoadCore();
        }
        private void CloneProfile()
        {
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                return;
            }
            if (ConfigHandler.CopyProfile(ref _config, new List<ProfileItem>() { item }) == 0)
            {
                _noticeHandler?.Enqueue(ResUI.OperationSuccess);
                RefreshProfiles();
            }
        }
        public void SetDefaultProfile()
        {
            if (Utils.IsNullOrEmpty(SelectedSource?.indexId))
            {
                return;
            }
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                _noticeHandler?.Enqueue(ResUI.PleaseSelectProfile);
                return;
            }

            if (ConfigHandler.SetDefaultProfile(ref _config, item) == 0)
            {
                _noticeHandler?.Enqueue(ResUI.OperationSuccess);
                RefreshProfiles();

                Locator.Current.GetService<MainWindowViewModel>()?.LoadCore();
            }
        }

        public void RefreshProfiles()
        {
            ConfigHandler.SetDefaultProfile(_config, _config.profileItems);

            var lstModel = new List<ProfileItemModel>();
            foreach (var item in _config.profileItems)
            {
                var model = Utils.FromJson<ProfileItemModel>(Utils.ToJson(item));
                model.isActive = _config.IsActiveNode(item);
                lstModel.Add(model);
            }

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                _profileItems.Clear();
                _profileItems.AddRange(lstModel);
            }));

        }

        public async void ProfileQrcode()
        {
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                return;
            }
            if (Utils.IsNullOrEmpty(item.url))
            {
                return;
            }
            var img = QRCodeHelper.GetQRCode(item.url);
            var dialog = new ProfileQrcodeView()
            {
                imgQrcode = { Source = img }
            };

            await DialogHost.Show(dialog, "RootDialog");
        }

    }
}
