using Ex_terminate.App_logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ex_terminate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ProcessList procList = new ProcessList();
        bool timerActive = false;
        ExTimer curTimer;

        public MainWindow()
        {
            InitializeComponent();
            update();

            // Set initial empty values.
            selectedProcess.Content = "";
            timeLabel.Content = "H: 0 M: 0";
            timedProcLabel.Content = "";
            timerRunningLabel.Content = "H: 0 M: 0 S: 0";
        }

        public void update() {
            processNames.ItemsSource = procList.ProcNames;
        }

        private void Update_ProcList_Click(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void Select_Proc_Click(object sender, RoutedEventArgs e)
        {
            // Get index of selection.
            int i = processNames.SelectedIndex;
            if (i != -1)
            {
                procList.setSelProc(i);
                selectedProcess.Content = processNames.SelectedValue;
            }
        }

        private void timeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int hours, minutes;
            int time = (int)timeSlider.Value;
            hours = (int)time / 60;
            minutes = time - (hours * 60);
            timeLabel.Content = "H: " + hours + " M: " + minutes;
        }

        private void Start_time_click(object sender, RoutedEventArgs e)
        {
            if (timerActive == false && procList.SelectedProcess != null)
            {
                timerActive = true;
                startTimeBut.Content = "Stop!";
                selectedProcess.Content = "";
                timedProcLabel.Content = StringHandling.UppercaseFirst(procList.SelectedProcess.ProcessName);
                int timeSecs = ((int)timeSlider.Value) * 60;
                ElapsedEventHandler handler = delegate(object anotherSender, ElapsedEventArgs args)
                {
                    procList.killProcess(procList.SelectedProcess);
                    timerActive = false;
                    Application.Current.Dispatcher.Invoke((Action)delegate()
                    {
                        startTimeBut.Content = "Start!";
                        timedProcLabel.Content = "";
                        timerRunningLabel.Content = "H: 0 M: 0 S: 0";
                    });
                };
                curTimer = new ExTimer(timeSecs, handler, timerRunningLabel);
            }
            else if (timerActive == true && curTimer != null)
            {
                curTimer.stop();
                curTimer = null;
                startTimeBut.Content = "Start!";
                timerActive = false;
                timedProcLabel.Content = "";
                timerRunningLabel.Content = "H: 0 M: 0 S: 0";
            }
        }

    }
}
