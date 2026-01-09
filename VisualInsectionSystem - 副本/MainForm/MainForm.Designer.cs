using System;
using System.Windows.Forms;

namespace VisualInsectionSystem
{
    partial class MainForm
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
            System.Windows.Forms.PictureBox pictureBoxLogo;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dakaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.相机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通讯ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.调试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报警ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.vmMainViewConfigControl1 = new VMControls.Winform.Release.VmMainViewConfigControl();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonPathLoad = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.buttonStopExecute = new System.Windows.Forms.Button();
            this.buttonExecuteOnce = new System.Windows.Forms.Button();
            this.buttonContinuExecute = new System.Windows.Forms.Button();
            this.groupBoxTimeInterval = new System.Windows.Forms.GroupBox();
            this.textBoxTimeInterval = new System.Windows.Forms.TextBox();
            this.buttonSetTimeInterval = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.vmRenderControl1 = new VMControls.Winform.Release.VmRenderControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.打开toolStripMenu11 = new System.Windows.Forms.ToolStripMenuItem();
            this.另存toolStripMenu12 = new System.Windows.Forms.ToolStripMenuItem();
            this.CamStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.ContStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.DebugStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.WarnStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.LogStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.UserStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.关于toolStripMenu81 = new System.Windows.Forms.ToolStripMenuItem();
            this.文档toolStripMenu82 = new System.Windows.Forms.ToolStripMenuItem();
            this.扩展ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扩展ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTitleBar = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnMinimize = new System.Windows.Forms.Button();
            pictureBoxLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxLogo)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxControl.SuspendLayout();
            this.groupBoxTimeInterval.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelTitleBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = System.Drawing.SystemColors.Window;
            pictureBoxLogo.Enabled = false;
            pictureBoxLogo.Image = global::VisualInsectionSystem.Properties.Resources.AUTOBOX_LOGO;
            pictureBoxLogo.Location = new System.Drawing.Point(0, 0);
            pictureBoxLogo.Margin = new System.Windows.Forms.Padding(1);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new System.Drawing.Size(369, 44);
            pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 70;
            pictureBoxLogo.TabStop = false;
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.Checked = true;
            this.文件ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dakaToolStripMenuItem,
            this.保存ToolStripMenuItem});
            this.文件ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.文件ToolStripMenuItem.Text = "文件";
            this.文件ToolStripMenuItem.Click += new System.EventHandler(this.文件ToolStripMenuItem_Click);
            // 
            // dakaToolStripMenuItem
            // 
            this.dakaToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dakaToolStripMenuItem.Name = "dakaToolStripMenuItem";
            this.dakaToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.dakaToolStripMenuItem.Text = "打开";
            this.dakaToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.保存ToolStripMenuItem.Text = "另存为";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.另存为ToolStripMenuItem_Click);
            // 
            // 相机ToolStripMenuItem
            // 
            this.相机ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.相机ToolStripMenuItem.Name = "相机ToolStripMenuItem";
            this.相机ToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.相机ToolStripMenuItem.Text = " 相机";
            this.相机ToolStripMenuItem.Click += new System.EventHandler(this.相机ToolStripMenuItem_Click);
            // 
            // 通讯ToolStripMenuItem
            // 
            this.通讯ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.通讯ToolStripMenuItem.Name = "通讯ToolStripMenuItem";
            this.通讯ToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.通讯ToolStripMenuItem.Text = " 通讯";
            this.通讯ToolStripMenuItem.Click += new System.EventHandler(this.通讯ToolStripMenuItem_Click);
            // 
            // 调试ToolStripMenuItem
            // 
            this.调试ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.调试ToolStripMenuItem.Name = "调试ToolStripMenuItem";
            this.调试ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.调试ToolStripMenuItem.Text = "调试";
            this.调试ToolStripMenuItem.Click += new System.EventHandler(this.调试ToolStripMenuItem_Click);
            // 
            // 报警ToolStripMenuItem
            // 
            this.报警ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.报警ToolStripMenuItem.Name = "报警ToolStripMenuItem";
            this.报警ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.报警ToolStripMenuItem.Text = "报警";
            this.报警ToolStripMenuItem.Click += new System.EventHandler(this.报警ToolStripMenuItem_Click);
            // 
            // 日志ToolStripMenuItem
            // 
            this.日志ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.日志ToolStripMenuItem.Name = "日志ToolStripMenuItem";
            this.日志ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.日志ToolStripMenuItem.Text = "日志";
            this.日志ToolStripMenuItem.Click += new System.EventHandler(this.日志ToolStripMenuItem_Click);
            // 
            // 用户ToolStripMenuItem
            // 
            this.用户ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.用户ToolStripMenuItem.Name = "用户ToolStripMenuItem";
            this.用户ToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.用户ToolStripMenuItem.Text = " 用户";
            this.用户ToolStripMenuItem.Click += new System.EventHandler(this.用户ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.帮助ToolStripMenuItem.Text = "帮助";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.vmMainViewConfigControl1);
            this.groupBox4.Location = new System.Drawing.Point(336, 50);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(918, 566);
            this.groupBox4.TabIndex = 63;
            this.groupBox4.TabStop = false;
            // 
            // vmMainViewConfigControl1
            // 
            this.vmMainViewConfigControl1.BackColor = System.Drawing.Color.Black;
            this.vmMainViewConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmMainViewConfigControl1.Location = new System.Drawing.Point(3, 17);
            this.vmMainViewConfigControl1.Margin = new System.Windows.Forms.Padding(0);
            this.vmMainViewConfigControl1.Name = "vmMainViewConfigControl1";
            this.vmMainViewConfigControl1.Size = new System.Drawing.Size(912, 546);
            this.vmMainViewConfigControl1.TabIndex = 0;
