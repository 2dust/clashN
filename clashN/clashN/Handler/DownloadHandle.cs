using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using clashN.Base;
using clashN.Resx;

namespace clashN.Handler
{
    /// <summary>
    ///Download
    /// </summary>
    class DownloadHandle
    {
        public event EventHandler<ResultEventArgs> UpdateCompleted;

        public event ErrorEventHandler Error;


        public class ResultEventArgs : EventArgs
        {
            public bool Success;
            public string Msg;

            public ResultEventArgs(bool success, string msg)
            {
                this.Success = success;
                this.Msg = msg;
            }
        }

        public void DownloadFileAsync(string url, bool blProxy, int downloadTimeout)
        {
            try
            {
                Utils.SetSecurityProtocol(LazyConfig.Instance.GetConfig().enableSecurityProtocolTls13);
                UpdateCompleted?.Invoke(this, new ResultEventArgs(false, ResUI.Downloading));

                var client = new HttpClient(new SocketsHttpHandler()
                {
                    Proxy = blProxy ? new WebProxy($"socks5://{Global.Loopback}:{LazyConfig.Instance.GetConfig().socksPort}") : null
                });

                var progress = new Progress<double>();
                progress.ProgressChanged += (sender, value) =>
                {
                    if (UpdateCompleted != null)
                    {
                        string msg = string.Format("...{0}%", value);
                        UpdateCompleted(this, new ResultEventArgs(value > 100 ? true : false, msg));
                    }
                };

                var cancellationToken = new CancellationTokenSource();
                _ = HttpClientHelper.GetInstance().DownloadFileAsync(client,
                       url,
                       Utils.GetPath(Utils.GetDownloadFileName(url)),
                       progress,
                       cancellationToken.Token);
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);

                Error?.Invoke(this, new ErrorEventArgs(ex));
            }
        }

        /// <summary>
        /// DownloadString
        /// </summary> 
        /// <param name="url"></param>
        public async Task<string> DownloadStringAsync(string url, bool blProxy, string userAgent)
        {
            try
            {
                Utils.SetSecurityProtocol(LazyConfig.Instance.GetConfig().enableSecurityProtocolTls13);
                var client = new HttpClient(new SocketsHttpHandler()
                {
                    Proxy = blProxy ? new WebProxy($"socks5://{Global.Loopback}:{LazyConfig.Instance.GetConfig().socksPort}") : null
                });

                if (Utils.IsNullOrEmpty(userAgent))
                {
                    userAgent = $"{Utils.GetVersion(false)}";
                }
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

                Uri uri = new Uri(url);
                //Authorization Header
                if (!Utils.IsNullOrEmpty(uri.UserInfo))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Utils.Base64Encode(uri.UserInfo));
                }
                var result = await HttpClientHelper.GetInstance().GetAsync(client, url);
                return result;
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
            return null;
        }

    }
}
