using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Ex_terminate.App_logic
{
    class ExTimer
    {
        Timer timer = new Timer();
        Timer UITimer = new Timer();
        int time, timeElapsed; // Time in seconds.
        String labelContent = "";
        Label labelToUpdate;

        public ExTimer(int seconds, ElapsedEventHandler handler, Label labelToUpdate)
        {
            time = seconds;
            timeElapsed = 0;

            // Process kill timer.
            timer.Elapsed += handler;
            timer.Elapsed += timer_Elapsed;
            timer.Interval = seconds * 1000; // Seconds to miliseconds.
            timer.Enabled = true;
            GC.KeepAlive(timer);

            this.labelToUpdate = labelToUpdate;
            if (labelToUpdate != null)
            {
                // UI timer.
                UITimer.Elapsed += updateLabel;
                UITimer.Interval = 1000; // Each second.
                UITimer.Enabled = true; // Only enable, if update is wanted.
                GC.KeepAlive(UITimer);
            }
        }



        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            stop();
        }

        void updateLabel(object sender, ElapsedEventArgs e)
        {
            if (labelToUpdate != null)
            {
                timeElapsed++;
                int timeLeft = time - timeElapsed;
                int hours, minutes, seconds;
                hours = (int)timeLeft / 60 / 60;
                minutes = (int)timeLeft / 60 - (hours * 60);
                seconds = (int)timeLeft - (hours * 60 * 60) - (minutes * 60);
                labelContent = "H: " + hours + " M: " + minutes + " S: " + seconds;
                Application.Current.Dispatcher.Invoke((Action)delegate()
                {
                    labelToUpdate.Content = labelContent;
                });
                
            }
        }

        internal void stop()
        {
            timer.Enabled = false;
            timer.Dispose();
            UITimer.Enabled = false;
            UITimer.Dispose();
        }
    }
}
