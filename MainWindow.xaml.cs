using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Year_Percentage_Proj
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private DateTime currentDate;
        private TimeZoneInfo localTimeZone;

        public MainWindow()
        {
            currentDate = DateTime.Now;
            localTimeZone = TimeZoneInfo.Local;
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, localTimeZone);
            UpdateDateTime();
            UpdateYearProgress();
            UpdateTimeUntilNextPercent();
        }

        private void UpdateDateTime()
        {
            DateText.Text = currentDate.ToString("MMMM dd, yyyy HH:mm:ss");
        }

        private void UpdateYearProgress()
        {
            int year = currentDate.Year;
            // check if leap year
            int totalDays = DateTime.IsLeapYear(year) ? 366 : 365;

            // start/end of the year
            DateTime startOfYear = new DateTime(year, 1, 1);
            DateTime endOfYear = new DateTime(year, 12, 31, 23, 59, 59);

            // total years in seconds
            double totalSecondsInYear = (endOfYear - startOfYear).TotalSeconds;

            //time start of the year in seconds
            double elapsedSeconds = (currentDate - startOfYear).TotalSeconds;

            // calculates percentage
            double percentage = (elapsedSeconds / totalSecondsInYear) * 100;

            // displayes the percentage
            LargePercentageTextBlock.Text = $"{percentage:0.00}%";
            YearProgressBar.Value = percentage;
        }


        private void UpdateTimeUntilNextPercent()
        {
            int year = currentDate.Year;
            DateTime startOfYear = new DateTime(year, 1, 1);
            DateTime endOfYear = new DateTime(year, 12, 31, 23, 59, 59);
            double totalSecondsInYear = (endOfYear - startOfYear).TotalSeconds;
            double elapsedSeconds = (currentDate - startOfYear).TotalSeconds;
            double currentPercentage = (elapsedSeconds / totalSecondsInYear) * 100;

            // find next whole percentage and calculate seconds until percentage
            double nextWholePercentage = Math.Ceiling(currentPercentage);
            double secondsUntilNextPercent = ((nextWholePercentage / 100) * totalSecondsInYear) - elapsedSeconds;

            TimeSpan time = TimeSpan.FromSeconds(secondsUntilNextPercent);
            string timeText = string.Format("{0:D2}h {1:D2}m {2:D2}s", time.Hours, time.Minutes, time.Seconds);
            TimeUntilNextPercentTextBlock.Text = $"Time until {nextWholePercentage}%: " + timeText;
        }


    }
}
