using ClashN.Base;
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
        #endregion

        #region clashN

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
        #endregion

        #region  System proxy
        [Reactive]
        public string systemProxyExceptions { get; set; }
        [Reactive]
        public string systemProxyAdvancedProtocol { get; set; }
        [Reactive]
        public int PacPort { get; set; }


        #endregion

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
        #endregion

        public ReactiveCommand<Unit, Unit> SaveCommand { get; }


        public SettingsViewModel()
        {
            _config = LazyConfig.Instance.GetConfig();

            //Core
            MixedPort = _config.mixedPort;
            SocksPort = _config.socksPort;
            HttpPort = _config.httpPort;
            APIPort = _config.APIPort;
            AllowLANConn = _config.allowLANConn;
            EnableIpv6 = _config.enableIpv6;
            LogLevel = _config.logLevel;
            EnableMixinContent = _config.enableMixinContent;
            EditMixinContentCmd = ReactiveCommand.Create(() =>
            {
                EditMixinContent();
            }, this.IsValid());

            //clashN       
            AutoRun = _config.autoRun;
            EnableStatistics = _config.enableStatistics;
            EnableSecurityProtocolTls13 = _config.enableSecurityProtocolTls13;
            autoUpdateSubInterval = _config.autoUpdateSubInterval;
            autoDelayTestInterval = _config.autoDelayTestInterval;
            SubConvertUrl = _config.constItem.subConvertUrl;
            currentFontFamily = _config.uiItem.currentFontFamily;
            AutoHideStartup = _config.autoHideStartup;

            SetLoopbackCmd = ReactiveCommand.Create(() =>
            {
                Utils.ProcessStart(Utils.GetBinPath("EnableLoopback.exe"));
            }, this.IsValid());
            SetGlobalHotkeyCmd = ReactiveCommand.Create(() =>
            {
                new GlobalHotkeySettingWindow().ShowDialog();
            }, this.IsValid());

            //System proxy
            systemProxyExceptions = _config.systemProxyExceptions;
            systemProxyAdvancedProtocol = _config.systemProxyAdvancedProtocol;
            PacPort = _config.PacPort;

            //UI
            ColorModeDark = _config.uiItem.colorModeDark;
            _swatches.AddRange(new SwatchesProvider().Swatches);
            if (!_config.uiItem.colorPrimaryName.IsNullOrEmpty())
            {
                SelectedSwatch = _swatches.FirstOrDefault(t => t.Name == _config.uiItem.colorPrimaryName);
            }
            CurrentLanguage = Utils.RegReadValue(Global.MyRegPath, Global.MyRegKeyLanguage, Global.Languages[0]);
            CurrentFontSize = _config.uiItem.currentFontSize;

            this.WhenAnyValue(
            x => x.ColorModeDark,
            y => y == true)
                .Subscribe(c =>
                {
                    if (_config.uiItem.colorModeDark != ColorModeDark)
                    {
                        _config.uiItem.colorModeDark = ColorModeDark;
                        Locator.Current.GetService<MainWindowViewModel>()?.ModifyTheme(ColorModeDark);
                        ConfigHandler.SaveConfig(ref _config);
                    }
                });

            this.WhenAnyValue(
              x => x.SelectedSwatch,
              y => y != null && !y.Name.IsNullOrEmpty())
                 .Subscribe(c =>
                 {
                     if (SelectedSwatch == null
                     || SelectedSwatch.Name.IsNullOrEmpty()
                     || SelectedSwatch.ExemplarHue == null
                     || SelectedSwatch.ExemplarHue?.Color == null)
                     {
                         return;
                     }
                     if (_config.uiItem.colorPrimaryName != SelectedSwatch?.Name)
                     {
                         _config.uiItem.colorPrimaryName = SelectedSwatch?.Name;
                         Locator.Current.GetService<MainWindowViewModel>()?.ChangePrimaryColor(SelectedSwatch.ExemplarHue.Color);
                         ConfigHandler.SaveConfig(ref _config);
                     }
                 });

            this.WhenAnyValue(
             x => x.CurrentLanguage,
             y => y != null && !y.IsNullOrEmpty())
                .Subscribe(c =>
                {
                    if (!Utils.IsNullOrEmpty(CurrentLanguage))
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
                 if (_config.uiItem.colorModeDark != ColorModeDark)
                 {
                     _config.uiItem.colorModeDark = ColorModeDark;
                     Locator.Current.GetService<MainWindowViewModel>()?.ModifyTheme(ColorModeDark);
                     ConfigHandler.SaveConfig(ref _config);
                 }
             });

            this.WhenAnyValue(
             x => x.CurrentFontSize,
             y => y > 0)
                .Subscribe(c =>
                {
                    if (CurrentFontSize >= Global.MinFontSize)
                    {
                        _config.uiItem.currentFontSize = CurrentFontSize;
                        double size = (long)CurrentFontSize;
                        Application.Current.Resources["StdFontSize1"] = size;
                        Application.Current.Resources["StdFontSize2"] = size + 1;
                        Application.Current.Resources["StdFontSize3"] = size + 2;
                        Application.Current.Resources["StdFontSize4"] = size + 3;

                        ConfigHandler.SaveConfig(ref _config);
                    }
                });

            //CMD
            SaveCommand = ReactiveCommand.Create(() =>
            {
                SaveConfig();
            }, this.IsValid());

        }

        void SaveConfig()
        {
            //Core
            _config.mixedPort = MixedPort;
            _config.socksPort = SocksPort;
            _config.httpPort = HttpPort;
            _config.APIPort = APIPort;
            _config.allowLANConn = AllowLANConn;
            _config.enableIpv6 = EnableIpv6;
            _config.logLevel = LogLevel;
            _config.enableMixinContent = EnableMixinContent;

            //clashN                   
            Utils.SetAutoRun(AutoRun);
            _config.autoRun = AutoRun;
            _config.enableStatistics = EnableStatistics;
            _config.enableSecurityProtocolTls13 = EnableSecurityProtocolTls13;
            _config.autoUpdateSubInterval = autoUpdateSubInterval;
            _config.autoDelayTestInterval = autoDelayTestInterval;
            _config.constItem.subConvertUrl = SubConvertUrl;
            _config.uiItem.currentFontFamily = currentFontFamily;
            _config.autoHideStartup = AutoHideStartup;

            //System proxy
            _config.systemProxyExceptions = systemProxyExceptions;
            _config.systemProxyAdvancedProtocol = systemProxyAdvancedProtocol;
            _config.PacPort = PacPort;

            if (ConfigHandler.SaveConfig(ref _config) == 0)
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
                if (!Utils.IsNullOrEmpty(contents))
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