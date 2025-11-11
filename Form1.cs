using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;

namespace SoundRecorder
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll")]
        private static extern int mciSendString(string MciComando, string MciRetorno, int MciRetornoLeng, int CallBack);

        string record = "";
        public Form1()
        {
            InitializeComponent();
        }

        //sound card record

        WasapiCapture capture;
        WaveWriter w;

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Visible = false;

        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            capture = new WasapiLoopbackCapture();
            //initialize the selected device for recording
            capture.Initialize();
            //mciSendString("open new type waveaudio alias recsound", null, 0, 0);
            //mciSendString("record recsound", null, 0, 0);
            string datestr = string.Format("{0:yyyymmddHHmm }", DateTime.Now);
            w = new WaveWriter("soundcard" + datestr + ".wav", capture.WaveFormat);

            capture.DataAvailable += (s, e1) =>
            {
                //save the recorded audio
                w.Write(e1.Data, e1.Offset, e1.ByteCount);
            };

            //start recording
            capture.Start();

        }

        private void btnStopandSave_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            capture.Stop();

            w.Dispose();
            capture.Dispose();
            
            //mciSendString("pause recsound", null, 0, 0);
            //SaveFileDialog save = new SaveFileDialog();
            //save.Filter = "wave|*.wav";

            //if (save.ShowDialog() == DialogResult.OK)
            //{

            //    mciSendString("save recsound " + save.FileName, null, 0, 0);
            //    mciSendString("close recsound", null, 0, 0);
            //}
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (record == "")
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Wave|*.wav";
                if (open.ShowDialog() == DialogResult.OK) { record = open.FileName; }
            }
            mciSendString("play " + record, null, 0, 0);
        }
    }
}