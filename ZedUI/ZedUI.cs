using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedTester;

namespace ZedTester
{
    public partial class ZedUI : Form
    {
        int fps = 0;
        int erode = 3;
        int dilate = 20;

        string fpsLabelValue = "0 FPS";
        DateTime currentTime;

        Emgu.CV.Image<Bgra, byte> finalLeft;
        Emgu.CV.Image<Bgra, byte> finalRight;
        ImageServer server;

        private Gray threshold = new Gray(20);
        private Gray thresholdDepth = new Gray(20);

        private Gray max = new Gray(255);
        private float saturation;
        private float contrast;
        private bool serverInititalised;
        
        private Wrapper wrapper;

        // controls

        //private Emgu.CV.UI.ImageBox backgroundViewLeft;
        //private Emgu.CV.UI.ImageBox backgroundViewRight;
        //private Emgu.CV.UI.ImageBox inputViewLeft;
        //private Emgu.CV.UI.ImageBox inputViewRight;
        //private Emgu.CV.UI.ImageBox outputViewLeft;
        //private Emgu.CV.UI.ImageBox outputViewRight;
        //private Emgu.CV.UI.ImageBox maskViewLeft;
        //private Emgu.CV.UI.ImageBox maskViewRight;

        public ZedUI()
        {
            InitializeComponent();

            int[] setup = { 1, 0, 1, 2, 0 };

            wrapper = new Wrapper(setup, null);
            // z.InitCamera(setup, null);

            // init settings
            this.threshold = new Gray(Properties.Settings.Default.Threshold);
            this.thresholdTrack.Value = (int)this.threshold.Intensity;
            this.erodeTrack.Value = this.erode = Properties.Settings.Default.Erode;
            this.dilateTrack.Value = this.dilate = Properties.Settings.Default.Dilate;
            this.contrast = Properties.Settings.Default.Contrast;
            this.contrastTrack.Value = (int)((this.contrast > 3 ? 1.5 : this.contrast) * 50);

            this.Start();

            this.currentTime = DateTime.Now;

            this.UpdateViews(null, null);

            // update tracks 
        }

        private Emgu.CV.UI.ImageBox InitBox(string name)
        {
            var box = new Emgu.CV.UI.ImageBox();
            box.Dock = System.Windows.Forms.DockStyle.Fill;
            box.Location = new System.Drawing.Point(0, 0);
            box.Name = name;
            box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            box.TabStop = false;
            return box;
        }

        void LogServerMessage(string message)
        {
            this.statusStrip1.BeginInvoke((MethodInvoker)delegate
            {
                this.serverMessage.Text = message;
            });
        }

        bool running;
        int stride = 960 * 4;
        Image<Bgra, byte> left;
        Image<Bgra, byte> right;

        void Start()
        {
            // start an image server
            
            this.running = true;

            // start with stored SVO
            // var zed = new Zed(@"c:\Projects\files\HD720_SN17600_11-04-52.svo");

            //////////////////////////////////////////////////////////
            // CHOOSE YOUR CAMERA                                   //
            //////////////////////////////////////////////////////////

            new Thread(() =>
            {
                while (running)
                {
                    if (wrapper.grab())
                    {
                        if (!this.serverInititalised)
                        {
                            this.server = new ImageServer(this.LogServerMessage);
                            this.serverInititalised = true;
                        }

                        IntPtr leftPointer = wrapper.GetLeft();                       
                        if (leftPointer != IntPtr.Zero)
                        {
                            left = new Image<Bgra, byte>(960, 540, stride, leftPointer);
                            // img = new Mat(new System.Drawing.Size(960, 540), DepthType.Cv8U, 4, color, stride);
                        }
                        IntPtr rightPointer = wrapper.GetRight();
                        if (rightPointer != IntPtr.Zero)
                        {
                            right = new Image<Bgra, byte>(960, 540, stride, rightPointer);
                            // img = new Mat(new System.Drawing.Size(960, 540), DepthType.Cv8U, 4, color, stride);
                        }

                        if (left != null && right != null)
                        {
                            this.AcquireImages(left, right);
                        } else if (left != null)
                        {
                            this.AcquireImages(left, left);
                        }
                        // img.Save("test.jpg");
                        // CvInvoke.Imshow(win1, img); //Show the image
                    }
                }

            })
            { IsBackground = true }.Start();



        }

