using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Pomodoro
{
    public partial class settings : Form
    {
        times myTimes = new times();
        
        public settings(times myTimes)
        {
            InitializeComponent();
            this.myTimes = myTimes;

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                int longBreak = int.Parse(longBreakTextBox.Text);
                int shortBreak = int.Parse(shortBreakTextBox.Text);
                int workTime = int.Parse(workTextBox.Text);
                int numShortBreaks = int.Parse(numShortBreaksTextBox.Text);
                myTimes.longBreak = longBreak;
                myTimes.shortBreak = shortBreak;
                myTimes.workTime = workTime;
                myTimes.numShortBreaks = numShortBreaks;
                String timeStr = longBreak + Environment.NewLine + shortBreak + Environment.NewLine + workTime + Environment.NewLine + numShortBreaks;
                String folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pdoro";
                try
                {
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    FileStream fStream = File.Create(folder + "\\pdoro.cfg");
                    char[] timeArray = timeStr.ToArray();
                    byte[] timeBytes = new byte[timeStr.Length];
                    for (int i = 0; i < timeStr.Length; i++)
                        timeBytes[i] = Convert.ToByte(timeArray[i]);
                    fStream.Write(timeBytes,0,timeStr.Length);
                    fStream.Close();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch
            {
                MessageBox.Show(null,"All fields must be integers.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public times getTimes()
        {
            return myTimes;
        }
        private void settings_Shown(object sender, EventArgs e)
        {
            workTextBox.Text = myTimes.workTime.ToString();
            shortBreakTextBox.Text = myTimes.shortBreak.ToString();
            longBreakTextBox.Text = myTimes.longBreak.ToString();
            numShortBreaksTextBox.Text = myTimes.numShortBreaks.ToString();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
