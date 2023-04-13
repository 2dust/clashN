using System.Windows.Forms;

namespace ClashN.Mode
{
    [Serializable]
    public struct KeyShortcut
    {
        public GlobalHotkeyAction GlobalHotkey { get; set; }

        public bool Alt { get; set; }

        public bool Control { get; set; }

        public bool Shift { get; set; }

        public Keys? KeyCode { get; set; }
    }
}