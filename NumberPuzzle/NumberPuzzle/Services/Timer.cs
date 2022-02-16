using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Number_Puzzle_Android.Services
{
    public class Timer
    {
        public int Minutes { get; set; }

        public int Seconds { get; set; }
        
        private bool StopTimer { get; set; }

        public bool GameWinStopTimer { get; set; } = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public Timer()
        {
            Minutes = 0;
            Seconds = 0;
            StopTimer = true;
        }

        /// <summary>
        /// Start Timer
        /// </summary>
        /// <param name="btnTextDisplay"></param>
        public void Start(Button btnTextDisplay)
        {
            Minutes = 0;
            Seconds = 0;
            GameWinStopTimer = false;
            StartTimerDevice(btnTextDisplay, 1);
        }

        /// <summary>
        /// Initialise Timer to 0
        /// </summary>
        public void InitialiseMinuteSecond()
        {
            Minutes = 0;
            Seconds = 0;
            StopTimer = true;
        }

        /// <summary>
        /// Stop Timer
        /// </summary>
        public void Stop()
        {
            StopTimer = false;
        }

        /// <summary>
        /// Start Timer 
        /// </summary>
        /// <param name="btnTextDisplay"></param>
        /// <param name="sec"></param>
        private void StartTimerDevice(Button btnTextDisplay, int sec)
        {
            Device.StartTimer(new TimeSpan(0, 0, sec), () =>
            {
                Seconds++;
                if (Seconds == 60)
                {
                    Minutes++;
                    Seconds = 0;
                }
                Device.InvokeOnMainThreadAsync(() =>
                {
                    string m = Minutes.ToString();
                    string s = Seconds.ToString();
                    if (Minutes < 10)
                        m = $"0{Minutes}";
                    if (Seconds < 10)
                        s = $"0{Seconds}";
                    btnTextDisplay.Text = $"{m}:{s}";
                });

                if (Minutes == 59 && Seconds == 59)
                    LeftGame();

                return StopTimer;
            });
        }

        /// <summary>
        /// Time is up
        /// </summary>
        private async void LeftGame()
        {
            await App.Current.MainPage.DisplayAlert("Time is up", "You have reached the maximum time for this level 59:59", "Ok");
            await App.Current.MainPage.Navigation.PopAsync();
            StopTimer = false;
        }
    }
}
