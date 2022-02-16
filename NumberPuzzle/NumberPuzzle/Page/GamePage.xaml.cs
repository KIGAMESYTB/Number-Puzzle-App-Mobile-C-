using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Number_Puzzle_Android.Models;
using Number_Puzzle_Android.Services;

namespace Number_Puzzle_Android
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        #region Variable

        List<Level> nbLevel;

        Level level;
        
        AreaGame areaGame;
        
        Grid grid;
        
        readonly Timer timer;

        bool onSleep;
        #endregion


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="level">Level</param>
        public GamePage(Level level)
        {
            InitializeComponent();
            this.level = level;
            timer = new Timer();
            Initialisation();
            timer.Start(btnTime);
        }

        /// <summary>
        /// Initialise page
        /// </summary>
        private async void Initialisation()
        {
            onSleep = false;
            nbLevel = await App.Database.GetLevelsAsync();
            btnTime.Text = "00:00";
            lblMin.Text = $"[{level.NbFirst};";
            lblMax.Text = $"{(level.NbFirst + (level.NbRowCol * level.NbRowCol) - 2)}]";
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!onSleep)
            {
                lblCoins.Text = level.Coin.ToString();
                frameGame.IsVisible = true;
                frameWin.IsVisible = false;
                this.Title = $"Level {level.ID}";
                areaGame = new AreaGame(stackAreaGame.Width, level.NbRowCol, level.NbFirst, level.FontSize);
                grid = areaGame.CreateArea();
                stackLoading.IsVisible = false;
                stackAreaGame.Children.Add(grid);
                App.MoveButtonGame.BtnGame_Clicked(level, frameWin, frameGame, timer);
                onSleep = true;
            }
        }

        /// <summary>
        /// Button click before level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnBeforeLevel_Clicked(object sender, EventArgs e)
        {
            timer.InitialiseMinuteSecond();
            if (timer.GameWinStopTimer)
                timer.Start(btnTime);
            if (level.ID <= 1)
                level = await App.Database.GetLevelAsync(nbLevel.Count);
            else
                level = await App.Database.GetLevelAsync(level.ID - 1);
            Initialisation();
            stackAreaGame.Children.Remove(grid);
            stackLoading.IsVisible = true;
            OnAppearing();
        }

        /// <summary>
        /// Button click after level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnAfterLevel_Clicked(object sender, EventArgs e)
        {
            timer.InitialiseMinuteSecond();
            if (timer.GameWinStopTimer)
                timer.Start(btnTime);
            if(level.ID >= nbLevel.Count)
                level = await App.Database.GetLevelAsync(1);
            else
                level = await App.Database.GetLevelAsync(level.ID + 1);
            Initialisation();
            stackAreaGame.Children.Remove(grid);
            stackLoading.IsVisible = true;
            OnAppearing();
        }

        private void Rules_clicked(object sender, EventArgs e)
        {
            RulesAlert();
        }

        /// <summary>
        /// Rules game Display Alert
        /// </summary>
        private async void RulesAlert()
        {
            int nbMax = level.NbFirst + (level.NbRowCol * level.NbRowCol) - 2;
            await DisplayAlert("Rules of the game", $"The goal of the games is to put in ascending order the numbers between [{level.NbFirst} ; " +
                $"{nbMax}]\n----------------\nDirection of the puzzle :\n¤ Left to Right\n¤ Top to Bottom\n----------------\n" +
                $"Time is up : \nThe maximum time for this level is 59:59","It's understood");

        }
    }
}