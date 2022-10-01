using static clashN.Mode.ClashProxies;

namespace clashN.Mode
{
    public class ClashProviders
    {
        public Dictionary<String, ProvidersItem> providers { get; set; }

        public class ProvidersItem
        {
            public string name { get; set; }
            public ProxiesItem[] proxies { get; set; }
            public string type { get; set; }
            public string vehicleType { get; set; }
        }
    }
}