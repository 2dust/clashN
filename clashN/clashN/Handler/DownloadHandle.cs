using ClashN.Base;
using ClashN.Resx;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;

namespace ClashN.Handler
{
    /// <summary>
    ///Download
    /// </summary>
    internal class DownloadHandle
    {
        public event EventHandler<ResultEventArgs>? UpdateCompleted;

        public event ErrorEventHandler? Error;

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
                Utils.SetSecurityProtocol(LazyConfig.Instance.Config.EnableSecurityProtocolTls13);
                UpdateCompleted?.Invoke(this, new ResultEventArgs(false, ResUI.Downloading));

                var client = new HttpClient(new SocketsHttpHandler()
                {
                    Proxy = GetWebProxy(blProxy)
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
                       Utils.GetTempPath(Utils.GetDownloadFileName(url)),
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
        public async Task<(string, HttpResponseHeaders)?> DownloadStringAsync(string url, bool blProxy, string userAgent)
        {
            try
            {
                Utils.SetSecurityProtocol(LazyConfig.Instance.Config.EnableSecurityProtocolTls13);
                var webProxy = GetWebProxy(blProxy);
                var client = new HttpClient(new SocketsHttpHandler()
                {
                    Proxy = webProxy,
                    UseProxy = webProxy != null
                });

                if (string.IsNullOrEmpty(userAgent))
                {
                    userAgent = $"{Utils.GetVersion(false)}";
                }
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

                Uri uri = new Uri(url);
                //Authorization Header
                if (!string.IsNullOrEmpty(uri.UserInfo))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Utils.Base64Encode(uri.UserInfo));
                }

                var cts = new CancellationTokenSource();
                cts.CancelAfter(1000 * 30);

                var result = await HttpClientHelper.GetInstance().GetAsync(client, url, cts.Token);
                return result;
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
                Error?.Invoke(this, new ErrorEventArgs(ex));
                if (ex.InnerException != null)
                {
                    Error?.Invoke(this, new ErrorEventArgs(ex.InnerException));
                }
            }

            return null;
        }

        public async Task<string?> UrlRedirectAsync(string url, bool blProxy)
        {
            Utils.SetSecurityProtocol(LazyConfig.Instance.Config.EnableSecurityProtocolTls13);
            SocketsHttpHandler webRequestHandler = new SocketsHttpHandler
            {
                AllowAutoRedirect = false,
                Proxy = GetWebProxy(blProxy)
            };
            HttpClient client = new HttpClient(webRequestHandler);

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode.ToString() == "Redirect")
            {
                return response.Headers.Location?.ToString();
            }
            else
            {
                Utils.SaveLog("StatusCode error: " + url);
                return null;
            }
        }

        private WebProxy GetWebProxy(bool blProxy)
        {
            if (!blProxy)
            {
                return null;
            }
            var socksPort = LazyConfig.Instance.Config.SocksPort;
            if (!SocketCheck(Global.Loopback, socksPort))
            {
                return null;
            }

            return new WebProxy($"socks5://{Global.Loopback}:{socksPort}");
        }

        private bool SocketCheck(string ip, int port)
        {
            Socket sock = null;
            try
            {
                IPAddress ipa = IPAddress.Parse(ip);
                IPEndPoint point = new IPEndPoint(ipa, port);
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(point);
                return true;
            }
            catch { }
            finally
            {
                if (sock != null)
                {
                    sock.Close();
                    sock.Dispose();
                }
            }
            return false;
        }
    }
}