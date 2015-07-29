using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Media;
using System.IO;
namespace Pomodoro
{
    public struct times
    {
       public int shortBreak;
       public int longBreak;
       public int workTime;
       public int numShortBreaks;
    }
    
    public partial class Pomodoro : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MINIMIZE = 0xF020;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public Pomodoro()
        {
            InitializeComponent();
            LoadTimes();
            newPdoro();
            timer1.Enabled = true;
        }
        pdoro myPdoro;
        int currentTime;
        int stageTime;
        times myTimes = new times();

        private void SetDefaultTimes()
        {
            myTimes.workTime = 25;
            myTimes.shortBreak = 5;
            myTimes.longBreak = 25;
            myTimes.numShortBreaks = 4;
        }
        private void LoadTimes()
        {
            String folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pdoro";
            if (Directory.Exists(folder))
            {
                if (File.Exists(folder + "\\pdoro.cfg"))
                {
                    string[] lines = File.ReadAllLines(folder + "\\pdoro.cfg");
                    try
                    {
                        myTimes.longBreak = int.Parse(lines[0]);
                        myTimes.shortBreak = int.Parse(lines[1]);
                        myTimes.workTime = int.Parse(lines[2]);
                        myTimes.numShortBreaks = int.Parse(lines[3]);
                    }
                    catch
                    {
                        SetDefaultTimes();
                    }
                }
                else
                    SetDefaultTimes();
            }
            else
                SetDefaultTimes();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        
        private void minimizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessage(Handle, WM_SYSCOMMAND, SC_MINIMIZE, 0);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {  
                contextMenuStrip1.Show(this,e.Location);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

      

        private void pictureBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                contextMenuStrip1.Show(this, e.Location);
        }

        private String ToTimeString(int time)
        {
            String minutes = ((int)(time / 60)).ToString();
            if (minutes.Length < 2)
                minutes = "0" + minutes;
            String seconds = (time % 60).ToString();
            if (seconds.Length < 2)
                seconds = "0" + seconds;
            return minutes + ":" + seconds;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            currentTime++;
            textBox1.Text = ToTimeString(currentTime);
            if (currentTime >= stageTime)
            {
                currentTime = 0;
                stageTime = myPdoro.AdvanceStage() * 60;
                textBox2.Text = myPdoro.GetStageName();
                SoundPlayer chime = new SoundPlayer(@"chime.wav");
                chime.Play();
            }

        }

        private void currentStageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentTime = 0;
            textBox1.Text = "00:00";
        }

        private void newPdoro()
        {
            myPdoro = new pdoro(myTimes);
            currentTime = 0;
            stageTime = myPdoro.AdvanceStage() * 60;
            textBox2.Text = myPdoro.GetStageName();
            textBox1.Text = ToTimeString(currentTime);
        }
        private void entirePomodoroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newPdoro();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settings settingsForm = new settings(myTimes);
            settingsForm.ShowDialog();
            times newTimes = settingsForm.getTimes();
            if (!newTimes.Equals(myTimes))
            {
                myTimes = newTimes;
                newPdoro();
            }
        }

        private void Pomodoro_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                ShowInTaskbar = true;
            else
                ShowInTaskbar = false;

        }
    }
}
