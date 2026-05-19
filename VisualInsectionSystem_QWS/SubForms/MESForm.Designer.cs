namespace VisualInsectionSystem.SubForms
{
    partial class MESForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpConnectionStatus = new System.Windows.Forms.GroupBox();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.grpServerConfig = new System.Windows.Forms.GroupBox();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.btnStopService = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.grpExecutionResult = new System.Windows.Forms.GroupBox();
            this.txtExecutionResult = new System.Windows.Forms.TextBox();
            this.grpImageStorage = new System.Windows.Forms.GroupBox();
            this.lblImageStorage = new System.Windows.Forms.Label();
            this.grpConnectionStatus.SuspendLayout();
            this.grpServerConfig.SuspendLayout();
            this.grpExecutionResult.SuspendLayout();
            this.grpImageStorage.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConnectionStatus
            // 
            this.grpConnectionStatus.Controls.Add(this.lblConnectionStatus);
            this.grpConnectionStatus.Location = new System.Drawing.Point(20, 15);
            this.grpConnectionStatus.Name = "grpConnectionStatus";
            this.grpConnectionStatus.Size = new System.Drawing.Size(400, 60);
            this.grpConnectionStatus.TabIndex = 0;
            this.grpConnectionStatus.TabStop = false;
            this.grpConnectionStatus.Text = "连接状态";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectionStatus.Location = new System.Drawing.Point(20, 25);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(58, 22);
            this.lblConnectionStatus.TabIndex = 0;
            this.lblConnectionStatus.Text = "未连接";
            // 
            // grpServerConfig
            // 
            this.grpServerConfig.Controls.Add(this.txtServerPort);
            this.grpServerConfig.Controls.Add(this.lblPort);
            this.grpServerConfig.Controls.Add(this.txtServerIP);
            this.grpServerConfig.Controls.Add(this.lblServerIP);
            this.grpServerConfig.Location = new System.Drawing.Point(20, 85);
            this.grpServerConfig.Name = "grpServerConfig";
            this.grpServerConfig.Size = new System.Drawing.Size(400, 80);
            this.grpServerConfig.TabIndex = 1;
            this.grpServerConfig.TabStop = false;
            this.grpServerConfig.Text = "服务器配置";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(280, 35);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(80, 21);
            this.txtServerPort.TabIndex = 3;
            this.txtServerPort.Text = "5000";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(240, 38);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(41, 12);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "端口：";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(80, 35);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(140, 21);
            this.txtServerIP.TabIndex = 1;
            this.txtServerIP.Text = "0.0.0.0";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(20, 38);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(65, 12);
            this.lblServerIP.TabIndex = 0;
            this.lblServerIP.Text = "服务器IP：";
            // 
            // btnStopService
            // 
            this.btnStopService.Enabled = false;
            this.btnStopService.Location = new System.Drawing.Point(240, 180);
            this.btnStopService.Name = "btnStopService";
            this.btnStopService.Size = new System.Drawing.Size(100, 30);
            this.btnStopService.TabIndex = 3;
            this.btnStopService.Text = "停止MES服务";
            this.btnStopService.UseVisualStyleBackColor = true;
            this.btnStopService.Click += new System.EventHandler(this.btnStopMES_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.Location = new System.Drawing.Point(80, 180);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(100, 30);
            this.btnStartService.TabIndex = 2;
            this.btnStartService.Text = "启动MES服务";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartMES_Click);
            // 
            // grpExecutionResult
            // 
            this.grpExecutionResult.Controls.Add(this.txtExecutionResult);
            this.grpExecutionResult.Location = new System.Drawing.Point(20, 220);
            this.grpExecutionResult.Name = "grpExecutionResult";
            this.grpExecutionResult.Size = new System.Drawing.Size(400, 150);
            this.grpExecutionResult.TabIndex = 4;
            this.grpExecutionResult.TabStop = false;
            this.grpExecutionResult.Text = "执行结果";
            // 
            // txtExecutionResult
            // 
            this.txtExecutionResult.Location = new System.Drawing.Point(15, 25);
            this.txtExecutionResult.Multiline = true;
            this.txtExecutionResult.Name = "txtExecutionResult";
            this.txtExecutionResult.ReadOnly = true;
            this.txtExecutionResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExecutionResult.Size = new System.Drawing.Size(370, 110);
            this.txtExecutionResult.TabIndex = 0;
            // 
            // grpImageStorage
            // 
            this.grpImageStorage.Controls.Add(this.lblImageStorage);
            this.grpImageStorage.Location = new System.Drawing.Point(20, 380);
            this.grpImageStorage.Name = "grpImageStorage";
            this.grpImageStorage.Size = new System.Drawing.Size(400, 60);
            this.grpImageStorage.TabIndex = 5;
            this.grpImageStorage.TabStop = false;
            this.grpImageStorage.Text = "图片存储";
            // 
            // lblImageStorage
            // 
            this.lblImageStorage.AutoSize = true;
            this.lblImageStorage.Location = new System.Drawing.Point(20, 25);
            this.lblImageStorage.Name = "lblImageStorage";
            this.lblImageStorage.Size = new System.Drawing.Size(41, 12);
            this.lblImageStorage.TabIndex = 0;
            this.lblImageStorage.Text = "无图片";
            // 
            // MESForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(444, 461);
            this.Controls.Add(this.grpImageStorage);
            this.Controls.Add(this.grpExecutionResult);
            this.Controls.Add(this.btnStopService);
            this.Controls.Add(this.btnStartService);
            this.Controls.Add(this.grpServerConfig);
            this.Controls.Add(this.grpConnectionStatus);
            this.Name = "MESForm";
            this.Text = "MES管理";
            this.grpConnectionStatus.ResumeLayout(false);
            this.grpConnectionStatus.PerformLayout();
            this.grpServerConfig.ResumeLayout(false);
            this.grpServerConfig.PerformLayout();
            this.grpExecutionResult.ResumeLayout(false);
            this.grpExecutionResult.PerformLayout();
            this.grpImageStorage.ResumeLayout(false);
            this.grpImageStorage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConnectionStatus;
        private System.Windows.Forms.Label lblConnectionStatus;

        private System.Windows.Forms.GroupBox grpServerConfig;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Button btnStopService;
        private System.Windows.Forms.Button btnStartService;

        private System.Windows.Forms.GroupBox grpExecutionResult;
        private System.Windows.Forms.TextBox txtExecutionResult;
        private System.Windows.Forms.GroupBox grpImageStorage;
        private System.Windows.Forms.Label lblImageStorage;

        private System.Windows.Forms.Button btnStartMES;
        private System.Windows.Forms.Button btnStopMES;
        private System.Windows.Forms.Label lblLastImageName;
        private System.Windows.Forms.Timer timerConnectionCheck;
    }
}