using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pomodoro
{
    class pdoro
    {
        private int stage = -1;
        private times myTimes = new times();
        public pdoro(times myTimes)
        {

            this.myTimes = myTimes;
        }
        public int AdvanceStage()
        {
            stage++;
            if (stage > (myTimes.numShortBreaks * 2) - 1)
                stage = 0;
            if (stage == (myTimes.numShortBreaks * 2) - 1)
                return myTimes.longBreak;
            else if (stage % 2 == 0)
                return myTimes.workTime;
            else
                return myTimes.shortBreak;
        }
        public String GetStageName()
        {
            if (stage == (myTimes.numShortBreaks * 2) - 1)
                return myTimes.longBreak.ToString() + " minute break.";
            else if (stage % 2 == 0)
                return "Work for " + myTimes.workTime.ToString() + " minutes.";
            else
                return myTimes.shortBreak.ToString() + " minute break.";
        }
    }
}
