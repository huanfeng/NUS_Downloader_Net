using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NUS_Downloader_Net
{
    public partial class Form1 : Form
    {

        bool mDownloading = false;
        BackgroundWorker mNusDownload = new BackgroundWorker();

        libWiiSharp.WiiuNusClient mNusClient;

        System.Threading.Timer mTimer;

        bool mExporting = false;
        BackgroundWorker mNusExport = new BackgroundWorker();

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            textBox_readme.Text = global::NUS_Downloader_Net.Properties.Resources.text_readme;

            progressBar_current.Maximum = 1000;
            progressBar_total.Maximum = 1000;

            mNusDownload.DoWork += MNusDownload_DoWork;
            mNusDownload.RunWorkerCompleted += MNusDownload_RunWorkerCompleted;

            mNusExport.DoWork += MNusExport_DoWork;
            mNusExport.RunWorkerCompleted += MNusExport_RunWorkerCompleted;

#if DEBUG
            textBox_down_titles.Text = @"000500001f600a00
0005000010101d00
0005000010142C00
0005000010162D00
0005000010168700";
#endif
        }
        void nusClient_Debug(object sender, libWiiSharp.MessageEventArgs e)
        {
            ShowLogNoWrap(e.Message);
        }
        private void MNusDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mTimer = new System.Threading.Timer(MTimer_Tick, null, 0, 1000);


                mNusClient = null;
                ShowLog("Start download list...");
                List<string> successList = new List<string>();
                List<string> todoList = parseTitleList();

                int total = todoList.Count;
                if (todoList != null && todoList.Count > 0)
                {
                    for (int i = 0; i < total; i++)
                    {
                        var title = todoList[i];
                        if (mNusClient == null || !mNusClient.cancelDownload)
                        {
                            if (successList.Contains(title))
                            {
                                ShowLog("Already download " + i + ", ignore it.");
                            }
                            else
                            {
                                while (true)
                                {
                                    try
                                    {
                                        DownloadOneTitle(title, "", i, total);
                                        successList.Add(title);
                                        break;
                                    }
                                    catch (OperationCanceledException ex)
                                    {
                                        ShowLog("\r\nDownload canceled!");
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowLog("\r\nDownload fail: \"" + ex.Message + "\"");
                                        if (!checkBox_auto_retry.Checked || mNusClient.cancelDownload)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    ShowLog("Title list is empty!");
                }
            }
            catch (Exception ex)
            {
                ShowLog("Download fail: \"" + ex.Message + "\"");
            }
        }

        private void MNusDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowLog("Download completed...");
            mTimer.Dispose();
            mLastDownSize = 0;
            button_download.Enabled = true;
            setDownloading(false);
        }

        long mLastDownSize = 0;
        private void MTimer_Tick(object state)
        {
            long now = DateTime.Now.Ticks;

            if (mLastDownSize == 0)
            {
                mLastDownSize = mCurrDownSize;
            }
            else
            {
                long downSize = mCurrDownSize - mLastDownSize;
                mLastDownSize = mCurrDownSize;
                long speed = downSize;
                string text = libWiiSharp.WiiuNusClient.ConvertUnit(speed);
                label_downSpeed.Text = text + "/s";
            }
        }

        public class TimeoutWebClient : WebClient
        {
            public int Timeout { get; set; }

            public TimeoutWebClient(int timeout)
            {
                Timeout = timeout;
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.Timeout = Timeout;
                request.ReadWriteTimeout = Timeout;
                return request;
            }
        }

        private void DownloadOneTitle(string titleId, string version, int curr, int total)
        {
            titleId = titleId.ToUpper();

            mLastDownSize = 0;
            string outputDir = Path.Combine(Environment.CurrentDirectory, "NusData");

            WebClient nusWC = new TimeoutWebClient(60 * 1000);

            // Create\Configure NusClient
            mNusClient = new libWiiSharp.WiiuNusClient(titleId, version, outputDir);
            mNusClient.ConfigureNusClient(nusWC);
            mNusClient.UseLocalFiles = true;

            mNusClient.SetToWiiServer();

            // Events
            mNusClient.Debug += nusClient_Debug;
            mNusClient.CurrProgress += NusClient_CurrProgress;
            mNusClient.TotalProgress += NusClient_TotalProgress;

            mNusClient.cancelDownload = false;

            ShowLog("");
            ShowLog(string.Format("  Download [{0}/{1}] - TitleId:{2} Start...", curr + 1, total, titleId));
            mNusClient.downloadTitle();
            ShowLog(string.Format("  Download [{0}/{1}] - TitleId:{2} Finish...", curr + 1, total, titleId));
        }

        private void NusClient_TotalProgress(object sender, libWiiSharp.WiiuNusClient.ProgressEventArgs e)
        {
            progressBar_total.Value = (int)(progressBar_total.Maximum * e.curr / e.total);
        }

        long mCurrDownSize = 0;

        private void NusClient_CurrProgress(object sender, libWiiSharp.WiiuNusClient.ProgressEventArgs e)
        {
            mCurrDownSize = e.curr;
            progressBar_current.Value = (int)(progressBar_current.Maximum * e.curr / e.total);
        }

        private List<string> parseTitleList()
        {
            var list = new List<string>();
            var lines = textBox_down_titles.Text;
            System.IO.StringReader sr = new System.IO.StringReader(lines);

            string tmp = null;
            do
            {
                tmp = sr.ReadLine();
                if (tmp != null)
                {
                    tmp = tmp.Trim();
                    tmp = tmp.Replace("-", "");

                    if (tmp.Length == 16)
                    {
                        list.Add(tmp);
                        //ShowLog("Title id:" + tmp);
                    }
                    else if (tmp.Length == 0)
                    {

                    }
                    else
                    {
                        ShowLog("Not valid title id:" + tmp);
                    }
                }
            } while (tmp != null);

            return list;
        }

        private void MNusExport_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ShowLog("Start export list...");
                List<string> successList = new List<string>();
                List<string> todoList = parseTitleList();

                int total = todoList.Count;
                if (todoList != null && todoList.Count > 0)
                {
                    for (int i = 0; i < total; i++)
                    {
                        var title = todoList[i];
                        ExportOneTitle(title, "", i, total);
                    }
                }
                else
                {
                    ShowLog("Title list is empty!");
                }
            }
            catch (Exception ex)
            {
                ShowLog("Download fail: \"" + ex.Message + "\"");
            }
        }

        private void MNusExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowLog("Export completed...");
            setExporting(false);
        }

        private void ExportOneTitle(string titleId, string version, int curr, int total)
        {
            titleId = titleId.ToUpper();

            string outputDir = Path.Combine(Environment.CurrentDirectory, "NusExport");

            WebClient nusWC = new TimeoutWebClient(60 * 1000);

            // Create\Configure NusClient
            mNusClient = new libWiiSharp.WiiuNusClient(titleId, version, outputDir);
            mNusClient.ConfigureNusClient(nusWC);
            mNusClient.UseLocalFiles = true;

            mNusClient.SetToWiiServer();

            // Events
            mNusClient.Debug += nusClient_Debug;

            ShowLog("");
            ShowLog(string.Format("  Export [{0}/{1}] - TitleId:{2} Start...", curr + 1, total, titleId));
            mNusClient.exportTitle();
            ShowLog(string.Format("  Export [{0}/{1}] - TitleId:{2} Finish...", curr + 1, total, titleId));
        }


        private void button_download_Click(object sender, EventArgs e)
        {
            if (!mDownloading)
            { // "Start Download"
                ClearLog();
                setDownloading(true);

                mNusDownload.RunWorkerAsync();
            }
            else
            {
                if (mNusClient != null)
                {
                    mNusClient.cancelDownload = true;
                    button_download.Enabled = false;
                }
            }
        }

        private void setDownloading(bool downloading, bool force = false)
        {
            if (downloading != mDownloading || force)
            {
                if (downloading)
                {
                    button_download.Text = "Stop";
                    progressBar_current.Value = 0;
                    progressBar_total.Value = 0;

                    textBox_down_titles.Enabled = false;
                    button_export_down_list.Enabled = false;
                    button_patch_external.Enabled = false;

                    button_export_down_list.Enabled = false;
                    button_patch_external.Enabled = false;
                }
                else
                {
                    button_download.Text = "Download";

                    textBox_down_titles.Enabled = true;
                    button_export_down_list.Enabled = true;
                    button_patch_external.Enabled = true;
                    label_downSpeed.Text = "";

                    button_export_down_list.Enabled = true;
                    button_patch_external.Enabled = true;
                }
                mDownloading = downloading;
            }
        }

        void setExporting(bool exporting, bool force = false)
        {
            if (exporting != mExporting)
            {
                if (exporting)
                {
                    button_download.Enabled = false;
                    progressBar_current.Value = 0;
                    progressBar_total.Value = 0;

                    textBox_down_titles.Enabled = false;
                    button_export_down_list.Enabled = false;
                    button_patch_external.Enabled = false;

                    button_export_down_list.Enabled = false;
                    button_patch_external.Enabled = false;
                }
                else
                {
                    button_download.Enabled = true;

                    textBox_down_titles.Enabled = true;
                    button_export_down_list.Enabled = true;
                    button_patch_external.Enabled = true;
                    label_downSpeed.Text = "";

                    button_export_down_list.Enabled = true;
                    button_patch_external.Enabled = true;
                }
                mExporting = exporting;
            }
        }

        private void button_export_down_list_Click(object sender, EventArgs e)
        {
            if (!mExporting)
            {
                setExporting(true);
                mNusExport.RunWorkerAsync();
            }
            else
            {
                mNusExport.CancelAsync();
            }
        }
        private void button_patch_external_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please selectd dir:";
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string foldPath = fbd.SelectedPath;

                if (Directory.Exists(foldPath))
                {
                    PatchExternalDownloadDir(foldPath);
                }
            }
        }

        private void RenameFileInDir(string dir, string fileName, string newFileName)
        {
            var oldFile = Path.Combine(dir, fileName);
            var newFile = Path.Combine(dir, newFileName);
            File.Move(oldFile, newFile);
        }
        private void PatchExternalDownloadDir(string foldPath)
        {
            ShowLog("Patch dir [" + foldPath + "] start...");

            ShowLog("Check file count & size...");


            try
            {    // Load tmd
                var tmdFile = Path.Combine(foldPath, "tmd");
                if (!File.Exists(tmdFile))
                {
                    ShowLog("Not found tmd file!");
                    return;
                }

                libWiiSharp.TMD tmd = new libWiiSharp.TMD();
                tmd.LoadFile(tmdFile);

                List<string> renameList = new List<string>();

                // Check content & size
                for (int i = 0; i < tmd.NumOfContents; ++i)
                {
                    libWiiSharp.TMD_Content content = tmd.Contents[i];

                    var contentId = content.ContentID.ToString("x8");

                    // check content size
                    {
                        FileInfo fi = new FileInfo(Path.Combine(foldPath, contentId));
                        if (!fi.Exists)
                        {
                            throw new Exception("Content [" + contentId + "] not exists!");
                        }
                        if (fi.Length != (long)content.Size)
                        {
                            throw new Exception("Content [" + contentId + "] size error!");
                        }
                    }

                    // check h3 size
                    if (((short)content.Type & 0x02) == 0x02)
                    {
                        FileInfo h3fi = new FileInfo(Path.Combine(foldPath, contentId + ".h3"));
                        if (!h3fi.Exists)
                        {
                            throw new Exception("Content [" + contentId + "] h3 not exists!");
                        }
                        if (!(h3fi.Length == 20 || h3fi.Length == 40))
                        {
                            throw new Exception("Content [" + contentId + "] h3 size error!");
                        }
                    }
                    renameList.Add(contentId);
                }

                // Rename

                // tmd
                RenameFileInDir(foldPath, "tmd", "title.tmd");
                foreach (var i in renameList)
                {
                    RenameFileInDir(foldPath, i, i + ".app");
                }

                ShowLog("Patch dir [" + foldPath + "] finish...");
            }
            catch (Exception ex)
            {
                ShowLog("Fail: " + ex.Message);
            }
        }

        private void ShowLog(string msg)
        {
            textBox_log.Text += msg + "\r\n";
            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.SelectionLength = 0;
            textBox_log.ScrollToCaret();
        }

        private void ShowLogNoWrap(string msg)
        {
            textBox_log.Text += msg;
            textBox_log.SelectionStart = textBox_log.TextLength;
            textBox_log.SelectionLength = 0;
            textBox_log.ScrollToCaret();
        }

        private void ClearLog()
        {
            textBox_log.Text = "";
        }

        private void button_clear_log_Click(object sender, EventArgs e)
        {
            ClearLog();
        }
    }
}
