using ClashN.Handler;
using ClashN.Mode;
using ClashN.Resx;
using ClashN.Views;
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

namespace ClashN.ViewModels
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
            _config = LazyConfig.Instance.Config;

            SelectedSource = new();

            RefreshProfiles();

            var canEditRemove = this.WhenAnyValue(
               x => x.SelectedSource,
               selectedSource => selectedSource != null && !string.IsNullOrEmpty(selectedSource.indexId));

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
                ConfigProc.ClearAllServerStatistics(ref _config);
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

        public void EditProfile(bool blNew)
        {
            ProfileItem item;
            if (blNew)
            {
                item = new()
                {
                    coreType = CoreKind.ClashMeta
                };
            }
            else
            {
                item = _config.GetProfileItem(SelectedSource.indexId);
                if (item is null)
                {
                    return;
                }
            }

            PorfileEditWindow dialog = new PorfileEditWindow(item)
            {
                Owner = App.Current.MainWindow,
            };

            if (dialog.ShowDialog() == true)
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

            if (string.IsNullOrEmpty(result))
            {
                _noticeHandler?.Enqueue(ResUI.NoValidQRcodeFound);
            }
            else
            {
                int ret = ConfigProc.AddBatchProfiles(ref _config, result, "", "");
                if (ret == 0)
                {
                    RefreshProfiles();
                    _noticeHandler?.Enqueue(ResUI.SuccessfullyImportedProfileViaScan);
                }
            }
        }

        public void AddProfilesViaClipboard(bool bClear)
        {
            string? clipboardData = Utils.GetClipboardData();
            if (string.IsNullOrEmpty(clipboardData))
            {
                return;
            }
            int ret = ConfigProc.AddBatchProfiles(ref _config, clipboardData, "", "");
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
            var content = ConfigProc.GetProfileContent(item);
            if (string.IsNullOrEmpty(content))
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

            ConfigProc.RemoveProfile(_config, new List<ProfileItem>() { item });

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
            if (ConfigProc.CopyProfile(ref _config, new List<ProfileItem>() { item }) == 0)
            {
                _noticeHandler?.Enqueue(ResUI.OperationSuccess);
                RefreshProfiles();
            }
        }

        public void SetDefaultProfile()
        {
            if (string.IsNullOrEmpty(SelectedSource?.indexId))
            {
                return;
            }
            if (SelectedSource?.indexId == _config.IndexId)
            {
                return;
            }
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                _noticeHandler?.Enqueue(ResUI.PleaseSelectProfile);
                return;
            }

            if (ConfigProc.SetDefaultProfile(ref _config, item) == 0)
            {
                _noticeHandler?.SendMessage(ResUI.OperationSuccess);
                RefreshProfiles();

                Locator.Current.GetService<MainWindowViewModel>()?.LoadCore();
            }
        }

        public void RefreshProfiles()
        {
            ConfigProc.SetDefaultProfile(_config, _config.ProfileItems);

            var lstModel = new List<ProfileItemModel>();
            foreach (var item in _config.ProfileItems.OrderBy(it => it.sort))
            {
                var model = Utils.FromJson<ProfileItemModel>(Utils.ToJson(item));
                model.IsActive = _config.IsActiveNode(item);
                lstModel.Add(model);
            }

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                _profileItems.Clear();
                _profileItems.AddRange(lstModel);
            }));
        }

        public void MoveProfile(int startIndex, ProfileItemModel targetItem)
        {
            var targetIndex = _profileItems.IndexOf(targetItem);
            if (startIndex >= 0 && targetIndex >= 0 && startIndex != targetIndex)
            {
                if (ConfigProc.MoveProfile(ref _config, startIndex, MovementTarget.Position, targetIndex) == 0)
                {
                    RefreshProfiles();
                }
            }
        }

        public async void ProfileQrcode()
        {
            var item = _config.GetProfileItem(SelectedSource.indexId);
            if (item is null)
            {
                return;
            }
            if (string.IsNullOrEmpty(item.url))
            {
                return;
            }
            var img = QRCodeHelper.GetQRCode(item.url);
            var dialog = new ProfileQrcodeView()
            {
                imgQrcode = { Source = img },
                txtContent = { Text = item.url },
            };

            await DialogHost.Show(dialog, "RootDialog");
        }
    }
}