// TODO: “”的代码生成失败，原因是出现异常“无效的基元类型: System.IntPtr。请考虑使用 CodeObjectCreateExpression。”。
            this.vmMainViewConfigControl1.Load += new System.EventHandler(this.vmMainViewConfigControl1_Load);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(0, 403);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox3.Size = new System.Drawing.Size(335, 213);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "消息";
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 14);
            this.listBox1.Margin = new System.Windows.Forms.Padding(0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(335, 199);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.buttonPathLoad);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 50);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(335, 235);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // buttonPathLoad
            // 
            this.buttonPathLoad.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.buttonPathLoad.ForeColor = System.Drawing.SystemColors.WindowText;
            this.buttonPathLoad.Location = new System.Drawing.Point(18, 151);
            this.buttonPathLoad.Name = "buttonPathLoad";
            this.buttonPathLoad.Size = new System.Drawing.Size(75, 30);
            this.buttonPathLoad.TabIndex = 29;
            this.buttonPathLoad.Text = "流程";
            this.buttonPathLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPathLoad.UseVisualStyleBackColor = false;
            this.buttonPathLoad.Click += new System.EventHandler(this.buttonPathLoad_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(99, 157);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(211, 20);
            this.comboBox1.TabIndex = 28;
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(99, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(211, 21);
            this.textBox1.TabIndex = 21;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button8.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button8.Location = new System.Drawing.Point(180, 194);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 30);
            this.button8.TabIndex = 27;
            this.button8.Text = "删除";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button5.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button5.Location = new System.Drawing.Point(99, 115);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 30);
            this.button5.TabIndex = 24;
            this.button5.Text = "解锁工作区";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button6.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button6.Location = new System.Drawing.Point(18, 194);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 30);
            this.button6.TabIndex = 25;
            this.button6.Text = "导入";
            this.button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 21);
            this.label1.TabIndex = 20;
            this.label1.Text = "路径";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button7.Location = new System.Drawing.Point(99, 194);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 30);
            this.button7.TabIndex = 26;
            this.button7.Text = "导出";
            this.button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button3.Location = new System.Drawing.Point(18, 79);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 30);
            this.button3.TabIndex = 18;
            this.button3.Text = " 检查密码";
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button2.Location = new System.Drawing.Point(99, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 30);
            this.button2.TabIndex = 17;
            this.button2.Text = "保存";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button1.Location = new System.Drawing.Point(18, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 30);
            this.button1.TabIndex = 16;
            this.button1.Text = "加载";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.button4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.button4.Location = new System.Drawing.Point(18, 115);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 30);
            this.button4.TabIndex = 19;
            this.button4.Text = "锁定工作区";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.Location = new System.Drawing.Point(99, 85);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(211, 21);
            this.textBox2.TabIndex = 22;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.groupBoxControl);
            this.groupBox2.Controls.Add(this.groupBoxTimeInterval);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 285);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(335, 118);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "手动";
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Controls.Add(this.buttonStopExecute);
            this.groupBoxControl.Controls.Add(this.buttonExecuteOnce);
            this.groupBoxControl.Controls.Add(this.buttonContinuExecute);
            this.groupBoxControl.Location = new System.Drawing.Point(6, 19);
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.Size = new System.Drawing.Size(189, 85);
            this.groupBoxControl.TabIndex = 36;
            this.groupBoxControl.TabStop = false;
            this.groupBoxControl.Text = "单次执行 连续执行 停止执行";
            // 
            // buttonStopExecute
            // 
            this.buttonStopExecute.Image = ((System.Drawing.Image)(resources.GetObject("buttonStopExecute.Image")));
            this.buttonStopExecute.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonStopExecute.Location = new System.Drawing.Point(130, 20);
            this.buttonStopExecute.Name = "buttonStopExecute";
            this.buttonStopExecute.Size = new System.Drawing.Size(38, 36);
            this.buttonStopExecute.TabIndex = 19;
            this.buttonStopExecute.UseVisualStyleBackColor = true;
            this.buttonStopExecute.Click += new System.EventHandler(this.buttonStopExecute_Click);
            // 
            // buttonExecuteOnce
            // 
            this.buttonExecuteOnce.Image = ((System.Drawing.Image)(resources.GetObject("buttonExecuteOnce.Image")));
            this.buttonExecuteOnce.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonExecuteOnce.Location = new System.Drawing.Point(12, 20);
            this.buttonExecuteOnce.Name = "buttonExecuteOnce";
            this.buttonExecuteOnce.Size = new System.Drawing.Size(36, 36);
            this.buttonExecuteOnce.TabIndex = 18;
            this.buttonExecuteOnce.UseVisualStyleBackColor = true;
            this.buttonExecuteOnce.Click += new System.EventHandler(this.buttonExecuteOnce_Click);
            // 
            // buttonContinuExecute
            // 
            this.buttonContinuExecute.Image = ((System.Drawing.Image)(resources.GetObject("buttonContinuExecute.Image")));
            this.buttonContinuExecute.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonContinuExecute.Location = new System.Drawing.Point(72, 20);
            this.buttonContinuExecute.Name = "buttonContinuExecute";
            this.buttonContinuExecute.Size = new System.Drawing.Size(36, 36);
            this.buttonContinuExecute.TabIndex = 20;
            this.buttonContinuExecute.UseVisualStyleBackColor = true;
            this.buttonContinuExecute.Click += new System.EventHandler(this.buttonContinuExecute_Click);
            // 
            // groupBoxTimeInterval
            // 
            this.groupBoxTimeInterval.Controls.Add(this.textBoxTimeInterval);
            this.groupBoxTimeInterval.Controls.Add(this.buttonSetTimeInterval);
            this.groupBoxTimeInterval.Location = new System.Drawing.Point(201, 19);
            this.groupBoxTimeInterval.Name = "groupBoxTimeInterval";
            this.groupBoxTimeInterval.Size = new System.Drawing.Size(128, 88);
            this.groupBoxTimeInterval.TabIndex = 37;
            this.groupBoxTimeInterval.TabStop = false;
            this.groupBoxTimeInterval.Text = " 连续执行时间间隔";
            // 
            // textBoxTimeInterval
            // 
            this.textBoxTimeInterval.Location = new System.Drawing.Point(6, 20);
            this.textBoxTimeInterval.Name = "textBoxTimeInterval";
            this.textBoxTimeInterval.Size = new System.Drawing.Size(110, 21);
            this.textBoxTimeInterval.TabIndex = 35;
            // 
            // buttonSetTimeInterval
            // 
            this.buttonSetTimeInterval.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.buttonSetTimeInterval.ForeColor = System.Drawing.SystemColors.WindowText;
            this.buttonSetTimeInterval.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSetTimeInterval.Location = new System.Drawing.Point(18, 47);
            this.buttonSetTimeInterval.Name = "buttonSetTimeInterval";
            this.buttonSetTimeInterval.Size = new System.Drawing.Size(91, 33);
            this.buttonSetTimeInterval.TabIndex = 31;
            this.buttonSetTimeInterval.Text = "设置时间间隔";
            this.buttonSetTimeInterval.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSetTimeInterval.UseVisualStyleBackColor = false;
            this.buttonSetTimeInterval.Click += new System.EventHandler(this.buttonSetTimeInterval_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox5.Controls.Add(this.vmRenderControl1);
            this.groupBox5.Location = new System.Drawing.Point(1014, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(78, 320);
            this.groupBox5.TabIndex = 64;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            this.groupBox5.Visible = false;
            // 
            // vmRenderControl1
            // 
            this.vmRenderControl1.BackColor = System.Drawing.Color.Black;
            this.vmRenderControl1.CoordinateInfoVisible = true;
            this.vmRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmRenderControl1.ImageSource = null;
            this.vmRenderControl1.Location = new System.Drawing.Point(3, 17);
            this.vmRenderControl1.ModuleSource = null;
            this.vmRenderControl1.Name = "vmRenderControl1";
            this.vmRenderControl1.Size = new System.Drawing.Size(72, 300);
            this.vmRenderControl1.TabIndex = 0;
            this.vmRenderControl1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Location = new System.Drawing.Point(5, 47);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1254, 616);
            this.panel1.TabIndex = 65;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileStripMenuItem1,
            this.CamStripMenuItem2,
            this.ContStripMenuItem3,
            this.DebugStripMenuItem4,
            this.WarnStripMenuItem5,
            this.LogStripMenuItem6,
            this.UserStripMenuItem7,
            this.HelpStripMenuItem8,
            this.扩展ToolStripMenuItem,
            this.扩展ToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(2);
            this.menuStrip1.Size = new System.Drawing.Size(1254, 50);
            this.menuStrip1.TabIndex = 72;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FileStripMenuItem1
            // 
            this.FileStripMenuItem1.Checked = true;
            this.FileStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FileStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开toolStripMenu11,
            this.另存toolStripMenu12});
            this.FileStripMenuItem1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FileStripMenuItem1.Name = "FileStripMenuItem1";
            this.FileStripMenuItem1.Size = new System.Drawing.Size(51, 46);
            this.FileStripMenuItem1.Text = "文件";
            // 
            // 打开toolStripMenu11
            // 
            this.打开toolStripMenu11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.打开toolStripMenu11.Name = "打开toolStripMenu11";
            this.打开toolStripMenu11.Size = new System.Drawing.Size(108, 22);
            this.打开toolStripMenu11.Text = "打开";
            this.打开toolStripMenu11.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 另存toolStripMenu12
            // 
            this.另存toolStripMenu12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.另存toolStripMenu12.Name = "另存toolStripMenu12";
            this.另存toolStripMenu12.Size = new System.Drawing.Size(108, 22);
            this.另存toolStripMenu12.Text = "另存为";
            this.另存toolStripMenu12.Click += new System.EventHandler(this.另存为ToolStripMenuItem_Click);
            // 
            // CamStripMenuItem2
            // 
            this.CamStripMenuItem2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CamStripMenuItem2.Name = "CamStripMenuItem2";
            this.CamStripMenuItem2.Size = new System.Drawing.Size(59, 46);
            this.CamStripMenuItem2.Text = " 相机";
            this.CamStripMenuItem2.Click += new System.EventHandler(this.相机ToolStripMenuItem_Click);
            // 
            // ContStripMenuItem3
            // 
            this.ContStripMenuItem3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ContStripMenuItem3.Name = "ContStripMenuItem3";
            this.ContStripMenuItem3.Size = new System.Drawing.Size(59, 46);
            this.ContStripMenuItem3.Text = " 通讯";
            this.ContStripMenuItem3.Click += new System.EventHandler(this.通讯ToolStripMenuItem_Click);
            // 
            // DebugStripMenuItem4
            // 
            this.DebugStripMenuItem4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DebugStripMenuItem4.Name = "DebugStripMenuItem4";
            this.DebugStripMenuItem4.Size = new System.Drawing.Size(51, 46);
            this.DebugStripMenuItem4.Text = "调试";
            this.DebugStripMenuItem4.Click += new System.EventHandler(this.调试ToolStripMenuItem_Click);
            // 
            // WarnStripMenuItem5
            // 
            this.WarnStripMenuItem5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WarnStripMenuItem5.Name = "WarnStripMenuItem5";
            this.WarnStripMenuItem5.Size = new System.Drawing.Size(51, 46);
            this.WarnStripMenuItem5.Text = "报警";
            this.WarnStripMenuItem5.Click += new System.EventHandler(this.报警ToolStripMenuItem_Click);
            // 
            // LogStripMenuItem6
            // 
            this.LogStripMenuItem6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LogStripMenuItem6.Name = "LogStripMenuItem6";
            this.LogStripMenuItem6.Size = new System.Drawing.Size(51, 46);
            this.LogStripMenuItem6.Text = "日志";
            this.LogStripMenuItem6.Click += new System.EventHandler(this.日志ToolStripMenuItem_Click);
            // 
            // UserStripMenuItem7
            // 
            this.UserStripMenuItem7.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserStripMenuItem7.Name = "UserStripMenuItem7";
            this.UserStripMenuItem7.Size = new System.Drawing.Size(59, 46);
            this.UserStripMenuItem7.Text = " 用户";
            this.UserStripMenuItem7.Click += new System.EventHandler(this.用户ToolStripMenuItem_Click);
            // 
            // HelpStripMenuItem8
            // 
            this.HelpStripMenuItem8.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于toolStripMenu81,
            this.文档toolStripMenu82});
            this.HelpStripMenuItem8.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HelpStripMenuItem8.Name = "HelpStripMenuItem8";
            this.HelpStripMenuItem8.Size = new System.Drawing.Size(51, 46);
            this.HelpStripMenuItem8.Text = "帮助";
            this.HelpStripMenuItem8.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // 关于toolStripMenu81
            // 
            this.关于toolStripMenu81.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.关于toolStripMenu81.Name = "关于toolStripMenu81";
            this.关于toolStripMenu81.Size = new System.Drawing.Size(106, 22);
            this.关于toolStripMenu81.Text = "关于";
            this.关于toolStripMenu81.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 文档toolStripMenu82
            // 
            this.文档toolStripMenu82.Name = "文档toolStripMenu82";
            this.文档toolStripMenu82.Size = new System.Drawing.Size(106, 22);
            this.文档toolStripMenu82.Text = "文档";
            // 
            // 扩展ToolStripMenuItem
            // 
            this.扩展ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.扩展ToolStripMenuItem.Name = "扩展ToolStripMenuItem";
            this.扩展ToolStripMenuItem.Size = new System.Drawing.Size(51, 46);
            this.扩展ToolStripMenuItem.Text = "扩展";
            this.扩展ToolStripMenuItem.Visible = false;
            // 
            // 扩展ToolStripMenuItem1
            // 
            this.扩展ToolStripMenuItem1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.扩展ToolStripMenuItem1.Name = "扩展ToolStripMenuItem1";
            this.扩展ToolStripMenuItem1.Size = new System.Drawing.Size(51, 46);
            this.扩展ToolStripMenuItem1.Text = "扩展";
            this.扩展ToolStripMenuItem1.Visible = false;
            // 
            // panelTitleBar
            // 
            this.panelTitleBar.BackColor = System.Drawing.SystemColors.Window;
            this.panelTitleBar.Controls.Add(pictureBoxLogo);
            this.panelTitleBar.Controls.Add(this.btnClose);
            this.panelTitleBar.Controls.Add(this.btnMaximize);
            this.panelTitleBar.Controls.Add(this.btnMinimize);
            this.panelTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitleBar.Location = new System.Drawing.Point(2, 2);
            this.panelTitleBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelTitleBar.Name = "panelTitleBar";
            this.panelTitleBar.Size = new System.Drawing.Size(1260, 45);
            this.panelTitleBar.TabIndex = 72;
            this.panelTitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseDown);
            this.panelTitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseMove);
            this.panelTitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTitleBar_MouseUp);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.IndianRed;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1217, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(40, 30);
            this.btnClose.TabIndex = 69;
            this.btnClose.Text = "✕";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(1171, 3);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(40, 30);
            this.btnMaximize.TabIndex = 68;
            this.btnMaximize.Text = "□";
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            this.btnMaximize.MouseLeave += new System.EventHandler(this.btnMaximize_MouseLeave);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(1125, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(40, 30);
            this.btnMinimize.TabIndex = 67;
            this.btnMinimize.TabStop = false;
            this.btnMinimize.Text = "—";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1264, 668);
            this.Controls.Add(this.panelTitleBar);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximumSize = new System.Drawing.Size(1280, 960);
            this.MinimumSize = new System.Drawing.Size(320, 240);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(pictureBoxLogo)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBoxControl.ResumeLayout(false);
            this.groupBoxTimeInterval.ResumeLayout(false);
            this.groupBoxTimeInterval.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelTitleBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dakaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 相机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 通讯ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 调试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 报警ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private GroupBox groupBox4;
        private VMControls.Winform.Release.VmMainViewConfigControl vmMainViewConfigControl1;
        private GroupBox groupBox3;
        private ListBox listBox1;
        private GroupBox groupBox1;
        private Button buttonPathLoad;
        private ComboBox comboBox1;
        private TextBox textBox1;
        private Button button8;
        private Button button5;
        private Button button6;
        private Label label1;
        private Button button7;
        private Button button3;
        private Button button2;
        private Button button1;
        private Button button4;
        private TextBox textBox2;
        private GroupBox groupBox2;
        private GroupBox groupBoxControl;
        private Button buttonStopExecute;
        private Button buttonExecuteOnce;
        private Button buttonContinuExecute;
        internal GroupBox groupBoxTimeInterval;
        private TextBox textBoxTimeInterval;
        private Button buttonSetTimeInterval;
        private GroupBox groupBox5;
        private VMControls.Winform.Release.VmRenderControl vmRenderControl1;
        private Panel panel1;
        private Panel panelTitleBar;
        private Button btnClose;
        private Button btnMaximize;
        private Button btnMinimize;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem FileStripMenuItem1;
        private ToolStripMenuItem 打开toolStripMenu11;
        private ToolStripMenuItem 另存toolStripMenu12;
        private ToolStripMenuItem CamStripMenuItem2;
        private ToolStripMenuItem ContStripMenuItem3;
        private ToolStripMenuItem DebugStripMenuItem4;
        private ToolStripMenuItem WarnStripMenuItem5;
        private ToolStripMenuItem LogStripMenuItem6;
        private ToolStripMenuItem UserStripMenuItem7;
        private ToolStripMenuItem HelpStripMenuItem8;
        private ToolStripMenuItem 关于toolStripMenu81;
        private ToolStripMenuItem 文档toolStripMenu82;
        private ToolStripMenuItem 扩展ToolStripMenuItem;
        private ToolStripMenuItem 扩展ToolStripMenuItem1;

        //private readonly EventHandler Form1_Load;
    }
}

