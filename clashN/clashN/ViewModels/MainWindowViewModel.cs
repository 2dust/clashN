using ClashN.Handler;
using ClashN.Mode;
using ClashN.Views;
using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using NHotkey;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace ClashN.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private CoreHandler coreHandler;
        private static Config _config;
        private NoticeHandler? _noticeHandler;
        private StatisticsHandler? statistics;
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        #region Views

        //public DashboardView GetDashboardView { get; }
        public ProxiesView GetProxyView { get; }

        public ProfilesView GetProfilesView { get; }
        public LogsView GetLogsView { get; }
        public ConnectionsView GetConnectionsView { get; }
        public SettingsView GetSettingsView { get; }
        public HelpView GetHelpView { get; }
        public PromotionView GetPromotionView { get; }

        [Reactive]
        public string SpeedUpload { get; set; } = "0.00";

        [Reactive]
        public string SpeedDownload { get; set; } = "0.00";

        #endregion Views

        #region System Proxy

        [Reactive]
        public bool BlSystemProxyClear { get; set; }

        [Reactive]
        public bool BlSystemProxySet { get; set; }

        [Reactive]
        public bool BlSystemProxyNothing { get; set; }

        [Reactive]
        public bool BlSystemProxyPac { get; set; }

        public ReactiveCommand<Unit, Unit> SystemProxyClearCmd { get; }
        public ReactiveCommand<Unit, Unit> SystemProxySetCmd { get; }
        public ReactiveCommand<Unit, Unit> SystemProxyNothingCmd { get; }

        public ReactiveCommand<Unit, Unit> SystemProxyPacCmd { get; }

        #endregion System Proxy

        #region Rule mode

        [Reactive]
        public bool BlModeRule { get; set; }

        [Reactive]
        public bool BlModeGlobal { get; set; }

        [Reactive]
        public bool BlModeDirect { get; set; }

        [Reactive]
        public bool BlModeNothing { get; set; }

        public ReactiveCommand<Unit, Unit> ModeRuleCmd { get; }
        public ReactiveCommand<Unit, Unit> ModeGlobalCmd { get; }
        public ReactiveCommand<Unit, Unit> ModeDirectCmd { get; }
        public ReactiveCommand<Unit, Unit> ModeNothingCmd { get; }

        #endregion Rule mode

        #region Other

        public ReactiveCommand<Unit, Unit> AddProfileViaScanCmd { get; }
        public ReactiveCommand<Unit, Unit> SubUpdateCmd { get; }
        public ReactiveCommand<Unit, Unit> SubUpdateViaProxyCmd { get; }
        public ReactiveCommand<Unit, Unit> ExitCmd { get; }

        public ReactiveCommand<Unit, Unit> ReloadCmd { get; }
        public ReactiveCommand<Unit, Unit> NotifyLeftClickCmd { get; }

        [Reactive]
        public Icon NotifyIcon { get; set; }

        #endregion Other

        #region Init

        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            _config = LazyConfig.Instance.Config;

            Locator.CurrentMutable.RegisterLazySingleton(() => new NoticeHandler(snackbarMessageQueue), typeof(NoticeHandler));
            _noticeHandler = Locator.Current.GetService<NoticeHandler>();

            ThreadPool.RegisterWaitForSingleObject(App.ProgramStarted, OnProgramStarted, null, -1, false);

            Init();

            //Views
            //GetDashboardView = new();
            GetProxyView = new();
            GetProfilesView = new();
            GetLogsView = new();
            GetConnectionsView = new();
            GetSettingsView = new();
            GetHelpView = new();
            GetPromotionView = new();

            RestoreUI();
            if (_config.AutoHideStartup)
            {
                Observable.Range(1, 1)
                 .Delay(TimeSpan.FromSeconds(1))
                 .Subscribe(x =>
                 {
                     Application.Current.Dispatcher.Invoke((Action)(() =>
                     {
                         ShowHideWindow(false);
                     }));
                 });
            }

            //System proxy
            SystemProxyClearCmd = ReactiveCommand.Create(() =>
            {
                SetListenerType(SysProxyType.ForcedClear);
            });//, this.WhenAnyValue(x => x.BlSystemProxyClear, y => !y));
            SystemProxySetCmd = ReactiveCommand.Create(() =>
            {
                SetListenerType(SysProxyType.ForcedChange);
            });//, this.WhenAnyValue(x => x.BlSystemProxySet, y => !y));
            SystemProxyNothingCmd = ReactiveCommand.Create(() =>
            {
                SetListenerType(SysProxyType.Unchanged);
            });//, this.WhenAnyValue(x => x.BlSystemProxyNothing, y => !y));
            SystemProxyPacCmd = ReactiveCommand.Create(() =>
            {
                SetListenerType(SysProxyType.Pac);
            });//, this.WhenAnyValue(x => x.BlSystemProxyNothing, y => !y));

            //Rule mode
            ModeRuleCmd = ReactiveCommand.Create(() =>
            {
                SetRuleModeCheck(ERuleMode.Rule);
            });//, this.WhenAnyValue(x => x.BlModeRule, y => !y));
            ModeGlobalCmd = ReactiveCommand.Create(() =>
            {
                SetRuleModeCheck(ERuleMode.Global);
            });//, this.WhenAnyValue(x => x.BlModeGlobal, y => !y));
            ModeDirectCmd = ReactiveCommand.Create(() =>
            {
                SetRuleModeCheck(ERuleMode.Direct);
            });//, this.WhenAnyValue(x => x.BlModeDirect, y => !y));
            ModeNothingCmd = ReactiveCommand.Create(() =>
            {
                SetRuleModeCheck(ERuleMode.Unchanged);
            });//, this.WhenAnyValue(x => x.BlModeNothing, y => !y));

            //Other
            AddProfileViaScanCmd = ReactiveCommand.CreateFromTask(() =>
            {
                return Locator.Current?.GetService<ProfilesViewModel>()?.ScanScreenTaskAsync();
            });
            SubUpdateCmd = ReactiveCommand.Create(() =>
            {
                Locator.Current.GetService<ProfilesViewModel>()?.UpdateSubscriptionProcess(false, false);
            });
            SubUpdateViaProxyCmd = ReactiveCommand.Create(() =>
            {
                Locator.Current.GetService<ProfilesViewModel>()?.UpdateSubscriptionProcess(true, false);
            });
            //ExitCmd = ReactiveCommand.Create(() =>
            //{
            //    MyAppExit(false);
            //});
            ReloadCmd = ReactiveCommand.Create(() =>
            {
                Global.reloadCore = true;
                _ = LoadCore();
            });
            NotifyLeftClickCmd = ReactiveCommand.Create(() =>
            {
                ShowHideWindow(null);
            });

            Global.ShowInTaskbar = true;//Application.Current.MainWindow.ShowInTaskbar;
        }

        private void OnProgramStarted(object state, bool timeout)
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                string? clipboardData = Utils.GetClipboardData();
                if (state != null && clipboardData != null)
                {
                    if (string.IsNullOrEmpty(clipboardData) || !clipboardData.StartsWith(Global.clashProtocol))
                    {
                        return;
                    }
                }

                ShowHideWindow(true);

                Locator.Current.GetService<ProfilesViewModel>()?.AddProfilesViaClipboard(true);
            }));
        }

        public void MyAppExit(bool blWindowsShutDown)
        {
            try
            {
                coreHandler.CoreStop();

                //HttpProxyHandle.CloseHttpAgent(config);
                if (blWindowsShutDown)
                {
                    SysProxyHandle.ResetIEProxy4WindowsShutDown();
                }
                else
                {
                    SysProxyHandle.UpdateSysProxy(_config, true);
                }

                StorageUI();
                ConfigProc.SaveConfig(_config);
                //statistics?.SaveToFile();
                statistics?.Close();
            }
            catch { }
            finally
            {
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
        }

        private void OnHotkeyHandler(object sender, HotkeyEventArgs e)
        {
            switch (Utils.ToInt(e.Name))
            {
                case (int)GlobalHotkeyAction.ShowForm:
                    ShowHideWindow(null);
                    break;

                case (int)GlobalHotkeyAction.SystemProxyClear:
                    SetListenerType(SysProxyType.ForcedClear);
                    break;

                case (int)GlobalHotkeyAction.SystemProxySet:
                    SetListenerType(SysProxyType.ForcedChange);
                    break;

                case (int)GlobalHotkeyAction.SystemProxyUnchanged:
                    SetListenerType(SysProxyType.Unchanged);
                    break;

                case (int)GlobalHotkeyAction.SystemProxyPac:
                    SetListenerType(SysProxyType.Pac);
                    break;
            }
            e.Handled = true;
        }

        private void Init()
        {
            MainFormHandler.Instance.BackupGuiNConfig(_config, true);
            MainFormHandler.Instance.InitRegister(_config);

            coreHandler = new CoreHandler(UpdateHandler);

            if (_config.EnableStatistics)
            {
                statistics = new StatisticsHandler(_config, UpdateStatisticsHandler);
            }

            MainFormHandler.Instance.UpdateTask(_config, UpdateTaskHandler);
            MainFormHandler.Instance.RegisterGlobalHotkey(_config, OnHotkeyHandler, UpdateTaskHandler);

            OnProgramStarted("shown", true);

            _ = LoadCore();
        }

        private void UpdateHandler(bool notify, string msg)
        {
            if (notify)
            {
                _noticeHandler?.Enqueue(msg);
                _noticeHandler?.SendMessage(msg);
            }
            else
            {
                _noticeHandler?.SendMessage(msg);
            }
        }

        private async void UpdateTaskHandler(bool success, string msg)
        {
            _noticeHandler?.SendMessage(msg);
            if (success)
            {
                Global.reloadCore = true;
                await LoadCore();
            }
        }

        private void UpdateStatisticsHandler(ulong up, ulong down)
        {
            try
            {
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    if (!Global.ShowInTaskbar)
                    {
                        return;
                    }

                    SpeedUpload = @$"{Utils.HumanFy(up)}/s";
                    SpeedDownload = @$"{Utils.HumanFy(down)}/s";
                }));
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
        }

        #endregion Init

        #region Core

        public async Task LoadCore()
        {
            Locator.Current.GetService<ProxiesViewModel>()?.ProxiesClear();

            //if (Global.reloadCore)
            //{
            //    mainMsgControl.ClearMsg();
            //}
            await Task.Run(() =>
            {
                coreHandler.LoadCore(_config);
            });

            Global.reloadCore = false;
            ConfigProc.SaveConfig(_config, false);
            //statistics?.SaveToFile();

            ChangePACButtonStatus(_config.SysProxyType);
            SetRuleMode(_config.ruleMode);

            Locator.Current.GetService<ProxiesViewModel>()?.ProxiesReload();
            Locator.Current.GetService<ProxiesViewModel>()?.ProxiesDelayTest();
            Locator.Current.GetService<ProfilesViewModel>()?.RefreshProfiles();
        }

        public void CloseCore()
        {
            ConfigProc.SaveConfig(_config, false);
            //statistics?.SaveToFile();

            ChangePACButtonStatus(SysProxyType.ForcedClear);

            coreHandler.CoreStop();
        }

        #endregion Core

        #region System proxy and Rule mode

        public void SetListenerType(SysProxyType type)
        {
            if (_config.SysProxyType == type)
            {
                return;
            }
            _config.SysProxyType = type;
            ChangePACButtonStatus(type);

            Locator.Current.GetService<ProxiesViewModel>()?.ReloadSystemProxySelected();
        }

        private void ChangePACButtonStatus(SysProxyType type)
        {
            SysProxyHandle.UpdateSysProxy(_config, false);

            BlSystemProxyClear = (type == SysProxyType.ForcedClear);
            BlSystemProxySet = (type == SysProxyType.ForcedChange);
            BlSystemProxyNothing = (type == SysProxyType.Unchanged);
            BlSystemProxyPac = (type == SysProxyType.Pac);

            _noticeHandler?.SendMessage($"Change system proxy", true);

            ConfigProc.SaveConfig(_config, false);

            //mainMsgControl.DisplayToolStatus(config);

            NotifyIcon = MainFormHandler.Instance.GetNotifyIcon(_config);
        }

        public void SetRuleModeCheck(ERuleMode mode)
        {
            if (_config.ruleMode == mode)
            {
                return;
            }
            SetRuleMode(mode);

            Locator.Current.GetService<ProxiesViewModel>()?.ReloadRulemodeSelected();
        }

        private void SetRuleMode(ERuleMode mode)
        {
            BlModeRule = (mode == ERuleMode.Rule);
            BlModeGlobal = (mode == ERuleMode.Global);
            BlModeDirect = (mode == ERuleMode.Direct);
            BlModeNothing = (mode == ERuleMode.Unchanged);

            //mainMsgControl.SetToolSslInfo("routing", mode.ToString());

            _noticeHandler?.SendMessage($"Set rule mode {_config.ruleMode.ToString()}->{mode.ToString()}", true);
            _config.ruleMode = mode;
            ConfigProc.SaveConfig(_config, false);

            if (mode != ERuleMode.Unchanged)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("mode", _config.ruleMode.ToString().ToLower());
                MainFormHandler.Instance.ClashConfigUpdate(headers);
            }
        }

        #endregion System proxy and Rule mode

        #region UI

        public void ShowHideWindow(bool? blShow)
        {
            var bl = blShow.HasValue ? blShow.Value : !Global.ShowInTaskbar;
            if (bl)
            {
                //Application.Current.MainWindow.ShowInTaskbar = true;
                Application.Current.MainWindow.Show();
                if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
                Application.Current.MainWindow.Activate();
                Application.Current.MainWindow.Focus();
            }
            else
            {
                Application.Current.MainWindow.Hide();
                //Application.Current.MainWindow.ShowInTaskbar = false;
            };
            Global.ShowInTaskbar = bl;
        }

        private void RestoreUI()
        {
            ModifyTheme(_config.UiItem.colorModeDark);

            if (!string.IsNullOrEmpty(_config.UiItem.colorPrimaryName))
            {
                var swatch = new SwatchesProvider().Swatches.FirstOrDefault(t => t.Name == _config.UiItem.colorPrimaryName);
                if (swatch != null
                   && swatch.ExemplarHue != null
                   && swatch.ExemplarHue?.Color != null)
                {
                    ChangePrimaryColor(swatch.ExemplarHue.Color);
                }
            }

            //if (!config.uiItem.mainLocation.IsEmpty)
            //{
            //    this.Location = config.uiItem.mainLocation;
            //}

            if (_config.UiItem.mainWidth > 0 && _config.UiItem.mainHeight > 0)
            {
                Application.Current.MainWindow.Width = _config.UiItem.mainWidth;
                Application.Current.MainWindow.Height = _config.UiItem.mainHeight;
            }

            IntPtr hWnd = new WindowInteropHelper(Application.Current.MainWindow).EnsureHandle();
            Graphics g = Graphics.FromHwnd(hWnd);
            if (Application.Current.MainWindow.Width > SystemInformation.WorkingArea.Width * 96 / g.DpiX)
            {
                Application.Current.MainWindow.Width = SystemInformation.WorkingArea.Width * 96 / g.DpiX;
            }
            if (Application.Current.MainWindow.Height > SystemInformation.WorkingArea.Height * 96 / g.DpiY)
            {
                Application.Current.MainWindow.Height = SystemInformation.WorkingArea.Height * 96 / g.DpiY;
            }
        }

        private void StorageUI()
        {
            _config.UiItem.mainWidth = Application.Current.MainWindow.Width;
            _config.UiItem.mainHeight = Application.Current.MainWindow.Height;
        }

        public void ModifyTheme(bool isDarkTheme)
        {
            var theme = _paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            _paletteHelper.SetTheme(theme);

            Utils.SetDarkBorder(Application.Current.MainWindow, isDarkTheme);
        }

        public void ChangePrimaryColor(System.Windows.Media.Color color)
        {
            //  var  Swatches = new SwatchesProvider().Swatches;

            var theme = _paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            _paletteHelper.SetTheme(theme);
        }

        #endregion UI
    }
}