using ClashN.Handler;
using ClashN.Mode;
using ClashN.Resx;
using ClashN.Views;
using DynamicData;
using DynamicData.Binding;
using MaterialDesignColors;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;
using Splat;
using System.IO;
using System.Reactive;
using System.Windows;

namespace ClashN.ViewModels
{
    public class SettingsViewModel : ReactiveValidationObject
    {
        private static Config _config;

        #region Core

        [Reactive]
        public int MixedPort { get; set; }

        [Reactive]
        public int SocksPort { get; set; }

        [Reactive]
        public int HttpPort { get; set; }

        [Reactive]
        public int APIPort { get; set; }

        [Reactive]
        public bool AllowLANConn { get; set; }

        [Reactive]
        public bool EnableIpv6 { get; set; }

        [Reactive]
        public string LogLevel { get; set; }

        [Reactive]
        public bool EnableMixinContent { get; set; }

        public ReactiveCommand<Unit, Unit> EditMixinContentCmd { get; }

        #endregion Core

        #region ClashN

        [Reactive]
        public bool AutoRun { get; set; }

        [Reactive]
        public bool EnableStatistics { get; set; }

        [Reactive]
        public bool EnableSecurityProtocolTls13 { get; set; }

        [Reactive]
        public int autoUpdateSubInterval { get; set; }

        [Reactive]
        public int autoDelayTestInterval { get; set; }

        [Reactive]
        public string SubConvertUrl { get; set; }

        [Reactive]
        public string currentFontFamily { get; set; }

        [Reactive]
        public bool AutoHideStartup { get; set; }

        public ReactiveCommand<Unit, Unit> SetLoopbackCmd { get; }
        public ReactiveCommand<Unit, Unit> SetGlobalHotkeyCmd { get; }

        #endregion ClashN

        #region System proxy

        [Reactive]
        public string systemProxyExceptions { get; set; }

        [Reactive]
        public string systemProxyAdvancedProtocol { get; set; }

        [Reactive]
        public int PacPort { get; set; }

        #endregion System proxy

        #region UI

        private IObservableCollection<Swatch> _swatches = new ObservableCollectionExtended<Swatch>();
        public IObservableCollection<Swatch> Swatches => _swatches;

        [Reactive]
        public Swatch SelectedSwatch { get; set; }

        [Reactive]
        public bool ColorModeDark { get; set; }

        [Reactive]
        public string CurrentLanguage { get; set; }

        [Reactive]
        public int CurrentFontSize { get; set; }

        #endregion UI

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }

        public SettingsViewModel()
        {
            _config = LazyConfig.Instance.Config;

            //Core
            MixedPort = _config.MixedPort;
            SocksPort = _config.SocksPort;
            HttpPort = _config.HttpPort;
            APIPort = _config.ApiPort;
            AllowLANConn = _config.AllowLANConn;
            EnableIpv6 = _config.EnableIpv6;
            LogLevel = _config.LogLevel;
            EnableMixinContent = _config.EnableMixinContent;
            EditMixinContentCmd = ReactiveCommand.Create(() =>
            {
                EditMixinContent();
            }, this.IsValid());

            //ClashN
            AutoRun = _config.AutoRun;
            EnableStatistics = _config.EnableStatistics;
            EnableSecurityProtocolTls13 = _config.EnableSecurityProtocolTls13;
            autoUpdateSubInterval = _config.AutoUpdateSubInterval;
            autoDelayTestInterval = _config.AutoDelayTestInterval;
            SubConvertUrl = _config.ConstItem.subConvertUrl;
            currentFontFamily = _config.UiItem.currentFontFamily;
            AutoHideStartup = _config.AutoHideStartup;

            SetLoopbackCmd = ReactiveCommand.Create(() =>
            {
                Utils.ProcessStart(Utils.GetBinPath("EnableLoopback.exe"));
            }, this.IsValid());
            SetGlobalHotkeyCmd = ReactiveCommand.Create(() =>
            {
                GlobalHotkeySettingWindow dialog = new GlobalHotkeySettingWindow()
                {
                    Owner = App.Current.MainWindow
                };

                dialog.ShowDialog();
            }, this.IsValid());

            //System proxy
            systemProxyExceptions = _config.SystemProxyExceptions;
            systemProxyAdvancedProtocol = _config.SystemProxyAdvancedProtocol;
            PacPort = _config.PacPort;

            //UI
            ColorModeDark = _config.UiItem.colorModeDark;
            _swatches.AddRange(new SwatchesProvider().Swatches);
            if (!string.IsNullOrEmpty(_config.UiItem.colorPrimaryName))
            {
                SelectedSwatch = _swatches.FirstOrDefault(t => t.Name == _config.UiItem.colorPrimaryName);
            }
            CurrentLanguage = Utils.RegReadValue(Global.MyRegPath, Global.MyRegKeyLanguage, Global.Languages[0]);
            CurrentFontSize = _config.UiItem.currentFontSize;

            this.WhenAnyValue(
            x => x.ColorModeDark,
            y => y == true)
                .Subscribe(c =>
                {
                    if (_config.UiItem.colorModeDark != ColorModeDark)
                    {
                        _config.UiItem.colorModeDark = ColorModeDark;
                        Locator.Current.GetService<MainWindowViewModel>()?.ModifyTheme(ColorModeDark);
                        ConfigProc.SaveConfig(_config);
                    }
                });

            this.WhenAnyValue(
              x => x.SelectedSwatch,
              y => y != null && !string.IsNullOrEmpty(y.Name))
                 .Subscribe(c =>
                 {
                     if (SelectedSwatch == null
                     || string.IsNullOrEmpty(SelectedSwatch.Name)
                     || SelectedSwatch.ExemplarHue == null
                     || SelectedSwatch.ExemplarHue?.Color == null)
                     {
                         return;
                     }
                     if (_config.UiItem.colorPrimaryName != SelectedSwatch?.Name)
                     {
                         _config.UiItem.colorPrimaryName = SelectedSwatch?.Name;
                         Locator.Current.GetService<MainWindowViewModel>()?.ChangePrimaryColor(SelectedSwatch.ExemplarHue.Color);
                         ConfigProc.SaveConfig(_config);
                     }
                 });

            this.WhenAnyValue(
             x => x.CurrentLanguage,
             y => y != null && !string.IsNullOrEmpty(y))
                .Subscribe(c =>
                {
                    if (!string.IsNullOrEmpty(CurrentLanguage))
                    {
                        Utils.RegWriteValue(Global.MyRegPath, Global.MyRegKeyLanguage, CurrentLanguage);
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CurrentLanguage);
                    }
                });

