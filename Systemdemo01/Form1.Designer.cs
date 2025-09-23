namespace Systemdemo01
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.vmProcedureConfigControl1 = new VMControls.Winform.Release.VmProcedureConfigControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.vmRenderControl1 = new VMControls.Winform.Release.VmRenderControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.vmParamsConfigWithRenderControl1 = new VMControls.Winform.Release.VmParamsConfigWithRenderControl();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.vmParamsConfigControl1 = new VMControls.Winform.Release.VmParamsConfigControl();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.vmProcedureConfigControl1);
            this.groupBox1.Location = new System.Drawing.Point(9, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(419, 468);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter_1);
            // 
            // vmProcedureConfigControl1
            // 
            this.vmProcedureConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmProcedureConfigControl1.Location = new System.Drawing.Point(2, 16);
            this.vmProcedureConfigControl1.Margin = new System.Windows.Forms.Padding(2);
            this.vmProcedureConfigControl1.Name = "vmProcedureConfigControl1";
            this.vmProcedureConfigControl1.Size = new System.Drawing.Size(415, 450);
            this.vmProcedureConfigControl1.TabIndex = 0;
// TODO: “”的代码生成失败，原因是出现异常“无效的基元类型: System.IntPtr。请考虑使用 CodeObjectCreateExpression。”。
            this.vmProcedureConfigControl1.Load += new System.EventHandler(this.vmProcedureConfigControl1_Load_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(9, 495);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(417, 132);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter_1);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 9.25F);
            this.textBox2.Location = new System.Drawing.Point(66, 101);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(344, 22);
            this.textBox2.TabIndex = 7;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 9.25F);
            this.textBox1.Location = new System.Drawing.Point(66, 24);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(344, 22);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("宋体", 9.25F);
            this.button6.Location = new System.Drawing.Point(4, 101);
            this.button6.Margin = new System.Windows.Forms.Padding(2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(56, 22);
            this.button6.TabIndex = 5;
            this.button6.Text = "保存";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("宋体", 9.25F);
            this.button5.Location = new System.Drawing.Point(127, 74);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(56, 22);
            this.button5.TabIndex = 4;
            this.button5.Text = "停止";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("宋体", 9.25F);
            this.button4.Location = new System.Drawing.Point(66, 74);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(56, 22);
            this.button4.TabIndex = 3;
            this.button4.Text = "连续";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 9.25F);
            this.button3.Location = new System.Drawing.Point(4, 74);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 22);
            this.button3.TabIndex = 2;
            this.button3.Text = "单次";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 9.25F);
            this.button2.Location = new System.Drawing.Point(4, 46);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(56, 22);
            this.button2.TabIndex = 1;
            this.button2.Text = "加载";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 9.25F);
            this.button1.Location = new System.Drawing.Point(4, 19);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 22);
            this.button1.TabIndex = 0;
            this.button1.Text = "打开";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox1);
            this.groupBox3.Location = new System.Drawing.Point(430, 496);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(320, 131);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.Font = new System.Drawing.Font("宋体", 9.25F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(2, 16);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(316, 113);
            this.listBox1.TabIndex = 0;
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
            this.vmRenderControl1.Size = new System.Drawing.Size(314, 448);
            this.vmRenderControl1.TabIndex = 3;
            this.vmRenderControl1.Load += new System.EventHandler(this.vmRenderControl1_Load);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.vmRenderControl1);
            this.groupBox4.Location = new System.Drawing.Point(430, 25);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(320, 468);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.vmParamsConfigWithRenderControl1);
            this.groupBox5.Location = new System.Drawing.Point(753, 25);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(541, 378);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // vmParamsConfigWithRenderControl1
            // 
            this.vmParamsConfigWithRenderControl1.BackColor = System.Drawing.Color.Black;
            this.vmParamsConfigWithRenderControl1.CoordinateInfoVisible = true;
            this.vmParamsConfigWithRenderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmParamsConfigWithRenderControl1.ImageSource = null;
            this.vmParamsConfigWithRenderControl1.Location = new System.Drawing.Point(3, 17);
            this.vmParamsConfigWithRenderControl1.ModuleSource = null;
            this.vmParamsConfigWithRenderControl1.MultiImageButtonVisible = false;
            this.vmParamsConfigWithRenderControl1.Name = "vmParamsConfigWithRenderControl1";
            this.vmParamsConfigWithRenderControl1.ParamsConfig = null;
            this.vmParamsConfigWithRenderControl1.ROIVisible = true;
            this.vmParamsConfigWithRenderControl1.Size = new System.Drawing.Size(535, 358);
            this.vmParamsConfigWithRenderControl1.TabIndex = 0;
            this.vmParamsConfigWithRenderControl1.Load += new System.EventHandler(this.vmParamsConfigWithRenderControl1_Load);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.vmParamsConfigControl1);
            this.groupBox6.Location = new System.Drawing.Point(753, 404);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(541, 223);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "groupBox6";
            this.groupBox6.Enter += new System.EventHandler(this.groupBox6_Enter);
            // 
            // vmParamsConfigControl1
            // 
            this.vmParamsConfigControl1.BackColor = System.Drawing.Color.Silver;
            this.vmParamsConfigControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmParamsConfigControl1.Location = new System.Drawing.Point(3, 17);
            this.vmParamsConfigControl1.ModuleSource = null;
            this.vmParamsConfigControl1.Name = "vmParamsConfigControl1";
            this.vmParamsConfigControl1.ParamsConfig = null;
            this.vmParamsConfigControl1.Size = new System.Drawing.Size(535, 203);
            this.vmParamsConfigControl1.TabIndex = 0;
            this.vmParamsConfigControl1.Load += new System.EventHandler(this.vmParamsConfigControl1_Load);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 640);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "HikonDemo1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private VMControls.Winform.Release.VmProcedureConfigControl vmProcedureConfigControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
        private VMControls.Winform.Release.VmRenderControl vmRenderControl1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private VMControls.Winform.Release.VmParamsConfigWithRenderControl vmParamsConfigWithRenderControl1;
        private VMControls.Winform.Release.VmParamsConfigControl vmParamsConfigControl1;
    }
}

