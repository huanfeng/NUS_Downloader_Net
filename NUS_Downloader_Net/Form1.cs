﻿using System;
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

        libWiiSharp.WiiuNusClient nusClient;

        System.Threading.Timer mTimer;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            progressBar_current.Maximum = 1000;
            progressBar_total.Maximum = 1000;

            mNusDownload.DoWork += MNusDownload_DoWork;
            mNusDownload.RunWorkerCompleted += MNusDownload_RunWorkerCompleted;

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
                

                nusClient = null;
                ShowLog("Start download list...");
                List<string> successList = new List<string>();
                List<string> todoList = parseTitleList();

                int total = todoList.Count;
                if (todoList != null && todoList.Count > 0)
                {
                    for (int i = 0; i < total; i++)
                    {
                        var title = todoList[i];
                        if (nusClient == null || !nusClient.cancelDownload)
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
                                        if (!checkBox_auto_retry.Checked || nusClient.cancelDownload)
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

        long mLastDownSize = 0;
        private void MTimer_Tick(object state)
        {
            long now = DateTime.Now.Ticks;

            if (mLastDownSize == 0)
            {
                mLastDownSize = mCurrDownSize;
            } else
            {
                long downSize = mCurrDownSize - mLastDownSize;
                mLastDownSize = mCurrDownSize;
                long speed = downSize;
                string text = libWiiSharp.WiiuNusClient.ConvertUnit(speed);
                label_downSpeed.Text = text + "/s";
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

            string outputDir = Path.Combine(Environment.CurrentDirectory, "NusData");

            WebClient nusWC = new TimeoutWebClient(60 * 1000);

            // Create\Configure NusClient
            nusClient = new libWiiSharp.WiiuNusClient(titleId, version, outputDir);
            nusClient.ConfigureNusClient(nusWC);
            nusClient.UseLocalFiles = true;

            nusClient.SetToWiiServer();

            // Events
            nusClient.Debug += nusClient_Debug;
            nusClient.CurrProgress += NusClient_CurrProgress;
            nusClient.TotalProgress += NusClient_TotalProgress;

            nusClient.cancelDownload = false;

            ShowLog("");
            ShowLog(string.Format("  Download [{0}/{1}] - TitleId:{2} Start...", curr + 1, total, titleId));
            nusClient.downloadTitle();
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
                if (nusClient != null)
                {
                    nusClient.cancelDownload = true;
                    button_download.Enabled = false;
                }
                //setDownloading(false);
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
                }
                else
                {
                    button_download.Text = "Download";

                    textBox_down_titles.Enabled = true;
                    button_export_down_list.Enabled = true;
                    button_patch_external.Enabled = true;
                    label_downSpeed.Text = "";
                }
                mDownloading = downloading;
            }
        }

        private void button_export_down_list_Click(object sender, EventArgs e)
        {

        }
        private void button_patch_external_Click(object sender, EventArgs e)
        {

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


    }
}
