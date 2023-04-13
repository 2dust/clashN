using ClashN.Handler;
using ClashN.Mode;
using ClashN.Resx;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using static ClashN.Mode.ClashProviders;
using static ClashN.Mode.ClashProxies;

namespace ClashN.ViewModels
{
    public class ProxiesViewModel : ReactiveObject
    {
        private static Config _config;
        private NoticeHandler? _noticeHandler;
        private Dictionary<String, ProxiesItem> proxies;
        private Dictionary<String, ProvidersItem> providers;
        private int delayTimeout = 99999999;

        private IObservableCollection<ProxyModel> _proxyGroups = new ObservableCollectionExtended<ProxyModel>();
        private IObservableCollection<ProxyModel> _proxyDetails = new ObservableCollectionExtended<ProxyModel>();

        public IObservableCollection<ProxyModel> ProxyGroups => _proxyGroups;
        public IObservableCollection<ProxyModel> ProxyDetails => _proxyDetails;

        [Reactive]
        public ProxyModel SelectedGroup { get; set; }

        [Reactive]
        public ProxyModel SelectedDetail { get; set; }

        public ReactiveCommand<Unit, Unit> ProxiesReloadCmd { get; }
        public ReactiveCommand<Unit, Unit> ProxiesDelaytestCmd { get; }
        public ReactiveCommand<Unit, Unit> ProxiesDelaytestPartCmd { get; }
        public ReactiveCommand<Unit, Unit> ProxiesSelectActivityCmd { get; }

        [Reactive]
        public int SystemProxySelected { get; set; }

        [Reactive]
        public int RuleModeSelected { get; set; }

        [Reactive]
        public int SortingSelected { get; set; }

        [Reactive]
        public bool AutoRefresh { get; set; }

        [Reactive]
        public bool EnableTun { get; set; }

        public ProxiesViewModel()
        {
            _noticeHandler = Locator.Current.GetService<NoticeHandler>();
            _config = LazyConfig.Instance.Config;

            SelectedGroup = new();
            SelectedDetail = new();
            AutoRefresh = _config.UiItem.proxiesAutoRefresh;
            EnableTun = _config.EnableTun;
            SortingSelected = _config.UiItem.proxiesSorting;

            //GetClashProxies(true);
            this.WhenAnyValue(
               x => x.SelectedGroup,
               y => y != null && !string.IsNullOrEmpty(y.name))
                   .Subscribe(c => RefreshProxyDetails(c));

            this.WhenAnyValue(
              x => x.SystemProxySelected,
              y => y >= 0)
                  .Subscribe(c => DoSystemProxySelected(c));

            this.WhenAnyValue(
               x => x.RuleModeSelected,
               y => y >= 0)
                   .Subscribe(c => DoRulemodeSelected(c));

            this.WhenAnyValue(
               x => x.SortingSelected,
               y => y >= 0)
                  .Subscribe(c => DoSortingSelected(c));

            this.WhenAnyValue(
                x => x.EnableTun,
                 y => y == true)
                    .Subscribe(c => DoEnableTun(c));

            this.WhenAnyValue(
            x => x.AutoRefresh,
            y => y == true)
                .Subscribe(c => { _config.UiItem.proxiesAutoRefresh = AutoRefresh; });

            ProxiesReloadCmd = ReactiveCommand.Create(() =>
            {
                ProxiesReload();
            });
            ProxiesDelaytestCmd = ReactiveCommand.Create(() =>
            {
                ProxiesDelayTest(true);
            });

            ProxiesDelaytestPartCmd = ReactiveCommand.Create(() =>
            {
                ProxiesDelayTest(false);
            });
            ProxiesSelectActivityCmd = ReactiveCommand.Create(() =>
            {
                SetActiveProxy();
            });

            ReloadSystemProxySelected();
            ReloadRulemodeSelected();

            DelayTestTask();
        }

        private void DoSystemProxySelected(bool c)
        {
            if (!c)
            {
                return;
            }
            if (_config.SysProxyType == (SysProxyType)SystemProxySelected)
            {
                return;
            }
            Locator.Current.GetService<MainWindowViewModel>()?.SetListenerType((SysProxyType)SystemProxySelected);
        }

        private void DoRulemodeSelected(bool c)
        {
            if (!c)
            {
                return;
            }
            if (_config.ruleMode == (ERuleMode)RuleModeSelected)
            {
                return;
            }
            Locator.Current.GetService<MainWindowViewModel>()?.SetRuleModeCheck((ERuleMode)RuleModeSelected);
        }

        private void DoSortingSelected(bool c)
        {
            if (!c)
            {
                return;
            }
            if (SortingSelected != _config.UiItem.proxiesSorting)
            {
                _config.UiItem.proxiesSorting = SortingSelected;
            }

            RefreshProxyDetails(c);
        }

