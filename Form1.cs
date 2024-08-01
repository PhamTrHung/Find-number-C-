using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;

namespace FindNumber
{
    public partial class Form1 : Form
    {
        Random ran = new Random();
        int numText;
        int indNum = 0;
        ArrayList numCorrect = new ArrayList(64);
        int playTime = 220;
        int score = 0;
        ArrayList indArr = new ArrayList(64);
        SoundPlayer saoCorrect = new SoundPlayer("..\\..\\floop2_x.wav");
        SoundPlayer saoFault = new SoundPlayer("..\\..\\blurp_x.wav");
        SpeechSynthesizer synth = new SpeechSynthesizer();
        bool isStart = false;
        Font f = new System.Drawing.Font("Arial", 10f, FontStyle.Bold);

        public Form1()
        {
            InitializeComponent();
            for(int i = 0; i < 64; i++)
            {
                indArr.Add(i);
            }

            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.Volume = 50;
            synth.Rate = 1;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            saoCorrect.Play();
            isStart = true;
            btnExit.Enabled = false;

            for (int i = 0; i < 64; i++)
            {
                Button btn = new Button();
                btn.Width = 65;
                btn.Height = 42;
                btn.TabStop = false;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.MouseOverBackColor = Color.Red;
                btn.FlatAppearance.BorderColor = Color.Black;

                numText = ran.Next(0, 100);
                numCorrect.Add(numText);
                btn.Text = numText.ToString();
                btn.Click += BtnNum_Click;
                btn.Font = f;

                tbLoutNum.Controls.Add(btn);
            }

            indNum = ran.Next(numCorrect.Count);
            synth.SpeakAsync(numCorrect[indNum].ToString());

            btnStart.Enabled = false;
        }

        private void BtnNum_Click(object sender, EventArgs e)
        {
            Button btNum = sender as Button;
            
            if(Convert.ToInt32(btNum.Text) == (int)numCorrect[indNum])
            {
                saoCorrect.Play();

                Console.WriteLine(numCorrect.Count.ToString());
                score++;
                btNum.Enabled = false;
                if (numCorrect.Count == 1) return;
                numCorrect.Remove(numCorrect[indNum]);
                indNum = ran.Next(numCorrect.Count);
                Thread.Sleep(150);
                synth.SpeakAsync(numCorrect[indNum].ToString());
            }
            else
            {
                saoFault.Play();
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            isStart = false;
            timer1.Enabled=false;

            btnExit.Enabled=true;
            btnStart.Enabled = true;
            tbLoutNum.Controls.Clear();
            playTime = 220;
            numCorrect.Clear();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            playTime--;
            if (playTime == 0) btnFinish.PerformClick();
            lblTime.Text = playTime.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            synth.SpeakAsync("A Ha Ha Ha");
            if (isStart == true)
                synth.SpeakAsync(numCorrect[indNum].ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {   
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Are you sure ???", "Exit", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
                e.Cancel = true;
        }
    }
}