        private byte[] GetBytes(Image<Bgra, byte> img)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                img.Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        private void AcquireImages(Image<Bgra, byte> finalLeft, Image<Bgra, byte> finalRight)
        {
            try
            {
                this.finalLeft = finalLeft;
                this.finalRight = finalRight;

                this.server.ReplaceImages(GetBytes(this.finalLeft), GetBytes(this.finalRight));

                // calculate FPS
                if ((DateTime.Now - this.currentTime).TotalMilliseconds > 1000)
                {
                    fpsLabelValue = this.fps + " FPS";
                    this.fps = 0;
                    this.currentTime = DateTime.Now;
                }
                else
                {
                    this.fps++;
                }

                // imageLeft = imageLeft.WarpAffine(mat, Emgu.CV.CvEnum.Inter.Linear, Emgu.CV.CvEnum.Warp.FillOutliers, Emgu.CV.CvEnum.BorderType.Constant, new Bgra(0, 0, 0, 0));

                this.outputViewLeft.BeginInvoke((MethodInvoker)delegate
                {
                    //this.inputViewLeft.Image = imageLeft;
                    //this.inputViewRight.Image = imageRight;
                    this.outputViewLeft.Image = this.finalLeft;
                    this.outputViewRight.Image = this.finalRight;
                    //this.depthViewLeft.Image = depth;

                    this.fpsLabel.Text = this.fpsLabelValue;


                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }



        private void clearBackground_Click(object sender, EventArgs e)
        {
            this.wrapper.ResetBackground();
        }

        private void setup()
        {
            var config = new int[]
            {
                this.thresholdTrack.Value,
                this.depthTrack.Value,
                this.erodeTrack.Value,
                this.dilateTrack.Value,
                this.contrastTrack.Value
            };
            if (this.wrapper != null)
            {
                this.wrapper.Setup(config);
            }
        }

        private void thresholdValueBox_TextChanged(object sender, EventArgs e)
        {
            this.threshold = new Gray(this.thresholdTrack.Value);
            this.toolTipThreshold.SetToolTip(this.thresholdTrack, this.thresholdTrack.Value.ToString());

            this.setup();
        }

        private void depthValueBox_TextChanged(object sender, EventArgs e)
        {
            this.thresholdDepth = new Gray(this.depthTrack.Value);
            this.depthToolTip.SetToolTip(this.depthTrack, this.depthTrack.Value.ToString());

            this.setup();
        }

        private void erodeBox_TextChanged(object sender, EventArgs e)
        {
            this.erode = this.erodeTrack.Value;
            this.erodeToolTip.SetToolTip(this.erodeTrack, this.erodeTrack.Value.ToString());

            this.setup();
        }

        private void dilateBox_TextChanged(object sender, EventArgs e)
        {
            this.dilate = this.dilateTrack.Value;
            this.dilateToolTip.SetToolTip(this.dilateTrack, this.dilateTrack.Value.ToString());

            this.setup();
        }

        private void contrast_TextChanged(object sender, EventArgs e)
        {
            this.contrast = this.contrastTrack.Value / 50f;
            this.contrastToolTip.SetToolTip(this.contrastTrack, this.contrast.ToString());

            this.setup();
        }

        private void UpdateViews(object sender, EventArgs e)
        {
            int rowCount = 0;

            //this.tableLayoutViews.RowCount = 0;

            //this.tableLayoutViews.RowStyles[0].Height = this.showBackground.Checked ? 50 : 0;
            //this.tableLayoutViews.RowStyles[1].Height = this.showInput.Checked ? 50 : 0;
            //this.tableLayoutViews.RowStyles[2].Height = this.showMask.Checked ? 50 : 0;
            //this.tableLayoutViews.RowStyles[3].Height = this.showDepth.Checked ? 50 : 0;
            //this.tableLayoutViews.RowStyles[4].Height = this.showOutput.Checked ? 50 : 0;

        }

        private void SaveSettings(object sender, FormClosingEventArgs e)
        {

            Properties.Settings.Default.Erode = this.erode;
            Properties.Settings.Default.Dilate = this.dilate;
            Properties.Settings.Default.Contrast = this.contrast;
            Properties.Settings.Default.Threshold = (int)this.threshold.Intensity;
            Properties.Settings.Default.Save();
        }



        private void sourceSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
