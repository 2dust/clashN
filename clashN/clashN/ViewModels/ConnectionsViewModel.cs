using clashN.Base;
using clashN.Handler;
using clashN.Mode;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace clashN.ViewModels
{
    public class ConnectionsViewModel : ReactiveObject
    {
        private static Config _config;
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
            _config = LazyConfig.Instance.GetConfig();

            var canEditRemove = this.WhenAnyValue(
             x => x.SelectedSource,
             selectedSource => selectedSource != null && !selectedSource.id.IsNullOrEmpty());

            this.WhenAnyValue(
              x => x.SortingSelected,
              y => y != null && y >= 0)
                  .Subscribe(c => DoSortingSelected(c));

            ConnectionCloseCmd = ReactiveCommand.Create(() =>
            {
                ClashConnectionClose(false);
            }, canEditRemove);

            ConnectionCloseAllCmd = ReactiveCommand.Create(() =>
            {
                ClashConnectionClose(true);
            });

            AutoRefreshInterval = 10;
            SortingSelected = 4;
            AutoRefresh = true;

            Init();
        }

        void DoSortingSelected(bool c)
        {
            if (!c)
            {
                return;
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
                    RefreshConnections(it?.connections!);
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

                model.id = item.id;
                model.network = item.metadata.network;
                model.type = item.metadata.type;
                model.host = $"{item.metadata.host}:{item.metadata.destinationPort}";
                var sp = (dtNow - item.start);
                model.time = sp.TotalSeconds < 0 ? 1 : sp.TotalSeconds;
                model.upload = item.upload;
                model.download = item.download;
                model.uploadTraffic = $"¡ü {Utils.HumanFy(item.upload)}";
                model.downloadTraffic = $"¡ý {Utils.HumanFy(item.download)}";
                model.elapsed = sp.ToString(@"hh\:mm\:ss");
                model.chain = item.chains.Count > 0 ? item.chains[0] : String.Empty;

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