        private void UpdateHandler(bool notify, string msg)
        {
            _noticeHandler?.SendMessage(msg, true);
        }

        public void ProxiesReload()
        {
            GetClashProxies(true);
        }

        public void ProxiesClear()
        {
            proxies = null;
            providers = null;

            LazyConfig.Instance.SetProxies(proxies);

            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                _proxyGroups.Clear();
                _proxyDetails.Clear();
            }));
        }

        public void ProxiesDelayTest()
        {
            ProxiesDelayTest(true);
        }

        public void ReloadSystemProxySelected()
        {
            SystemProxySelected = (int)_config.SysProxyType;
        }

        public void ReloadRulemodeSelected()
        {
            RuleModeSelected = (int)_config.ruleMode;
        }

        private void DoEnableTun(bool c)
        {
            if (_config.EnableTun != EnableTun)
            {
                _config.EnableTun = EnableTun;
                TunModeSwitch();
            }
        }

        private void TunModeSwitch()
        {
            Global.reloadCore = true;
            _ = Locator.Current.GetService<MainWindowViewModel>()?.LoadCore();
        }

        #region proxy function

        private void GetClashProxies(bool refreshUI)
        {
            MainFormHandler.Instance.GetClashProxies(_config, (it, it2) =>
            {
                UpdateHandler(false, "Refresh Clash Proxies");
                proxies = it?.proxies;
                providers = it2?.providers;

                LazyConfig.Instance.SetProxies(proxies);
                if (proxies == null)
                {
                    return;
                }
                if (refreshUI)
                {
                    Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        RefreshProxyGroups();
                    }));
                }
            });
        }

        private void RefreshProxyGroups()
        {
            var selectedName = SelectedGroup?.name;
            _proxyGroups.Clear();

            var proxyGroups = MainFormHandler.Instance.GetClashProxyGroups();
            if (proxyGroups != null && proxyGroups.Count > 0)
            {
                foreach (var it in proxyGroups)
                {
                    if (string.IsNullOrEmpty(it.name) || !proxies.ContainsKey(it.name))
                    {
                        continue;
                    }
                    var item = proxies[it.name];
                    if (!Global.allowSelectType.Contains(item.type.ToLower()))
                    {
                        continue;
                    }
                    _proxyGroups.Add(new ProxyModel()
                    {
                        now = item.now,
                        name = item.name,
                        type = item.type
                    });
                }
            }

            //from api
            foreach (KeyValuePair<string, ProxiesItem> kv in proxies)
            {
                if (!Global.allowSelectType.Contains(kv.Value.type.ToLower()))
                {
                    continue;
                }
                var item = _proxyGroups.Where(t => t.name == kv.Key).FirstOrDefault();
                if (item != null && !string.IsNullOrEmpty(item.name))
                {
                    continue;
                }
                _proxyGroups.Add(new ProxyModel()
                {
                    now = kv.Value.now,
                    name = kv.Key,
                    type = kv.Value.type
                });
            }

            if (_proxyGroups != null && _proxyGroups.Count > 0)
            {
                if (selectedName != null && _proxyGroups.Any(t => t.name == selectedName))
                {
                    SelectedGroup = _proxyGroups.FirstOrDefault(t => t.name == selectedName);
                }
                else
                {
                    SelectedGroup = _proxyGroups[0];
                }
            }
            else
            {
                SelectedGroup = new();
            }
        }

        private void RefreshProxyDetails(bool c)
        {
            _proxyDetails.Clear();
            if (!c)
            {
                return;
            }
            var name = SelectedGroup?.name;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            if (proxies == null)
            {
                return;
            }

            proxies.TryGetValue(name, out ProxiesItem proxy);
            if (proxy == null || proxy.all == null)
            {
                return;
            }
            var lstDetails = new List<ProxyModel>();
            foreach (var item in proxy.all)
            {
                var isActive = item == proxy.now;

                var proxy2 = TryGetProxy(item);
                if (proxy2 == null)
                {
                    continue;
                }
                int delay = -1;
                if (proxy2.history.Count > 0)
                {
                    delay = proxy2.history[proxy2.history.Count - 1].delay;
                }

                lstDetails.Add(new ProxyModel()
                {
                    isActive = isActive,
                    name = item,
                    type = proxy2.type,
                    delay = delay <= 0 ? delayTimeout : delay,
                    delayName = delay <= 0 ? string.Empty : $"{delay}ms",
                });
            }
            //sort
            switch (SortingSelected)
            {
                case 0:
                    lstDetails = lstDetails.OrderBy(t => t.delay).ToList();
                    break;

                case 1:
                    lstDetails = lstDetails.OrderBy(t => t.name).ToList();
                    break;

                default:
                    break;
            }
            _proxyDetails.AddRange(lstDetails);
        }

        private ProxiesItem TryGetProxy(string name)
        {
            proxies.TryGetValue(name, out ProxiesItem proxy2);
            if (proxy2 != null)
            {
                return proxy2;
            }
            //from providers
            if (providers != null)
            {
                foreach (KeyValuePair<string, ProvidersItem> kv in providers)
                {
                    if (Global.proxyVehicleType.Contains(kv.Value.vehicleType.ToLower()))
                    {
                        var proxy3 = kv.Value.proxies.FirstOrDefault(t => t.name == name);
                        if (proxy3 != null)
                        {
                            return proxy3;
                        }
                    }
                }
            }
            return null;
        }

        public void SetActiveProxy()
        {
            if (SelectedGroup == null || string.IsNullOrEmpty(SelectedGroup.name))
            {
                return;
            }
            if (SelectedDetail == null || string.IsNullOrEmpty(SelectedDetail.name))
            {
                return;
            }
            var name = SelectedGroup.name;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            var nameNode = SelectedDetail.name;
            if (string.IsNullOrEmpty(nameNode))
            {
                return;
            }
            var selectedProxy = TryGetProxy(name);
            if (selectedProxy == null || selectedProxy.type != "Selector")
            {
                _noticeHandler?.Enqueue(ResUI.OperationFailed);
                return;
            }

            MainFormHandler.Instance.ClashSetActiveProxy(name, nameNode);

            selectedProxy.now = nameNode;
            var group = _proxyGroups.Where(it => it.name == SelectedGroup.name).FirstOrDefault();
            if (group != null)
            {
                group.now = nameNode;
                var group2 = Utils.DeepCopy(group);
                _proxyGroups.Replace(group, group2);

                SelectedGroup = group2;

                //var index = _proxyGroups.IndexOf(group);
                //_proxyGroups.Remove(group);
                //_proxyGroups.Insert(index, group);
            }
            _noticeHandler?.Enqueue(ResUI.OperationSuccess);

            //RefreshProxyDetails(true);
            //GetClashProxies(true);
        }

        private void ProxiesDelayTest(bool blAll)
        {
            UpdateHandler(false, "Clash Proxies Latency Test");

            MainFormHandler.Instance.ClashProxiesDelayTest(blAll, _proxyDetails.ToList(), (item, result) =>
            {
                if (item == null)
                {
                    GetClashProxies(true);
                    return;
                }
                if (string.IsNullOrEmpty(result))
                {
                    return;
                }
                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    //UpdateHandler(false, $"{item.name}={result}");
                    var detail = _proxyDetails.Where(it => it.name == item.name).FirstOrDefault();
                    if (detail != null)
                    {
                        var dicResult = Utils.FromJson<Dictionary<string, object>>(result);
                        if (dicResult != null && dicResult.ContainsKey("delay"))
                        {
                            detail.delay = Convert.ToInt32(dicResult["delay"]);
                            detail.delayName = $"{dicResult["delay"]}ms";
                        }
                        else if (dicResult != null && dicResult.ContainsKey("message"))
                        {
                            detail.delay = delayTimeout;
                            detail.delayName = $"{dicResult["message"]}";
                        }
                        else
                        {
                            detail.delay = delayTimeout;
                            detail.delayName = String.Empty;
                        }
                        _proxyDetails.Replace(detail, Utils.DeepCopy(detail));
                    }
                }));
            });
        }

        #endregion proxy function

        #region task

        public void DelayTestTask()
        {
            var autoDelayTestTime = DateTime.Now;

            Observable.Interval(TimeSpan.FromSeconds(60))
              .Subscribe(x =>
              {
                  if (!AutoRefresh || !Global.ShowInTaskbar)
                  {
                      return;
                  }
                  var dtNow = DateTime.Now;

                  if (_config.AutoDelayTestInterval > 0)
                  {
                      if ((dtNow - autoDelayTestTime).Minutes % _config.AutoDelayTestInterval == 0)
                      {
                          ProxiesDelayTest();
                          autoDelayTestTime = dtNow;
                      }
                      Thread.Sleep(1000);
                  }
              });

            //Task.Run(() =>
            //{
            //    var autoDelayTestTime = DateTime.Now;

            //    Thread.Sleep(1000);

            //    while (true)
            //    {
            //        var dtNow = DateTime.Now;

            //        if (_config.autoDelayTestInterval > 0)
            //        {
            //            if ((dtNow - autoDelayTestTime).Minutes % _config.autoDelayTestInterval == 0)
            //            {
            //                ProxiesDelayTest();
            //                autoDelayTestTime = dtNow;
            //            }
            //            Thread.Sleep(1000);
            //        }

            //        Thread.Sleep(1000 * 60);
            //    }
            //}

            //);
        }

        #endregion task
    }
}