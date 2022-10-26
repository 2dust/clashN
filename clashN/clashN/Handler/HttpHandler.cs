using System.IO;
using System.Windows;
using clashN.Mode;
using TouchSocket.Core.Config;
using TouchSocket.Core.Plugins;
using TouchSocket.Http;
using TouchSocket.Http.Plugins;
using TouchSocket.Sockets;

namespace clashN.Handler;

public class HttpHandler : HttpPluginBase
{
    private static HttpService _httpService = new();
    private static string _pacText;

    public static void Start(Config config)
    {
        if (!File.Exists("./pac.txt"))
        {
            MessageBox.Show("pac file is not exists", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _pacText = File.ReadAllText("./pac.txt").Replace("__PROXY__", $"PROXY 127.0.0.1:{config.httpPort};DIRECT;");
        Stop();
        _httpService.Setup(config.PacPort).Start();
        _httpService.AddPlugin<HttpHandler>();
    }

    public static void Stop()
    {
        if (_httpService.ServerState == ServerState.Running)
        {
            _httpService.Stop();
        }
    }

    protected override void OnGet(ITcpClientBase client, HttpContextEventArgs e)
    {
        e.Context.Response.FromText(_pacText).Answer();
        base.OnGet(client, e);
    }
}