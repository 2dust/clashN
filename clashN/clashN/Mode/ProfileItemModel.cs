namespace clashN.Mode
{
    public class ProfileItemModel : ProfileItem
    {
        public bool isActive { get; set; }

        public string totalUp
        {
            get; set;
        }
        public string totalDown
        {
            get; set;
        }
        public string todayUp
        {
            get; set;
        }
        public string todayDown
        {
            get; set;
        }
    }
}
