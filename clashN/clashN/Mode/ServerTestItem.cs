namespace ClashN.Mode
{
    [Serializable]
    internal class ServerTestItem
    {
        public string IndexId { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public bool AllowTest { get; set; }
    }
}