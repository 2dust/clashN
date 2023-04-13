namespace ClashN.Mode
{
    [Serializable]
    public class ServerStatistics
    {
        public List<ProfileStatItem> profileStat
        {
            get; set;
        }
    }

    [Serializable]
    public class ProfileStatItem
    {
        public string indexId
        {
            get; set;
        }

        public ulong totalUp
        {
            get; set;
        }

        public ulong totalDown
        {
            get; set;
        }

        public ulong todayUp
        {
            get; set;
        }

        public ulong todayDown
        {
            get; set;
        }

        public long dateNow
        {
            get; set;
        }
    }

    [Serializable]
    public class TrafficItem
    {
        public ulong up
        {
            get; set;
        }

        public ulong down
        {
            get; set;
        }
    }
}