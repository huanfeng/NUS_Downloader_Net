namespace NUS_Downloader_Net
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_down_titles = new System.Windows.Forms.TextBox();
            this.groupBox_Config = new System.Windows.Forms.GroupBox();
            this.button_clear_log = new System.Windows.Forms.Button();
            this.button_patch_external = new System.Windows.Forms.Button();
            this.button_export_down_list = new System.Windows.Forms.Button();
            this.checkBox_auto_retry = new System.Windows.Forms.CheckBox();
            this.label_title_id = new System.Windows.Forms.Label();
            this.button_download = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_downSpeed = new System.Windows.Forms.Label();
            this.progressBar_total = new System.Windows.Forms.ProgressBar();
            this.progressBar_current = new System.Windows.Forms.ProgressBar();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.tabControl_main = new System.Windows.Forms.TabControl();
            this.tabPage_download = new System.Windows.Forms.TabPage();
            this.tabPage_setting = new System.Windows.Forms.TabPage();
            this.tabPage_help = new System.Windows.Forms.TabPage();
            this.textBox_readme = new System.Windows.Forms.TextBox();
            this.groupBox_Config.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl_main.SuspendLayout();
            this.tabPage_download.SuspendLayout();
            this.tabPage_help.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_down_titles
            // 
            this.textBox_down_titles.Location = new System.Drawing.Point(6, 38);
            this.textBox_down_titles.Multiline = true;
            this.textBox_down_titles.Name = "textBox_down_titles";
            this.textBox_down_titles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_down_titles.Size = new System.Drawing.Size(366, 196);
            this.textBox_down_titles.TabIndex = 0;
            // 
            // groupBox_Config
            // 
            this.groupBox_Config.Controls.Add(this.button_clear_log);
            this.groupBox_Config.Controls.Add(this.button_patch_external);
            this.groupBox_Config.Controls.Add(this.button_export_down_list);
            this.groupBox_Config.Controls.Add(this.checkBox_auto_retry);
            this.groupBox_Config.Controls.Add(this.label_title_id);
            this.groupBox_Config.Controls.Add(this.button_download);
            this.groupBox_Config.Controls.Add(this.textBox_down_titles);
            this.groupBox_Config.Location = new System.Drawing.Point(10, 6);
            this.groupBox_Config.Name = "groupBox_Config";
            this.groupBox_Config.Size = new System.Drawing.Size(587, 240);
            this.groupBox_Config.TabIndex = 1;
            this.groupBox_Config.TabStop = false;
            this.groupBox_Config.Text = "Config";
            // 
            // button_clear_log
            // 
            this.button_clear_log.Location = new System.Drawing.Point(387, 211);
            this.button_clear_log.Name = "button_clear_log";
            this.button_clear_log.Size = new System.Drawing.Size(134, 23);
            this.button_clear_log.TabIndex = 6;
            this.button_clear_log.Text = "Clear Log";
            this.button_clear_log.UseVisualStyleBackColor = true;
            this.button_clear_log.Click += new System.EventHandler(this.button_clear_log_Click);
            // 
            // button_patch_external
            // 
            this.button_patch_external.Location = new System.Drawing.Point(387, 151);
            this.button_patch_external.Name = "button_patch_external";
            this.button_patch_external.Size = new System.Drawing.Size(134, 42);
            this.button_patch_external.TabIndex = 5;
            this.button_patch_external.Text = "Patch external download";
            this.button_patch_external.UseVisualStyleBackColor = true;
            this.button_patch_external.Click += new System.EventHandler(this.button_patch_external_Click);
            // 
            // button_export_down_list
            // 
            this.button_export_down_list.Location = new System.Drawing.Point(387, 93);
            this.button_export_down_list.Name = "button_export_down_list";
            this.button_export_down_list.Size = new System.Drawing.Size(134, 42);
            this.button_export_down_list.TabIndex = 4;
            this.button_export_down_list.Text = "Export download list";
            this.button_export_down_list.UseVisualStyleBackColor = true;
            this.button_export_down_list.Click += new System.EventHandler(this.button_export_down_list_Click);
            // 
            // checkBox_auto_retry
            // 
            this.checkBox_auto_retry.AutoSize = true;
            this.checkBox_auto_retry.Checked = true;
            this.checkBox_auto_retry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_auto_retry.Location = new System.Drawing.Point(387, 71);
            this.checkBox_auto_retry.Name = "checkBox_auto_retry";
            this.checkBox_auto_retry.Size = new System.Drawing.Size(84, 16);
            this.checkBox_auto_retry.TabIndex = 3;
            this.checkBox_auto_retry.Text = "Auto retry";
            this.checkBox_auto_retry.UseVisualStyleBackColor = true;
            // 
            // label_title_id
            // 
            this.label_title_id.AutoSize = true;
            this.label_title_id.Location = new System.Drawing.Point(7, 20);
            this.label_title_id.Name = "label_title_id";
            this.label_title_id.Size = new System.Drawing.Size(53, 12);
            this.label_title_id.TabIndex = 2;
            this.label_title_id.Text = "Title id";
            // 
            // button_download
            // 
            this.button_download.Location = new System.Drawing.Point(387, 36);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(134, 23);
            this.button_download.TabIndex = 1;
            this.button_download.Text = "Download";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.button_download_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_downSpeed);
            this.groupBox2.Controls.Add(this.progressBar_total);
            this.groupBox2.Controls.Add(this.progressBar_current);
            this.groupBox2.Controls.Add(this.textBox_log);
            this.groupBox2.Location = new System.Drawing.Point(10, 266);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(587, 206);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log";
            // 
            // label_downSpeed
            // 
            this.label_downSpeed.AutoSize = true;
            this.label_downSpeed.Location = new System.Drawing.Point(512, 33);
            this.label_downSpeed.Name = "label_downSpeed";
            this.label_downSpeed.Size = new System.Drawing.Size(0, 12);
            this.label_downSpeed.TabIndex = 3;
            // 
            // progressBar_total
            // 
            this.progressBar_total.Location = new System.Drawing.Point(7, 42);
            this.progressBar_total.Name = "progressBar_total";
            this.progressBar_total.Size = new System.Drawing.Size(484, 15);
            this.progressBar_total.TabIndex = 2;
            // 
            // progressBar_current
            // 
            this.progressBar_current.Location = new System.Drawing.Point(7, 20);
            this.progressBar_current.Name = "progressBar_current";
            this.progressBar_current.Size = new System.Drawing.Size(484, 16);
            this.progressBar_current.TabIndex = 1;
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(7, 63);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(571, 137);
            this.textBox_log.TabIndex = 0;
            // 
            // tabControl_main
            // 
            this.tabControl_main.Controls.Add(this.tabPage_download);
            this.tabControl_main.Controls.Add(this.tabPage_setting);
            this.tabControl_main.Controls.Add(this.tabPage_help);
            this.tabControl_main.Location = new System.Drawing.Point(12, 12);
            this.tabControl_main.Name = "tabControl_main";
            this.tabControl_main.SelectedIndex = 0;
            this.tabControl_main.Size = new System.Drawing.Size(615, 506);
            this.tabControl_main.TabIndex = 6;
            // 
            // tabPage_download
            // 
            this.tabPage_download.Controls.Add(this.groupBox_Config);
            this.tabPage_download.Controls.Add(this.groupBox2);
            this.tabPage_download.Location = new System.Drawing.Point(4, 22);
            this.tabPage_download.Name = "tabPage_download";
            this.tabPage_download.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_download.Size = new System.Drawing.Size(607, 480);
            this.tabPage_download.TabIndex = 0;
            this.tabPage_download.Text = "Download";
            this.tabPage_download.UseVisualStyleBackColor = true;
            // 
            // tabPage_setting
            // 
            this.tabPage_setting.Location = new System.Drawing.Point(4, 22);
            this.tabPage_setting.Name = "tabPage_setting";
            this.tabPage_setting.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_setting.Size = new System.Drawing.Size(607, 480);
            this.tabPage_setting.TabIndex = 1;
            this.tabPage_setting.Text = "Setting";
            this.tabPage_setting.UseVisualStyleBackColor = true;
            // 
            // tabPage_help
            // 
            this.tabPage_help.Controls.Add(this.textBox_readme);
            this.tabPage_help.Location = new System.Drawing.Point(4, 22);
            this.tabPage_help.Name = "tabPage_help";
            this.tabPage_help.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_help.Size = new System.Drawing.Size(607, 480);
            this.tabPage_help.TabIndex = 2;
            this.tabPage_help.Text = "Readme";
            this.tabPage_help.UseVisualStyleBackColor = true;
            // 
            // textBox_readme
            // 
            this.textBox_readme.Location = new System.Drawing.Point(6, 6);
            this.textBox_readme.Multiline = true;
            this.textBox_readme.Name = "textBox_readme";
            this.textBox_readme.ReadOnly = true;
            this.textBox_readme.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_readme.Size = new System.Drawing.Size(595, 468);
            this.textBox_readme.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 530);
            this.Controls.Add(this.tabControl_main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "NUS Downloader .Net v0.2";
            this.groupBox_Config.ResumeLayout(false);
            this.groupBox_Config.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl_main.ResumeLayout(false);
            this.tabPage_download.ResumeLayout(false);
            this.tabPage_help.ResumeLayout(false);
            this.tabPage_help.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_down_titles;
        private System.Windows.Forms.GroupBox groupBox_Config;
        private System.Windows.Forms.Button button_patch_external;
        private System.Windows.Forms.Button button_export_down_list;
        private System.Windows.Forms.CheckBox checkBox_auto_retry;
        private System.Windows.Forms.Label label_title_id;
        private System.Windows.Forms.Button button_download;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar_total;
        private System.Windows.Forms.ProgressBar progressBar_current;
        private System.Windows.Forms.TextBox textBox_log;
        private System.Windows.Forms.Label label_downSpeed;
        private System.Windows.Forms.TabControl tabControl_main;
        private System.Windows.Forms.TabPage tabPage_download;
        private System.Windows.Forms.TabPage tabPage_setting;
        private System.Windows.Forms.TabPage tabPage_help;
        private System.Windows.Forms.Button button_clear_log;
        private System.Windows.Forms.TextBox textBox_readme;
    }
}

