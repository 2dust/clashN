using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ClashN.ViewModels
{
    public class LogsViewModel : ReactiveObject
    {
        [Reactive]
        public int SortingSelected { get; set; }

        [Reactive]
        public bool AutoRefresh { get; set; }

        [Reactive]
        public string MsgFilter { get; set; }

        [Reactive]
        public int LineCount { get; set; }

        public LogsViewModel()
        {
            AutoRefresh = true;
            MsgFilter = string.Empty;
            LineCount = 1000;
        }
    }
}