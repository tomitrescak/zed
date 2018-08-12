using System.Windows.Forms;

namespace ZedTester
{
    partial class ZedUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZedUI));
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.serverMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.fpsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.depthTrack = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.contrastTrack = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.dilateTrack = new System.Windows.Forms.TrackBar();
            this.erodeLabel = new System.Windows.Forms.Label();
            this.erodeTrack = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.thresholdTrack = new System.Windows.Forms.TrackBar();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.outputViewLeft = new Emgu.CV.UI.ImageBox();
            this.outputViewRight = new Emgu.CV.UI.ImageBox();
            this.toolTipThreshold = new System.Windows.Forms.ToolTip(this.components);
            this.depthToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.erodeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dilateToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contrastToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.depthTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dilateTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.erodeTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputViewLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputViewRight)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1083, 32);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverMessage,
            this.fpsLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 823);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1083, 30);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip2";
            // 
            // serverMessage
            // 
            this.serverMessage.Name = "serverMessage";
            this.serverMessage.Size = new System.Drawing.Size(114, 25);
            this.serverMessage.Text = "Server Ready";
            // 
            // fpsLabel
            // 
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(41, 25);
            this.fpsLabel.Text = "FPS";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.depthTrack);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.contrastTrack);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dilateTrack);
            this.panel2.Controls.Add(this.erodeLabel);
            this.panel2.Controls.Add(this.erodeTrack);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.thresholdTrack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(817, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(266, 791);
            this.panel2.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Depth";
            // 
            // depthTrack
            // 
            this.depthTrack.Location = new System.Drawing.Point(7, 315);
            this.depthTrack.Maximum = 255;
            this.depthTrack.Name = "depthTrack";
            this.depthTrack.Size = new System.Drawing.Size(247, 69);
            this.depthTrack.TabIndex = 8;
            this.depthTrack.ValueChanged += new System.EventHandler(this.depthValueBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 219);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Contrast";
            // 
            // contrastTrack
            // 
            this.contrastTrack.Location = new System.Drawing.Point(7, 243);
            this.contrastTrack.Maximum = 255;
            this.contrastTrack.Name = "contrastTrack";
            this.contrastTrack.Size = new System.Drawing.Size(247, 69);
            this.contrastTrack.TabIndex = 6;
            this.contrastTrack.ValueChanged += new System.EventHandler(this.contrast_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Dilate";
            // 
            // dilateTrack
            // 
            this.dilateTrack.Location = new System.Drawing.Point(7, 174);
            this.dilateTrack.Maximum = 30;
            this.dilateTrack.Name = "dilateTrack";
            this.dilateTrack.Size = new System.Drawing.Size(247, 69);
            this.dilateTrack.TabIndex = 4;
            this.dilateTrack.ValueChanged += new System.EventHandler(this.dilateBox_TextChanged);
            // 
            // erodeLabel
            // 
            this.erodeLabel.AutoSize = true;
            this.erodeLabel.Location = new System.Drawing.Point(16, 79);
            this.erodeLabel.Name = "erodeLabel";
            this.erodeLabel.Size = new System.Drawing.Size(52, 20);
            this.erodeLabel.TabIndex = 3;
            this.erodeLabel.Text = "Erode";
            // 
            // erodeTrack
            // 
            this.erodeTrack.Location = new System.Drawing.Point(7, 103);
            this.erodeTrack.Maximum = 30;
            this.erodeTrack.Name = "erodeTrack";
            this.erodeTrack.Size = new System.Drawing.Size(247, 69);
            this.erodeTrack.TabIndex = 2;
            this.erodeTrack.ValueChanged += new System.EventHandler(this.erodeBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Threshold";
            // 
            // thresholdTrack
            // 
            this.thresholdTrack.Location = new System.Drawing.Point(7, 33);
            this.thresholdTrack.Maximum = 255;
            this.thresholdTrack.Name = "thresholdTrack";
            this.thresholdTrack.Size = new System.Drawing.Size(247, 69);
            this.thresholdTrack.TabIndex = 0;
            this.thresholdTrack.ValueChanged += new System.EventHandler(this.thresholdValueBox_TextChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Black;
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 32);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.outputViewLeft);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.outputViewRight);
            this.splitContainer2.Size = new System.Drawing.Size(817, 791);
            this.splitContainer2.SplitterDistance = 598;
            this.splitContainer2.TabIndex = 3;
            // 
            // outputViewLeft
            // 
            this.outputViewLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputViewLeft.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.outputViewLeft.Location = new System.Drawing.Point(0, 0);
            this.outputViewLeft.Name = "outputViewLeft";
            this.outputViewLeft.Size = new System.Drawing.Size(596, 789);
            this.outputViewLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.outputViewLeft.TabIndex = 2;
            this.outputViewLeft.TabStop = false;
            // 
            // outputViewRight
            // 
            this.outputViewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputViewRight.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.outputViewRight.Location = new System.Drawing.Point(0, 0);
            this.outputViewRight.Name = "outputViewRight";
            this.outputViewRight.Size = new System.Drawing.Size(213, 789);
            this.outputViewRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.outputViewRight.TabIndex = 2;
            this.outputViewRight.TabStop = false;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(155, 29);
            this.toolStripButton1.Text = "Clear Background";
            this.toolStripButton1.Click += new System.EventHandler(this.clearBackground_Click);
            // 
            // ZedUI
            // 
            this.ClientSize = new System.Drawing.Size(1083, 853);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip2);
            this.Name = "ZedUI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveSettings);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.depthTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dilateTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.erodeTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdTrack)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outputViewLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputViewRight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStrip toolStrip2;
        private SplitContainer splitContainer2;
        private Emgu.CV.UI.ImageBox outputViewLeft;
        private Emgu.CV.UI.ImageBox outputViewRight;
        private Panel panel2;
        private StatusStrip statusStrip1;
        private TrackBar thresholdTrack;
        private Label label1;
        private Label label3;
        private TrackBar contrastTrack;
        private Label label2;
        private TrackBar dilateTrack;
        private Label erodeLabel;
        private TrackBar erodeTrack;
        private ToolStripStatusLabel serverMessage;
        private ToolStripStatusLabel fpsLabel;
        private Label label4;
        private TrackBar depthTrack;
        private ToolTip toolTipThreshold;
        private ToolTip depthToolTip;
        private ToolTip erodeToolTip;
        private ToolTip dilateToolTip;
        private ToolTip contrastToolTip;
        private ToolStripButton toolStripButton1;
    }
}