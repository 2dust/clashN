using ClashN.Mode;
using System.Net.WebSockets;
using System.Text;

namespace ClashN.Handler
{
    internal class StatisticsHandler
    {
        private Config config_;

        //private ServerStatistics serverStatistics_;
        private bool exitFlag_;

        private ClientWebSocket webSocket = null;
        private string url = string.Empty;

        private Action<ulong, ulong> updateFunc_;

        private bool Enable
        {
            get; set;
        }

        //private List<ProfileStatItem> Statistic
        //{
        //    get
        //    {
        //        return serverStatistics_.profileStat;
        //    }
        //}

        public StatisticsHandler(Config config, Action<ulong, ulong> update)
        {
            config_ = config;
            Enable = config.EnableStatistics;
            updateFunc_ = update;
            exitFlag_ = false;

            //LoadFromFile();

            Task.Run(() => Run());
        }

        private async void Init()
        {
            Thread.Sleep(5000);

            try
            {
                url = $"ws://{Global.Loopback}:{config_.ApiPort}/traffic";

                if (webSocket == null)
                {
                    webSocket = new ClientWebSocket();
                    await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);
                }
            }
            catch { }
        }

        public void Close()
        {
            try
            {
                exitFlag_ = true;
                if (webSocket != null)
                {
                    webSocket.Abort();
                    webSocket = null;
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
        }

        public async void Run()
        {
            Init();

            while (!exitFlag_)
            {
                try
                {
                    if (Enable)
                    {
                        if (webSocket.State == WebSocketState.Aborted
                            || webSocket.State == WebSocketState.Closed)
                        {
                            webSocket.Abort();
                            webSocket = null;
                            Init();
                        }

                        if (webSocket.State != WebSocketState.Open)
                        {
                            continue;
                        }

                        var buffer = new byte[1024];
                        var res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        while (!res.CloseStatus.HasValue)
                        {
                            var result = Encoding.UTF8.GetString(buffer, 0, res.Count);
                            if (!string.IsNullOrEmpty(result))
                            {
                                var serverStatItem = config_.GetProfileItem(config_.IndexId);
                                ParseOutput(result, out ulong up, out ulong down);
                                if (up + down > 0)
                                {
                                    serverStatItem.uploadRemote += up;
                                    serverStatItem.downloadRemote += down;
                                }
                                updateFunc_(up, down);
                            }
                            res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        //public void LoadFromFile()
        //{
        //    try
        //    {
        //        string result = Utils.LoadResource(Utils.GetConfigPath(Global.StatisticLogOverall));
        //        if (!string.IsNullOrEmpty(result))
        //        {
        //            serverStatistics_ = Utils.FromJson<ServerStatistics>(result);
        //        }

        //        if (serverStatistics_ == null)
        //        {
        //            serverStatistics_ = new ServerStatistics();
        //        }
        //        if (serverStatistics_.profileStat == null)
        //        {
        //            serverStatistics_.profileStat = new List<ProfileStatItem>();
        //        }

        //        long ticks = DateTime.Now.Date.Ticks;
        //        foreach (ProfileStatItem item in serverStatistics_.profileStat)
        //        {
        //            if (item.dateNow != ticks)
        //            {
        //                item.todayUp = 0;
        //                item.todayDown = 0;
        //                item.dateNow = ticks;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.SaveLog(ex.Message, ex);
        //    }
        //}

        //public void SaveToFile()
        //{
        //    try
        //    {
        //        Utils.ToJsonFile(serverStatistics_, Utils.GetConfigPath(Global.StatisticLogOverall));
        //    }
        //    catch (Exception ex)
        //    {
        //        Utils.SaveLog(ex.Message, ex);
        //    }
        //}

        //public void ClearAllServerStatistics()
        //{
        //    if (serverStatistics_ != null)
        //    {
        //        foreach (var item in serverStatistics_.profileStat)
        //        {
        //            item.todayUp = 0;
        //            item.todayDown = 0;
        //            item.totalUp = 0;
        //            item.totalDown = 0;
        //            // update ui display to zero
        //            updateFunc_(0, 0);
        //        }

        //        // update statistic json file
        //        //SaveToFile();
        //    }
        //}

        //public List<ProfileStatItem> GetStatistic()
        //{
        //    return Statistic;
        //}

        //private ProfileStatItem GetServerStatItem(string itemId)
        //{
        //    long ticks = DateTime.Now.Date.Ticks;
        //    int cur = Statistic.FindIndex(item => item.indexId == itemId);
        //    if (cur < 0)
        //    {
        //        Statistic.Add(new ProfileStatItem
        //        {
        //            indexId = itemId,
        //            totalUp = 0,
        //            totalDown = 0,
        //            todayUp = 0,
        //            todayDown = 0,
        //            dateNow = ticks
        //        });
        //        cur = Statistic.Count - 1;
        //    }
        //    if (Statistic[cur].dateNow != ticks)
        //    {
        //        Statistic[cur].todayUp = 0;
        //        Statistic[cur].todayDown = 0;
        //        Statistic[cur].dateNow = ticks;
        //    }
        //    return Statistic[cur];
        //}

        private void ParseOutput(string source, out ulong up, out ulong down)
        {
            up = 0; down = 0;
            try
            {
                var trafficItem = Utils.FromJson<TrafficItem>(source);
                if (trafficItem != null)
                {
                    up = trafficItem.up;
                    down = trafficItem.down;
                }
            }
            catch (Exception)
            {
                //Utils.SaveLog(ex.Message, ex);
            }
        }
    }
}