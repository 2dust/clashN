using ClashN.Base;

namespace ClashN.Mode
{
    public class ProfileItemModel : ProfileItem
    {
        public bool isActive { get; set; }
        public bool HasUrl
        {
            get { return !url.IsNullOrEmpty(); }
        }
        public bool HasAddress
        {
            get { return !address.IsNullOrEmpty(); }
        }
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
        public string TrafficUsed
        {
            get { return Utils.HumanFy(uploadRemote + downloadRemote); }
        }
        public string TrafficTotal
        {
            get { return totalRemote <= 0 ? "∞" : Utils.HumanFy(totalRemote); }
        }
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