            this.WhenAnyValue(
             x => x.CurrentFontSize,
             y => y > 0)
             .Subscribe(c =>
             {
                 if (_config.UiItem.colorModeDark != ColorModeDark)
                 {
                     _config.UiItem.colorModeDark = ColorModeDark;
                     Locator.Current.GetService<MainWindowViewModel>()?.ModifyTheme(ColorModeDark);
                     ConfigProc.SaveConfig(_config);
                 }
             });

            this.WhenAnyValue(
             x => x.CurrentFontSize,
             y => y > 0)
                .Subscribe(c =>
                {
                    if (CurrentFontSize >= Global.MinFontSize)
                    {
                        _config.UiItem.currentFontSize = CurrentFontSize;
                        double size = (long)CurrentFontSize;
                        Application.Current.Resources["StdFontSize1"] = size;
                        Application.Current.Resources["StdFontSize2"] = size + 1;
                        Application.Current.Resources["StdFontSize3"] = size + 2;
                        Application.Current.Resources["StdFontSize4"] = size + 3;

                        ConfigProc.SaveConfig(_config);
                    }
                });

            //CMD
            SaveCommand = ReactiveCommand.Create(() =>
            {
                SaveConfig();
            }, this.IsValid());
        }

        private void SaveConfig()
        {
            //Core
            _config.MixedPort = MixedPort;
            _config.SocksPort = SocksPort;
            _config.HttpPort = HttpPort;
            _config.ApiPort = APIPort;
            _config.AllowLANConn = AllowLANConn;
            _config.EnableIpv6 = EnableIpv6;
            _config.LogLevel = LogLevel;
            _config.EnableMixinContent = EnableMixinContent;

            //ClashN
            Utils.SetAutoRun(AutoRun);
            _config.AutoRun = AutoRun;
            _config.EnableStatistics = EnableStatistics;
            _config.EnableSecurityProtocolTls13 = EnableSecurityProtocolTls13;
            _config.AutoUpdateSubInterval = autoUpdateSubInterval;
            _config.AutoDelayTestInterval = autoDelayTestInterval;
            _config.ConstItem.subConvertUrl = SubConvertUrl;
            _config.UiItem.currentFontFamily = currentFontFamily;
            _config.AutoHideStartup = AutoHideStartup;

            //System proxy
            _config.SystemProxyExceptions = systemProxyExceptions;
            _config.SystemProxyAdvancedProtocol = systemProxyAdvancedProtocol;
            _config.PacPort = PacPort;

            if (ConfigProc.SaveConfig(_config) == 0)
            {
                Locator.Current.GetService<NoticeHandler>()?.Enqueue(ResUI.OperationSuccess);
                Locator.Current.GetService<MainWindowViewModel>()?.LoadCore();
            }
            else
            {
                Locator.Current.GetService<NoticeHandler>()?.Enqueue(ResUI.OperationFailed);
            }
        }

        private void EditMixinContent()
        {
            var address = Utils.GetConfigPath(Global.mixinConfigFileName);
            if (!File.Exists(address))
            {
                string contents = Utils.GetEmbedText(Global.SampleMixin);
                if (!string.IsNullOrEmpty(contents))
                {
                    File.WriteAllText(address, contents);
                }
            }

            if (File.Exists(address))
            {
                Utils.ProcessStart(address);
            }
            else
            {
                Locator.Current.GetService<NoticeHandler>()?.Enqueue(ResUI.FailedReadConfiguration);
            }
        }
    }
}