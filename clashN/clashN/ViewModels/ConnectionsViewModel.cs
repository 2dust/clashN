using ClashN.Handler;
using ClashN.Mode;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace ClashN.ViewModels
{
    public class ConnectionsViewModel : ReactiveObject
    {
        private static Config _config;

        static ConnectionsViewModel()
        {
            _config = LazyConfig.Instance.Config;
        }

        private NoticeHandler? _noticeHandler;
        private IObservableCollection<ConnectionModel> _connectionItems = new ObservableCollectionExtended<ConnectionModel>();

        public IObservableCollection<ConnectionModel> ConnectionItems => _connectionItems;

        [Reactive]
        public ConnectionModel SelectedSource { get; set; }

        public ReactiveCommand<Unit, Unit> ConnectionCloseCmd { get; }
        public ReactiveCommand<Unit, Unit> ConnectionCloseAllCmd { get; }

        [Reactive]
        public int SortingSelected { get; set; }

        [Reactive]
        public bool AutoRefresh { get; set; }

        private int AutoRefreshInterval;

        public ConnectionsViewModel()
        {
            _noticeHandler = Locator.Current.GetService<NoticeHandler>();

            AutoRefreshInterval = 10;
            SortingSelected = _config.UiItem.connectionsSorting;
            AutoRefresh = _config.UiItem.connectionsAutoRefresh;

            var canEditRemove = this.WhenAnyValue(
             x => x.SelectedSource,
             selectedSource => selectedSource != null && !string.IsNullOrEmpty(selectedSource.id));

            this.WhenAnyValue(
              x => x.SortingSelected,
              y => y >= 0)
                  .Subscribe(c => DoSortingSelected(c));

            this.WhenAnyValue(
               x => x.AutoRefresh,
               y => y == true)
                   .Subscribe(c => { _config.UiItem.connectionsAutoRefresh = AutoRefresh; });

            ConnectionCloseCmd = ReactiveCommand.Create(() =>
            {
                ClashConnectionClose(false);
            }, canEditRemove);

            ConnectionCloseAllCmd = ReactiveCommand.Create(() =>
            {
                ClashConnectionClose(true);
            });

            Init();
        }

        private void DoSortingSelected(bool c)
        {
            if (!c)
            {
                return;
            }
            if (SortingSelected != _config.UiItem.connectionsSorting)
            {
                _config.UiItem.connectionsSorting = SortingSelected;
            }

            GetClashConnections();
        }

        private void Init()
        {
            Observable.Interval(TimeSpan.FromSeconds(AutoRefreshInterval))
                .Subscribe(x =>
                {
                    if (AutoRefresh && Global.ShowInTaskbar)
                    {
                        GetClashConnections();
                    }
                });

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        if (AutoRefresh)
            //        {
            //            GetClashConnections();
            //        }
            //        Thread.Sleep(1000 * AutoRefreshInterval);
            //    }
            //});
        }

        private void GetClashConnections()
        {
            MainFormHandler.Instance.GetClashConnections(_config, (it) =>
            {
                //_noticeHandler?.SendMessage("Refresh Clash Connections", true);
                if (it == null)
                {
                    return;
                }

                Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    RefreshConnections(it?.Connections!);
                }));
            });
        }

        private void RefreshConnections(List<ConnectionItem> connections)
        {
            _connectionItems.Clear();

            var dtNow = DateTime.Now;
            var lstModel = new List<ConnectionModel>();
            foreach (var item in connections)
            {
                ConnectionModel model = new();

                model.id = item.Id;
                model.network = item.metadata.Network;
                model.type = item.metadata.Type;
                model.host = $"{(string.IsNullOrEmpty(item.metadata.Host) ? item.metadata.DestinationIP : item.metadata.Host)}:{item.metadata.DestinationPort}";
                var sp = (dtNow - item.start);
                model.time = sp.TotalSeconds < 0 ? 1 : sp.TotalSeconds;
                model.upload = item.upload;
                model.download = item.download;
                model.uploadTraffic = $"{Utils.HumanFy(item.upload)}";
                model.downloadTraffic = $"{Utils.HumanFy(item.download)}";
                model.elapsed = sp.ToString(@"hh\:mm\:ss");
                model.chain = item.Chains.Count > 0 ? item.Chains[0] : String.Empty;

                lstModel.Add(model);
            }
            if (lstModel.Count <= 0) { return; }

            //sort
            switch (SortingSelected)
            {
                case 0:
                    lstModel = lstModel.OrderBy(t => t.upload / t.time).ToList();
                    break;

                case 1:
                    lstModel = lstModel.OrderBy(t => t.download / t.time).ToList();
                    break;

                case 2:
                    lstModel = lstModel.OrderBy(t => t.upload).ToList();
                    break;

                case 3:
                    lstModel = lstModel.OrderBy(t => t.download).ToList();
                    break;

                case 4:
                    lstModel = lstModel.OrderBy(t => t.time).ToList();
                    break;

                case 5:
                    lstModel = lstModel.OrderBy(t => t.host).ToList();
                    break;
            }

            _connectionItems.AddRange(lstModel);
        }

        public void ClashConnectionClose(bool all)
        {
            var id = string.Empty;
            if (!all)
            {
                var item = SelectedSource;
                if (item is null)
                {
                    return;
                }
                id = item.id;
            }
            else
            {
                _connectionItems.Clear();
            }
            MainFormHandler.Instance.ClashConnectionClose(id);
            GetClashConnections();
        }
    }
}