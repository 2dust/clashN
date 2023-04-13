using ClashN.Handler;
using ClashN.Mode;
using ClashN.ViewModels;
using ReactiveUI;
using System.Globalization;
using System.IO;
using System.Reactive.Disposables;
using System.Windows.Media;

namespace ClashN.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        private static Config _config;

        public SettingsView()
        {
            InitializeComponent();
            _config = LazyConfig.Instance.Config;
            ViewModel = new SettingsViewModel();

            Global.SubConvertUrls.ForEach(it =>
            {
                cmbSubConvertUrl.Items.Add(it);
            });
            Global.Languages.ForEach(it =>
            {
                cmbCurrentLanguage.Items.Add(it);
            });
            Global.IEProxyProtocols.ForEach(it =>
            {
                cmbsystemProxyAdvancedProtocol.Items.Add(it);
            });
            Global.LogLevel.ForEach(it =>
            {
                cmbLogLevel.Items.Add(it);
            });

            for (int i = Global.MinFontSize; i <= Global.MinFontSize + 8; i++)
            {
                cmbCurrentFontSize.Items.Add(i.ToString());
            }

            //fill fonts
            try
            {
                var dir = new DirectoryInfo(Utils.GetFontsPath());
                var files = dir.GetFiles("*.ttf");
                var culture = "zh-cn";
                var culture2 = "en-us";
                foreach (var it in files)
                {
                    var families = Fonts.GetFontFamilies(Utils.GetFontsPath(it.Name));
                    foreach (FontFamily family in families)
                    {
                        var typefaces = family.GetTypefaces();
                        foreach (Typeface typeface in typefaces)
                        {
                            typeface.TryGetGlyphTypeface(out GlyphTypeface glyph);
                            //var fontFace = glyph.Win32FaceNames[new CultureInfo("en-us")];
                            //if (!fontFace.Equals("Regular") && !fontFace.Equals("Normal"))
                            //{
                            //    continue;
                            //}
                            var fontFamily = glyph.Win32FamilyNames[new CultureInfo(culture)];
                            if (string.IsNullOrEmpty(fontFamily))
                            {
                                fontFamily = glyph.Win32FamilyNames[new CultureInfo(culture2)];
                                if (string.IsNullOrEmpty(fontFamily))
                                {
                                    continue;
                                }
                            }
                            cmbcurrentFontFamily.Items.Add(fontFamily);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog("fill fonts error", ex);
            }
            cmbcurrentFontFamily.Items.Add(string.Empty);

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.MixedPort, v => v.txtMixedPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SocksPort, v => v.txtSocksPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.HttpPort, v => v.txtHttpPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.APIPort, v => v.txtAPIPort.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AllowLANConn, v => v.togAllowLANConn.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableIpv6, v => v.togEnableIpv6.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.LogLevel, v => v.cmbLogLevel.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableMixinContent, v => v.togEnableMixinContent.IsChecked).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.EditMixinContentCmd, v => v.btnEditMixinContent).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Swatches, v => v.cmbSwatches.ItemsSource).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedSwatch, v => v.cmbSwatches.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.ColorModeDark, v => v.togDarkMode.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.CurrentLanguage, v => v.cmbCurrentLanguage.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.CurrentFontSize, v => v.cmbCurrentFontSize.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AutoRun, v => v.togAutoRun.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableStatistics, v => v.togEnableStatistics.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.EnableSecurityProtocolTls13, v => v.togEnableSecurityProtocolTls13.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AutoHideStartup, v => v.togAutoHideStartup.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.autoUpdateSubInterval, v => v.txtautoUpdateSubInterval.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.autoDelayTestInterval, v => v.txtautoDelayTestInterval.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SubConvertUrl, v => v.cmbSubConvertUrl.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.currentFontFamily, v => v.cmbcurrentFontFamily.Text).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SetLoopbackCmd, v => v.btnSetLoopback).DisposeWith(disposables);
                this.BindCommand(ViewModel, vm => vm.SetGlobalHotkeyCmd, v => v.btnSetGlobalHotkey).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.systemProxyExceptions, v => v.txtsystemProxyExceptions.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.systemProxyAdvancedProtocol, v => v.cmbsystemProxyAdvancedProtocol.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.PacPort, v => v.txtPacPort.Text).DisposeWith(disposables);

                this.BindCommand(ViewModel, vm => vm.SaveCommand, v => v.btnSave).DisposeWith(disposables);
            });
        }
    }
}