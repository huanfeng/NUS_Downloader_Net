using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace libWiiSharp
{
    class WiiuNusClient : IDisposable
    {
        private const string WII_NUS_URL = "http://nus.cdn.shop.wii.com/ccs/download/";

        private const string WII_USER_AGENT = "wii libnup/1.0";

        private const string TMD_FILE_NAME = "title.tmd";
        private const string TIK_FILE_NAME = "title.tik";
        private const string CERT_FILE_NAME = "title.cert";

        private byte[] DEF_CERT = {
    0x00, 0x01, 0x00, 0x04, 0x91, 0x9E, 0xBE, 0x46, 0x4A, 0xD0, 0xF5, 0x52,
    0xCD, 0x1B, 0x72, 0xE7, 0x88, 0x49, 0x10, 0xCF, 0x55, 0xA9, 0xF0, 0x2E,
    0x50, 0x78, 0x96, 0x41, 0xD8, 0x96, 0x68, 0x3D, 0xC0, 0x05, 0xBD, 0x0A,
    0xEA, 0x87, 0x07, 0x9D, 0x8A, 0xC2, 0x84, 0xC6, 0x75, 0x06, 0x5F, 0x74,
    0xC8, 0xBF, 0x37, 0xC8, 0x80, 0x44, 0x40, 0x95, 0x02, 0xA0, 0x22, 0x98,
    0x0B, 0xB8, 0xAD, 0x48, 0x38, 0x3F, 0x6D, 0x28, 0xA7, 0x9D, 0xE3, 0x96,
    0x26, 0xCC, 0xB2, 0xB2, 0x2A, 0x0F, 0x19, 0xE4, 0x10, 0x32, 0xF0, 0x94,
    0xB3, 0x9F, 0xF0, 0x13, 0x31, 0x46, 0xDE, 0xC8, 0xF6, 0xC1, 0xA9, 0xD5,
    0x5C, 0xD2, 0x8D, 0x9E, 0x1C, 0x47, 0xB3, 0xD1, 0x1F, 0x4F, 0x54, 0x26,
    0xC2, 0xC7, 0x80, 0x13, 0x5A, 0x27, 0x75, 0xD3, 0xCA, 0x67, 0x9B, 0xC7,
    0xE8, 0x34, 0xF0, 0xE0, 0xFB, 0x58, 0xE6, 0x88, 0x60, 0xA7, 0x13, 0x30,
    0xFC, 0x95, 0x79, 0x17, 0x93, 0xC8, 0xFB, 0xA9, 0x35, 0xA7, 0xA6, 0x90,
    0x8F, 0x22, 0x9D, 0xEE, 0x2A, 0x0C, 0xA6, 0xB9, 0xB2, 0x3B, 0x12, 0xD4,
    0x95, 0xA6, 0xFE, 0x19, 0xD0, 0xD7, 0x26, 0x48, 0x21, 0x68, 0x78, 0x60,
    0x5A, 0x66, 0x53, 0x8D, 0xBF, 0x37, 0x68, 0x99, 0x90, 0x5D, 0x34, 0x45,
    0xFC, 0x5C, 0x72, 0x7A, 0x0E, 0x13, 0xE0, 0xE2, 0xC8, 0x97, 0x1C, 0x9C,
    0xFA, 0x6C, 0x60, 0x67, 0x88, 0x75, 0x73, 0x2A, 0x4E, 0x75, 0x52, 0x3D,
    0x2F, 0x56, 0x2F, 0x12, 0xAA, 0xBD, 0x15, 0x73, 0xBF, 0x06, 0xC9, 0x40,
    0x54, 0xAE, 0xFA, 0x81, 0xA7, 0x14, 0x17, 0xAF, 0x9A, 0x4A, 0x06, 0x6D,
    0x0F, 0xFC, 0x5A, 0xD6, 0x4B, 0xAB, 0x28, 0xB1, 0xFF, 0x60, 0x66, 0x1F,
    0x44, 0x37, 0xD4, 0x9E, 0x1E, 0x0D, 0x94, 0x12, 0xEB, 0x4B, 0xCA, 0xCF,
    0x4C, 0xFD, 0x6A, 0x34, 0x08, 0x84, 0x79, 0x82, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x52, 0x6F, 0x6F, 0x74,
    0x2D, 0x43, 0x41, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x33, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x01, 0x58, 0x53, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30,
    0x30, 0x63, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x13, 0x7A, 0x08, 0x94,
    0xAD, 0x50, 0x5B, 0xB6, 0xC6, 0x7E, 0x2E, 0x5B, 0xDD, 0x6A, 0x3B, 0xEC,
    0x43, 0xD9, 0x10, 0xC7, 0x72, 0xE9, 0xCC, 0x29, 0x0D, 0xA5, 0x85, 0x88,
    0xB7, 0x7D, 0xCC, 0x11, 0x68, 0x0B, 0xB3, 0xE2, 0x9F, 0x4E, 0xAB, 0xBB,
    0x26, 0xE9, 0x8C, 0x26, 0x01, 0x98, 0x5C, 0x04, 0x1B, 0xB1, 0x43, 0x78,
    0xE6, 0x89, 0x18, 0x1A, 0xAD, 0x77, 0x05, 0x68, 0xE9, 0x28, 0xA2, 0xB9,
    0x81, 0x67, 0xEE, 0x3E, 0x10, 0xD0, 0x72, 0xBE, 0xEF, 0x1F, 0xA2, 0x2F,
    0xA2, 0xAA, 0x3E, 0x13, 0xF1, 0x1E, 0x18, 0x36, 0xA9, 0x2A, 0x42, 0x81,
    0xEF, 0x70, 0xAA, 0xF4, 0xE4, 0x62, 0x99, 0x82, 0x21, 0xC6, 0xFB, 0xB9,
    0xBD, 0xD0, 0x17, 0xE6, 0xAC, 0x59, 0x04, 0x94, 0xE9, 0xCE, 0xA9, 0x85,
    0x9C, 0xEB, 0x2D, 0x2A, 0x4C, 0x17, 0x66, 0xF2, 0xC3, 0x39, 0x12, 0xC5,
    0x8F, 0x14, 0xA8, 0x03, 0xE3, 0x6F, 0xCC, 0xDC, 0xCC, 0xDC, 0x13, 0xFD,
    0x7A, 0xE7, 0x7C, 0x7A, 0x78, 0xD9, 0x97, 0xE6, 0xAC, 0xC3, 0x55, 0x57,
    0xE0, 0xD3, 0xE9, 0xEB, 0x64, 0xB4, 0x3C, 0x92, 0xF4, 0xC5, 0x0D, 0x67,
    0xA6, 0x02, 0xDE, 0xB3, 0x91, 0xB0, 0x66, 0x61, 0xCD, 0x32, 0x88, 0x0B,
    0xD6, 0x49, 0x12, 0xAF, 0x1C, 0xBC, 0xB7, 0x16, 0x2A, 0x06, 0xF0, 0x25,
    0x65, 0xD3, 0xB0, 0xEC, 0xE4, 0xFC, 0xEC, 0xDD, 0xAE, 0x8A, 0x49, 0x34,
    0xDB, 0x8E, 0xE6, 0x7F, 0x30, 0x17, 0x98, 0x62, 0x21, 0x15, 0x5D, 0x13,
    0x1C, 0x6C, 0x3F, 0x09, 0xAB, 0x19, 0x45, 0xC2, 0x06, 0xAC, 0x70, 0xC9,
    0x42, 0xB3, 0x6F, 0x49, 0xA1, 0x18, 0x3B, 0xCD, 0x78, 0xB6, 0xE4, 0xB4,
    0x7C, 0x6C, 0x5C, 0xAC, 0x0F, 0x8D, 0x62, 0xF8, 0x97, 0xC6, 0x95, 0x3D,
    0xD1, 0x2F, 0x28, 0xB7, 0x0C, 0x5B, 0x7D, 0xF7, 0x51, 0x81, 0x9A, 0x98,
    0x34, 0x65, 0x26, 0x25, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
};


        private string mCheckOnlineUrl = "www.baidu.com";

        private string mNusUrl = WII_NUS_URL;
        private WebClient mWcNus;
        private bool mUseLocalFiles = false;
        private bool mContinueWithoutTicket = true;

        private string mTitleId;
        private string mTitleVersion;

        private TMD mTmd;
        private Ticket mTicket;

        private string mOutputDirBase;
        private string mOutputDir;

        public WiiuNusClient(string titleId, string titleVersion, string outputDir)
        {
            mTitleId = titleId;
            mTitleVersion = titleVersion;
            mOutputDirBase = outputDir;
            mOutputDir = Path.Combine(outputDir, titleId + (string.IsNullOrEmpty(titleVersion) ? string.Empty : string.Format("_v{0}", titleVersion)));
        }

        /// <summary>
        /// If true, existing local files will be used.
        /// </summary>
        public bool UseLocalFiles { get { return mUseLocalFiles; } set { mUseLocalFiles = value; } }

        public string CheckOnlineUrl { get { return mCheckOnlineUrl; } set { mCheckOnlineUrl = value; } }

        #region IDisposable Members
        private bool isDisposed = false;

        ~WiiuNusClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed && mWcNus != null)
            {
                mWcNus.Dispose();
            }

            isDisposed = true;
        }
        #endregion

        public void ConfigureNusClient(WebClient wcReady)
        {
            mWcNus = wcReady;
        }

        public void SetToWiiServer()
        {
            mNusUrl = WII_NUS_URL;
            if (mWcNus != null)
            {
                mWcNus.Headers.Add("User-Agent", WII_USER_AGENT);
                mWcNus.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcNus_DownloadProgressChanged);
                mWcNus.DownloadFileCompleted += new AsyncCompletedEventHandler(wcNus_DownloadFileCompleted);
            }
        }

        public void CheckTitle(string titleId)
        {
            if (titleId.Length != 16) throw new Exception("Title ID must be 16 characters long!");
        }

        public static long getFileSize(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            return fi.Length;
        }

        public static string ConvertUnit(long size, string separateUnit = " ")
        {
            string FileSize = string.Empty;
            if (size > (1024 * 1024 * 1024))
                FileSize = string.Format("{0}" + separateUnit + "GB", ((double)size / (1024 * 1024 * 1024)).ToString(".##"));
            else if (size > (1024 * 1024))
                FileSize = string.Format("{0}" + separateUnit + "MB", ((double)size / (1024 * 1024)).ToString(".##"));
            else if (size > 1024)
                FileSize = string.Format("{0}" + separateUnit + "KB", ((double)size / 1024).ToString(".##"));
            else if (size == 0)
                FileSize = "0 Byte";
            else
                FileSize = string.Format("{0}" + separateUnit + "B", ((double)size / 1).ToString(".##"));

            return FileSize;
        }

        public string getTmdUrl()
        {
            string tmdFile = "tmd" + (string.IsNullOrEmpty(mTitleVersion) ? string.Empty : string.Format(".{0}", mTitleVersion));
            return string.Format("{0}{1}/{2}", mNusUrl, mTitleId, tmdFile);
        }

        public string getTicketUrl()
        {
            return string.Format("{0}{1}/{2}", mNusUrl, mTitleId, "cetk");
        }

        public string getContentUrl(string contentId)
        {
            return string.Format("{0}{1}/{2}", mNusUrl, mTitleId, contentId);
        }

        const int BUFF_SIZE = 512 * 1024;

        private void downloadFileAndWait(string url, string filePath)
        {
            Stream webStream = null;
            FileStream fileStream = null;
            try
            {
                webStream = mWcNus.OpenRead(url);

                fileStream = new FileStream(filePath, FileMode.Create);

                byte[] buff = new byte[BUFF_SIZE];
                long bytesTotal = Convert.ToInt64(mWcNus.ResponseHeaders["Content-Length"]);
                long bytesReaded = 0;
                long remain = bytesTotal;
                while (true)
                {
                    if (!cancelDownload)
                    {
                        int read = webStream.Read(buff, 0, buff.Length);
                        if (read > 0)
                        {
                            bytesReaded += read;
                            fileStream.Write(buff, 0, read);
                            fireCurrProgress(bytesReaded, bytesTotal);
                        }
                        else
                        {
                            // Download over
                            break;
                        }
                    }
                    else
                    {
                        // Download canceled
                        break;
                    }
                }

            
            } finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream = null;
                }
                if (webStream != null)
                {
                    webStream.Close();
                    webStream.Dispose();
                    webStream = null;
                }
            }

            if (cancelDownload)
            {
                throw new OperationCanceledException();
            }
        }

        private void downloadContent(TMD_Content content)
        {
            string contentId = content.ContentID.ToString("x8");
            //fireDebug("Downloading Content ID: {0} ...", contentId);

            if (!CheckInet()) { fireDebug("   Connection not found..."); throw new Exception("You're not connected to the internet!"); }

            string url = getContentUrl(contentId);
            string filePath = Path.Combine(mOutputDir, contentId + ".app");

            FileInfo fi = new FileInfo(filePath);
            if (mUseLocalFiles && fi.Exists && fi.Length == (long)content.Size)
            {
                fireDebug("ig app! ", contentId);
            }
            else
            {
                downloadFileAndWait(url, filePath);
            }

            if (((ushort)content.Type & 0x02) == 0x02) // Need download h3
            {
                string h3url = string.Format("{0}{1}/{2}", mNusUrl, mTitleId, contentId + ".h3");
                string h3Path = Path.Combine(mOutputDir, contentId + ".h3");
                FileInfo h3fi = new FileInfo(h3Path);

                if (mUseLocalFiles && h3fi.Exists && (h3fi.Length == 20 || h3fi.Length == 40))
                {
                    fireDebug("ig h3! ", contentId);
                }
                else
                {
                    mWcNus.DownloadFile(h3url, h3Path);
                }
            }

            //fireDebug("Downloading Content {0} Finished...", contentId);
        }

        private string[] getContentDownloadUrls(TMD_Content content)
        {
            string contentId = content.ContentID.ToString("x8");

            string url = getContentUrl(contentId);

            if (((ushort)content.Type & 0x02) == 0x02) // Need download h3
            {
                string h3url = getContentUrl(contentId + ".h3");
                string h3Path = Path.Combine(mOutputDir, contentId + ".h3");

                return new string[] { url, h3url };

            }
            return new string[] { url };
        }

        private byte[] downloadSingleContentAndDecrypt(string titleId, string titleVersion, string contentId, string outputFile)
        {
            uint cId = uint.Parse(contentId, System.Globalization.NumberStyles.HexNumber);
            contentId = cId.ToString("x8");

            fireDebug("Downloading Content (Content ID: {0}) of Title {1} v{2}...", contentId, titleId, (string.IsNullOrEmpty(titleVersion)) ? "[Latest]" : titleVersion);

            if (!CheckInet()) { fireDebug("   Connection not found..."); throw new Exception("You're not connected to the internet!"); }

            string tmdFile = "tmd" + (string.IsNullOrEmpty(titleVersion) ? string.Empty : string.Format(".{0}", titleVersion));
            string titleUrl = string.Format("{0}{1}/", mNusUrl, titleId);
            string contentIdString = string.Empty;
            int cIndex = 0;

            //Download TMD
            fireDebug("   Downloading TMD...");
            byte[] tmdArray = mWcNus.DownloadData(titleUrl + tmdFile);
            fireDebug("   Parsing TMD...");
            TMD tmd = TMD.Load(tmdArray);

            //Search for Content ID in TMD
            fireDebug("   Looking for Content ID {0} in TMD...", contentId);
            bool foundContentId = false;
            for (int i = 0; i < tmd.Contents.Length; i++)
                if (tmd.Contents[i].ContentID == cId)
                {
                    fireDebug("   Content ID {0} found in TMD...", contentId);
                    foundContentId = true;
                    contentIdString = tmd.Contents[i].ContentID.ToString("x8");
                    cIndex = i;
                    break;
                }

            if (!foundContentId) { fireDebug("   Content ID {0} wasn't found in TMD...", contentId); throw new Exception("Content ID wasn't found in the TMD!"); }

            //Download Ticket
            fireDebug("   Downloading Ticket...");
            byte[] tikArray = mWcNus.DownloadData(titleUrl + "cetk");
            fireDebug("   Parsing Ticket...");
            Ticket tik = Ticket.Load(tikArray);

            //Download and Decrypt Content
            fireDebug("   Downloading Content... ({0} bytes)", tmd.Contents[cIndex].Size);
            byte[] encryptedContent = mWcNus.DownloadData(titleUrl + contentIdString);

            fireDebug("   Decrypting Content...");
            byte[] decryptedContent = decryptContent(encryptedContent, cIndex, tik, tmd);
            Array.Resize(ref decryptedContent, (int)tmd.Contents[cIndex].Size);

            //Check SHA1
            SHA1 s = SHA1.Create();
            byte[] newSha = s.ComputeHash(decryptedContent);

            if (!Shared.CompareByteArrays(newSha, tmd.Contents[cIndex].Hash)) { fireDebug(@"/!\ /!\ /!\ Hashes do not match /!\ /!\ /!\"); throw new Exception("Hashes do not match!"); }

            fireDebug("Downloading Content (Content ID: {0}) of Title {1} v{2} Finished...", contentId, titleId, (string.IsNullOrEmpty(titleVersion)) ? "[Latest]" : titleVersion);
            return decryptedContent;
        }

        public Ticket downloadTicket()
        {
            if (!CheckInet()) throw new Exception("You're not connected to the internet!");

            string url = getTicketUrl();
            string filePath = Path.Combine(mOutputDir, TIK_FILE_NAME);

            FileInfo fi = new FileInfo(filePath);

            if (mUseLocalFiles && fi.Exists && fi.Length > 0)
            {
                // has file
            }
            else
            {
                mWcNus.DownloadFile(url, filePath);
            }

            mTicket = Ticket.Load(filePath);

            return mTicket;
        }

        private TMD downloadTmd()
        {
            if (!CheckInet()) throw new Exception("You're not connected to the internet!");

            string titleUrl = getTmdUrl();
            string filePath = Path.Combine(mOutputDir, TMD_FILE_NAME);


            FileInfo fi = new FileInfo(filePath);

            if (mUseLocalFiles && fi.Exists && fi.Length > 0)
            {
                // has file
            }
            else
            {
                mWcNus.DownloadFile(titleUrl, filePath);
            }

            mTmd = TMD.Load(filePath);

            return mTmd;
        }

        private TMD downloadTmdToMemory()
        {
            if (!CheckInet()) throw new Exception("You're not connected to the internet!");

            string titleUrl = getTmdUrl();

            byte[] data = mWcNus.DownloadData(titleUrl);

            mTmd = TMD.Load(data);

            return mTmd;
        }

        private void createCert()
        {
            try
            {
                string filePath = Path.Combine(mOutputDir, CERT_FILE_NAME);
                FileStream fos = new FileStream(filePath, FileMode.OpenOrCreate);
                fos.Write(mTmd.cert1, 0, mTmd.cert1.Length);
                fos.Write(mTmd.cert2, 0, mTmd.cert2.Length);
                fos.Write(DEF_CERT, 0, DEF_CERT.Length);
                fos.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public void downloadTitle()
        {
            //fireDebug("Downloading Title {0} {1}...", mTitleId, mTitleVersion);

            // Create dirs
            if (!Directory.Exists(mOutputDir)) Directory.CreateDirectory(mOutputDir);

            //Download TMD
            fireDebug("  - TMD... ");

            TMD tmd = downloadTmd();

            //fireDebug(" {0} Contents.\r\n", tmd.NumOfContents);

            mBytesTotal = 0;
            for (int i = 0; i < tmd.NumOfContents; i++)
            {
                mBytesTotal += (long)tmd.Contents[i].Size;
            }
            fireDebug("  Contents:" + tmd.NumOfContents + ", Size:" + ConvertUnit(mBytesTotal) + "\r\n");

            fireDebug("  - Ticket...");
            //Download cetk
            try
            {
                downloadTicket();
                fireDebug(" OK! \r\n");
            }
            catch (Exception ex)
            {
                fireDebug(" Fail! \r\n");
                if (!mContinueWithoutTicket)
                {
                    fireDebug("   + Downloading Ticket Failed...");
                    throw new Exception("Downloading Ticket Failed:\n" + ex.Message);
                }
            }

            string[] encryptedContents = new string[tmd.NumOfContents];

            mBytesTotalDone = 0;

            //Download Content
            for (int i = 0; i < tmd.NumOfContents; i++)
            {
                TMD_Content content = tmd.Contents[i];

                fireDebug("  - Content:[{0}/{1}], ID:{2}, Size:{3} ...", i + 1, tmd.NumOfContents, content.ContentID.ToString("x8"), ConvertUnit((long)content.Size));
                fireTotalProgress(mBytesTotalDone, mBytesTotal);

                downloadContent(content);
                if (!cancelDownload)
                {
                    mBytesTotalDone += (long)tmd.Contents[i].Size;
                    fireTotalProgress(mBytesTotalDone, mBytesTotal);
                    fireDebug(" OK! \r\n");
                }
                else
                {
                    fireDebug("Canceled!\r\n");
                    throw new OperationCanceledException();
                }
            }

            // Create cert file
            createCert();
        }


        public void StopDownload()
        {
            if (mWcNus != null)
            {

                mWcNus.CancelAsync();
            }
        }

        private bool isDownloading;
        public bool cancelDownload;
        private long mBytesTotalDone = 0;
        private long mBytesTotal = 0;

        private void wcNus_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            long downSize = e.BytesReceived;

            fireTotalProgress(downSize + mBytesTotalDone, mBytesTotal);
            fireCurrProgress(e.BytesReceived, e.TotalBytesToReceive);
        }

        private void wcNus_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            isDownloading = false;
        }

        private byte[] decryptContent(byte[] content, int contentIndex, Ticket tik, TMD tmd)
        {
            Array.Resize(ref content, Shared.AddPadding(content.Length, 16));
            byte[] titleKey = tik.TitleKey;
            byte[] iv = new byte[16];

            byte[] tmp = BitConverter.GetBytes(tmd.Contents[contentIndex].Index);
            iv[0] = tmp[1];
            iv[1] = tmp[0];

            RijndaelManaged rm = new RijndaelManaged();
            rm.Mode = CipherMode.CBC;
            rm.Padding = PaddingMode.None;
            rm.KeySize = 128;
            rm.BlockSize = 128;
            rm.Key = titleKey;
            rm.IV = iv;

            ICryptoTransform decryptor = rm.CreateDecryptor();

            MemoryStream ms = new MemoryStream(content);
            CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

            byte[] decCont = new byte[content.Length];
            cs.Read(decCont, 0, decCont.Length);

            cs.Dispose();
            ms.Dispose();

            return decCont;
        }

        private bool CheckInet()
        {
            try
            {
                System.Net.IPHostEntry ipHost = System.Net.Dns.GetHostEntry(mCheckOnlineUrl);
                return true;
            }
            catch { return false; }
        }

        public void exportTitle()
        {
            // Create dirs
            if (!Directory.Exists(mOutputDirBase)) Directory.CreateDirectory(mOutputDirBase);

            //Download TMD
            fireDebug("  - TMD... ");

            TMD tmd = downloadTmdToMemory();

            //fireDebug(" {0} Contents.\r\n", tmd.NumOfContents);

            mBytesTotal = 0;
            for (int i = 0; i < tmd.NumOfContents; i++)
            {
                mBytesTotal += (long)tmd.Contents[i].Size;
            }
            fireDebug("  Contents:" + tmd.NumOfContents + ", Size:" + ConvertUnit(mBytesTotal) + "\r\n");

            string[] encryptedContents = new string[tmd.NumOfContents];

            mBytesTotalDone = 0;

            var listFilePath = Path.Combine(mOutputDirBase, mTitleId + "_LIST_" + ConvertUnit(mBytesTotal, "") + ".txt");

            StringWriter sw = new StringWriter();

            sw.WriteLine(getTmdUrl());

            //Export Content
            for (int i = 0; i < tmd.NumOfContents; i++)
            {
                TMD_Content content = tmd.Contents[i];

                var list = getContentDownloadUrls(content);
                foreach (var j in list)
                {
                    sw.WriteLine(j);
                }
            }

            File.WriteAllText(listFilePath, sw.ToString());
        }

        #region Events
        public class ProgressEventArgs : EventArgs
        {
            public long curr;
            public long total;
            public ProgressEventArgs(long curr, long total) { this.curr = curr; this.total = total; }
        }


        public event EventHandler<ProgressEventArgs> CurrProgress;
        public event EventHandler<ProgressEventArgs> TotalProgress;


        public event EventHandler<MessageEventArgs> Debug;

        private void fireDebug(string debugMessage, params object[] args)
        {
            EventHandler<MessageEventArgs> debug = Debug;
            if (debug != null)
                debug(new object(), new MessageEventArgs(string.Format(debugMessage, args)));
        }

        private void fireCurrProgress(long curr, long total)
        {
            EventHandler<ProgressEventArgs> progress = CurrProgress;
            if (progress != null)
                progress(new object(), new ProgressEventArgs(curr, total));
        }
        private void fireTotalProgress(long curr, long total)
        {
            EventHandler<ProgressEventArgs> progress = TotalProgress;
            if (progress != null)
                progress(new object(), new ProgressEventArgs(curr, total));
        }

        private void fireProgress(int curr)
        {
            //EventHandler<ProgressChangedEventArgs> progress = Progress;
            //if (progress != null)
            //    progress(new object(), new ProgressChangedEventArgs(curr, string.Empty));
        }
        #endregion

    }
}
