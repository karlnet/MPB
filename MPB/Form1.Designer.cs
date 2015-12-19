namespace MPB
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.gbBar1 = new System.Windows.Forms.GroupBox();
            this.gbMPB2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btnSwitch = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tbTemp = new System.Windows.Forms.TextBox();
            this.tbMAC = new System.Windows.Forms.TextBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.gbBar2 = new System.Windows.Forms.GroupBox();
            this.lvDevice = new System.Windows.Forms.ListView();
            this.chNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMPBIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMPBState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTemp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.gbBar1.SuspendLayout();
            this.gbMPB2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gbBar2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(11, 143);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(116, 23);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "媒体播放机复位";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "MAC地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "显示器1电源：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "显示器2电源：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "媒体播放机电源：";
            // 
            // gbBar1
            // 
            this.gbBar1.Controls.Add(this.gbMPB2);
            this.gbBar1.Controls.Add(this.tbTemp);
            this.gbBar1.Controls.Add(this.tbMAC);
            this.gbBar1.Controls.Add(this.tbIP);
            this.gbBar1.Controls.Add(this.label7);
            this.gbBar1.Controls.Add(this.label1);
            this.gbBar1.Controls.Add(this.label2);
            this.gbBar1.Location = new System.Drawing.Point(341, 21);
            this.gbBar1.Name = "gbBar1";
            this.gbBar1.Size = new System.Drawing.Size(276, 307);
            this.gbBar1.TabIndex = 7;
            this.gbBar1.TabStop = false;
            this.gbBar1.Text = "媒体播放机：";
            // 
            // gbMPB2
            // 
            this.gbMPB2.Controls.Add(this.button2);
            this.gbMPB2.Controls.Add(this.button1);
            this.gbMPB2.Controls.Add(this.label6);
            this.gbMPB2.Controls.Add(this.panel3);
            this.gbMPB2.Controls.Add(this.panel2);
            this.gbMPB2.Controls.Add(this.panel1);
            this.gbMPB2.Controls.Add(this.btnSwitch);
            this.gbMPB2.Controls.Add(this.label4);
            this.gbMPB2.Controls.Add(this.label3);
            this.gbMPB2.Controls.Add(this.pictureBox2);
            this.gbMPB2.Controls.Add(this.label5);
            this.gbMPB2.Controls.Add(this.btnReset);
            this.gbMPB2.Location = new System.Drawing.Point(9, 110);
            this.gbMPB2.Name = "gbMPB2";
            this.gbMPB2.Size = new System.Drawing.Size(247, 179);
            this.gbMPB2.TabIndex = 25;
            this.gbMPB2.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(190, 142);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(23, 23);
            this.button2.TabIndex = 22;
            this.button2.Text = "2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(152, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(23, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(150, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "label6";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radioButton6);
            this.panel3.Controls.Add(this.radioButton5);
            this.panel3.Location = new System.Drawing.Point(129, 76);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(100, 25);
            this.panel3.TabIndex = 10;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(61, 5);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(35, 16);
            this.radioButton6.TabIndex = 22;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "关";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(11, 7);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(35, 16);
            this.radioButton5.TabIndex = 21;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "开";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.RadioCheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Location = new System.Drawing.Point(129, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 25);
            this.panel2.TabIndex = 19;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(61, 3);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(35, 16);
            this.radioButton4.TabIndex = 20;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "关";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(11, 4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(35, 16);
            this.radioButton3.TabIndex = 19;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "开";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.RadioCheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Location = new System.Drawing.Point(129, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 25);
            this.panel1.TabIndex = 18;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(11, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(35, 16);
            this.radioButton1.TabIndex = 16;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "开";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.RadioCheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(61, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(35, 16);
            this.radioButton2.TabIndex = 17;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "关";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // btnSwitch
            // 
            this.btnSwitch.Location = new System.Drawing.Point(11, 114);
            this.btnSwitch.Name = "btnSwitch";
            this.btnSwitch.Size = new System.Drawing.Size(114, 23);
            this.btnSwitch.TabIndex = 15;
            this.btnSwitch.Text = "媒体播放机开关机";
            this.btnSwitch.UseVisualStyleBackColor = true;
            this.btnSwitch.Click += new System.EventHandler(this.btn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Silver;
            this.pictureBox2.Location = new System.Drawing.Point(11, 107);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(220, 1);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // tbTemp
            // 
            this.tbTemp.BackColor = System.Drawing.SystemColors.Control;
            this.tbTemp.Location = new System.Drawing.Point(129, 74);
            this.tbTemp.Name = "tbTemp";
            this.tbTemp.ReadOnly = true;
            this.tbTemp.Size = new System.Drawing.Size(127, 21);
            this.tbTemp.TabIndex = 14;
            // 
            // tbMAC
            // 
            this.tbMAC.BackColor = System.Drawing.SystemColors.Control;
            this.tbMAC.Location = new System.Drawing.Point(129, 48);
            this.tbMAC.Name = "tbMAC";
            this.tbMAC.ReadOnly = true;
            this.tbMAC.Size = new System.Drawing.Size(127, 21);
            this.tbMAC.TabIndex = 13;
            // 
            // tbIP
            // 
            this.tbIP.BackColor = System.Drawing.SystemColors.Control;
            this.tbIP.Location = new System.Drawing.Point(129, 24);
            this.tbIP.Name = "tbIP";
            this.tbIP.ReadOnly = true;
            this.tbIP.Size = new System.Drawing.Size(127, 21);
            this.tbIP.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "媒体播放机温度：";
            // 
            // gbBar2
            // 
            this.gbBar2.Controls.Add(this.lvDevice);
            this.gbBar2.Location = new System.Drawing.Point(12, 18);
            this.gbBar2.Name = "gbBar2";
            this.gbBar2.Size = new System.Drawing.Size(323, 310);
            this.gbBar2.TabIndex = 8;
            this.gbBar2.TabStop = false;
            this.gbBar2.Text = "媒体播放机列表：";
            // 
            // lvDevice
            // 
            this.lvDevice.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chNo,
            this.chIP,
            this.chMPBIP,
            this.chMPBState,
            this.chTemp});
            this.lvDevice.FullRowSelect = true;
            this.lvDevice.HideSelection = false;
            this.lvDevice.Location = new System.Drawing.Point(7, 25);
            this.lvDevice.Name = "lvDevice";
            this.lvDevice.Size = new System.Drawing.Size(310, 275);
            this.lvDevice.TabIndex = 0;
            this.lvDevice.UseCompatibleStateImageBehavior = false;
            this.lvDevice.View = System.Windows.Forms.View.Details;
            this.lvDevice.SelectedIndexChanged += new System.EventHandler(this.lvDevice_SelectedIndexChanged);
            // 
            // chNo
            // 
            this.chNo.Text = "序号";
            this.chNo.Width = 40;
            // 
            // chIP
            // 
            this.chIP.Text = "控制板地址";
            this.chIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chIP.Width = 95;
            // 
            // chMPBIP
            // 
            this.chMPBIP.Text = "播放机地址";
            this.chMPBIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chMPBIP.Width = 75;
            // 
            // chMPBState
            // 
            this.chMPBState.Text = "状态";
            this.chMPBState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chMPBState.Width = 38;
            // 
            // chTemp
            // 
            this.chTemp.Text = "温度";
            this.chTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chTemp.Width = 48;
            // 
            // tbMsg
            // 
            this.tbMsg.BackColor = System.Drawing.SystemColors.Control;
            this.tbMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbMsg.Location = new System.Drawing.Point(12, 340);
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.ReadOnly = true;
            this.tbMsg.Size = new System.Drawing.Size(605, 14);
            this.tbMsg.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(630, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // serialPort1
            // 
            this.serialPort1.RtsEnable = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(630, 294);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(147, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "视频";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(875, 295);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "停止";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 349);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbMsg);
            this.Controls.Add(this.gbBar2);
            this.Controls.Add(this.gbBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "MPB";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbBar1.ResumeLayout(false);
            this.gbBar1.PerformLayout();
            this.gbMPB2.ResumeLayout(false);
            this.gbMPB2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gbBar2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox gbBar1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox gbBar2;
        private System.Windows.Forms.ListView lvDevice;
        private System.Windows.Forms.ColumnHeader chNo;
        private System.Windows.Forms.ColumnHeader chIP;
        private System.Windows.Forms.ColumnHeader chMPBIP;
        private System.Windows.Forms.ColumnHeader chMPBState;
        private System.Windows.Forms.ColumnHeader chTemp;
        private System.Windows.Forms.TextBox tbTemp;
        private System.Windows.Forms.TextBox tbMAC;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btnSwitch;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.GroupBox gbMPB2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

