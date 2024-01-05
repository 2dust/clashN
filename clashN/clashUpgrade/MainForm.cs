using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace clashUpgrade
{
    public partial class MainForm : Form
    {
        private readonly string defaultFilename = "clashN.zip_temp";
        private string fileName;

        public MainForm(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
            {
                fileName = string.Join(" ", args);
                fileName = HttpUtility.UrlDecode(fileName);
            }
        }

        private void UpgradeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] existing = Process.GetProcessesByName("clashN");
                foreach (Process p in existing)
                {
                    string path = p.MainModule.FileName;
                    if (path == GetPath("clashN.exe"))
                    {
                        p.Kill();
                        p.WaitForExit(100);
                    }
                }
            }
            catch (Exception ex)
            {
                // Access may be denied without admin right. The user may not be an administrator.
                ShowWarning("Failed to close Frok ClashN (关闭 Frok ClashN 失败)\n" +
                    "Close it manually, or the upgrade may fail (请手动关闭正在运行的 Frok ClashN，否则可能升级失败\n\n" + ex.StackTrace);
            }

            StringBuilder sb = new();
            try
            {
                if (!File.Exists(fileName))
                {
                    if (File.Exists(defaultFilename))
                        fileName = defaultFilename;
                    else
                    {
                        ShowWarning("Upgrade Failed, File Not Exist (升级失败，文件不存在)");
                        return;
                    }
                }

                string thisAppOldFile = Application.ExecutablePath + ".tmp";
                File.Delete(thisAppOldFile);
                string startKey = "clashN/";

                using ZipArchive archive = ZipFile.OpenRead(fileName);
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    try
                    {
                        if (entry.Length == 0)
                            continue;
                        string fullName = entry.FullName;
                        if (fullName.StartsWith(startKey))
                            fullName = fullName[startKey.Length..];
                        if (Application.ExecutablePath.Equals(GetPath(fullName), StringComparison.CurrentCultureIgnoreCase))
                            File.Move(Application.ExecutablePath, thisAppOldFile);

                        string entryOutputPath = GetPath(fullName);

                        FileInfo fileInfo = new(entryOutputPath);
                        fileInfo.Directory.Create();
                        entry.ExtractToFile(entryOutputPath, true);
                    }
                    catch (Exception ex)
                    {
                        sb.Append(ex.StackTrace);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowWarning("Upgrade Failed (升级失败)" + ex.StackTrace);
                return;
            }
            if (sb.Length > 0)
            {
                ShowWarning("Upgrade Failed, Hold Ctrl + C to copy to clipboard\n" +
                    "(升级失败，按住 Ctrl+C 可以复制到剪贴板)" + sb.ToString());
                return;
            }

            Process.Start("clashN.exe");
            MessageBox.Show("Upgrade succussed (升级成功)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }

        private void ExitBtn_Click(object sender, EventArgs e) => Close();

        private static string GetPath(string fileName)
        {
            string startupPath = Application.StartupPath;

            if (string.IsNullOrEmpty(fileName))
                return startupPath;
            else
                return Path.Combine(startupPath, fileName);
        }

        private static void ShowWarning(string message) => MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}