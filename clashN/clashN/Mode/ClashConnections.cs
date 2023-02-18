namespace ClashN.Mode
{
    public class ClashConnections
    {
        public ulong DownloadTotal { get; set; }
        public ulong UploadTotal { get; set; }
        public List<ConnectionItem> Connections { get; } = new List<ConnectionItem>();
    }

    public class ConnectionItem
    {
        public string Id { get; set; } = string.Empty;
        public MetadataItem metadata { get; set; }
        public ulong upload { get; set; }
        public ulong download { get; set; }
        public DateTime start { get; set; }
        public List<string> Chains { get; } = new List<string>();
        public string rule { get; set; }
        public string rulePayload { get; set; }
    }

    public class MetadataItem
    {
        public string Network { get; set; }
        public string Type { get; set; }
        public string SourceIP { get; set; }
        public string DestinationIP { get; set; }
        public string SourcePort { get; set; }
        public string DestinationPort { get; set; }
        public string Host { get; set; }
        public string DnsMode { get; set; }
        public object Uid { get; set; }
        public string Process { get; set; }
        public string ProcessPath { get; set; }
        public string RemoteDestination { get; set; }
    }
}