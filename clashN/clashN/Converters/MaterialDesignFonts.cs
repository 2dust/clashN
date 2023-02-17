using ClashN.Handler;
using System.Windows.Media;

namespace ClashN.Converters
{
    public class MaterialDesignFonts
    {
        public static FontFamily MyFont { get; }

        static MaterialDesignFonts()
        {
            try
            {
                var fontFamily = LazyConfig.Instance.Config.UiItem.currentFontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var fontPath = Utils.GetFontsPath();
                    MyFont = new FontFamily(new Uri(@$"file:///{fontPath}\"), $"./#{fontFamily}");
                }
            }
            catch
            {
            }
            if (MyFont is null)
            {
                MyFont = new FontFamily("Microsoft YaHei");
            }
        }
    }
}