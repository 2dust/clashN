using System;
using System.IO;
using System.Windows.Media;

namespace clashN.Converters
{
    public class MaterialDesignFonts
    {
        public static FontFamily MyFont { get; }

        static MaterialDesignFonts()
        {
            var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Fonts\");
            MyFont = new FontFamily(new Uri($"file:///{fontPath}"), "./#MiSans");
        }
    }
}