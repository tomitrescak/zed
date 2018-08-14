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

        string fpsLabelValue = "0 FPS";
        DateTime currentTime;

        Emgu.CV.Image<Bgra, byte> finalLeft;
        Emgu.CV.Image<Bgra, byte> finalRight;
        ImageServer server;

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

           
            // init settings
            this.thresholdTrack.Value = Properties.Settings.Default.Threshold;
            this.erodeTrack.Value = Properties.Settings.Default.Erode;
            this.dilateTrack.Value = Properties.Settings.Default.Dilate;
            this.contrastTrack.Value = (int) Properties.Settings.Default.Contrast;
            this.resolution.SelectedIndex = Properties.Settings.Default.Resolution;
            this.depthQuality.SelectedIndex = Properties.Settings.Default.DepthQuality;
            this.depthMode.SelectedIndex = Properties.Settings.Default.DepthMode;

            this.startZed();

            this.currentTime = DateTime.Now;

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
                Properties.Settings.Default.Threshold,
                Properties.Settings.Default.Depth,
                Properties.Settings.Default.Erode,
                Properties.Settings.Default.Dilate,
                Properties.Settings.Default.Contrast
            };

            Properties.Settings.Default.Save();

            if (this.wrapper != null)
            {
                this.wrapper.Setup(config);
            }
            
        }

        private void thresholdValueBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Threshold = this.thresholdTrack.Value;
            this.toolTipThreshold.SetToolTip(this.thresholdTrack, this.thresholdTrack.Value.ToString());

            this.setup();
        }

        private void depthValueBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Depth = this.depthTrack.Value;
            this.depthToolTip.SetToolTip(this.depthTrack, this.depthTrack.Value.ToString());

            this.setup();
        }

        private void erodeBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Erode = this.erodeTrack.Value;
            this.erodeToolTip.SetToolTip(this.erodeTrack, this.erodeTrack.Value.ToString());

            this.setup();
        }

        private void dilateBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Dilate = this.dilateTrack.Value;
            this.dilateToolTip.SetToolTip(this.dilateTrack, this.dilateTrack.Value.ToString());

            this.setup();
        }

        private void contrast_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Contrast = this.contrastTrack.Value;
            this.contrastToolTip.SetToolTip(this.contrastTrack, this.contrastTrack.Value.ToString());

            this.setup();
        }

        private void SaveSettings(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }



        private void sourceSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        

        private void startZed()
        {
            int[] setup = {
                Properties.Settings.Default.Resolution,
                Properties.Settings.Default.DepthQuality,
                Properties.Settings.Default.DepthMode,
                Properties.Settings.Default.Other,
                0
            };

            this.wrapper = new Wrapper(setup, null);
            this.Start();
        }

        private void resolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Resolution = this.resolution.SelectedIndex;
            Properties.Settings.Default.Save();

            this.running = false;
            if (this.wrapper != null)
            {
                this.wrapper.cleanup();
            }
        }

        private void depthQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DepthQuality = this.depthQuality.SelectedIndex;
            Properties.Settings.Default.Save();

            this.running = false;
            if (this.wrapper != null)
            {
                this.wrapper.cleanup();
            }
        }

        private void depthMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DepthMode = this.depthMode.SelectedIndex;
            Properties.Settings.Default.Save();

            this.running = false;
            if (this.wrapper != null)
            {
                this.wrapper.cleanup();
            }
        }
    }
}
