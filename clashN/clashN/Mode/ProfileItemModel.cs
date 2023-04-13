namespace ClashN.Mode
{
    public class ProfileItemModel : ProfileItem
    {
        public bool IsActive { get; set; }
        public bool HasUrl => !string.IsNullOrEmpty(url);
        public bool HasAddress => !string.IsNullOrEmpty(address);

        public string StrUpdateTime
        {
            get
            {
                if (updateTime <= 0)
                {
                    return String.Empty;
                }
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(updateTime).ToLocalTime().ToString("MM-dd HH:mm");
            }
        }

        public string TrafficUsed => Utils.HumanFy(uploadRemote + downloadRemote);
        public string TrafficTotal => totalRemote <= 0 ? "∞" : Utils.HumanFy(totalRemote);

        public string StrExpireTime
        {
            get
            {
                if (expireRemote <= 0)
                {
                    return String.Empty;
                }
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(expireRemote).ToLocalTime().ToString("yyyy-MM-dd");
            }
        }
    }
}