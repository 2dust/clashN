using ClashN.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;

namespace ClashN.Views
{
    /// <summary>
    /// Interaction logic for LogsView.xaml
    /// </summary>
    public partial class LogsView
    {
        public LogsView()
        {
            InitializeComponent();
            ViewModel = new LogsViewModel();

            MessageBus.Current.Listen<string>("MsgView").Subscribe(x => DelegateAppendText(x));

            this.WhenActivated(disposables =>
            {
                this.Bind(ViewModel, vm => vm.MsgFilter, v => v.txtFilter.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.AutoRefresh, v => v.togAutoRefresh.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.LineCount, v => v.cmbLineCount.Text).DisposeWith(disposables);
            });
        }

        private void DelegateAppendText(string msg)
        {
            Dispatcher.BeginInvoke(new Action<string>(AppendText), DispatcherPriority.Send, msg);
        }

        public void AppendText(string msg)
        {
            if (ViewModel?.AutoRefresh == false)
            {
                return;
            }
            string? msgFilter = ViewModel?.MsgFilter;
            if (!string.IsNullOrEmpty(msgFilter))
            {
                if (!Regex.IsMatch(msg, msgFilter))
                {
                    return;
                }
            }

            ShowMsg(msg);
        }

        private void ShowMsg(string msg)
        {
            if (txtMsg.LineCount > ViewModel?.LineCount)
            {
                ClearMsg();
            }
            this.txtMsg.AppendText(msg);
            if (!msg.EndsWith(Environment.NewLine))
            {
                this.txtMsg.AppendText(Environment.NewLine);
            }
            txtMsg.ScrollToEnd();
        }

        public void ClearMsg()
        {
            Dispatcher.Invoke((Action)(() =>
            {
                txtMsg.Clear();
            }));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ClearMsg();
        }
    }
}