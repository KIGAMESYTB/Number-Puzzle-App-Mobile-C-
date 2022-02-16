using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Number_Puzzle_Android.Models;

namespace Number_Puzzle_Android
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LevelPage : ContentPage
    {
        #region Variable
        String ActionDifficulty = "All";

        List<Level> listLevel;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public LevelPage()
        {
            InitializeComponent();
            listLevel = new List<Level>();
            listViewLevel.ItemSelected += ListViewLevel_ItemSelected;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.Coins = await App.Database.GetCoinsAsync(1);
            lblCoinsBar.Text = App.Coins.coins.ToString();
            ChooseDifficulty(ActionDifficulty);
            LevelPassed(ActionDifficulty);
            btnDifficulty.Text = ActionDifficulty;
            activityIndicatorLevel.IsVisible = false;
            activityIndicatorLevel.IsRunning = false;
        }
        
        /// <summary>
        /// Selected item in listView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListViewLevel_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            int index = e.SelectedItemIndex;
            await Navigation.PushAsync(new GamePage(listLevel[index]));
        }

        /// <summary>
        /// Click button Difficulty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LevelDataDificult_Clicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            Level level = listLevel[int.Parse(btn.AutomationId)-1];
            await DisplayAlert($"Difficulty {level.Difficulty}", $"The {level.Difficulty} level corresponds to a {level.NbRowCol} x {level.NbRowCol} game grid", "It's understood");
        }

        private async void ChooseDifficulty_Clicked(object sender, EventArgs e)
        {
            ActionDifficulty = await DisplayActionSheet("Choose level difficulty", "Cancel", null, "All", "Easy", "Medium", "Hard", "Legend");
            OnAppearing();
        }

        /// <summary>
        /// Choose difficulty
        /// </summary>
        /// <param name="action"></param>
        private async void ChooseDifficulty(String action)
        {
            if (action == "Easy" || action == "Medium" || action == "Hard" || action == "Legend")
            {
                listViewLevel.ItemsSource = await App.Database.GetLevelDifficultyAsync(action);
                listLevel = await App.Database.GetLevelDifficultyAsync(action);
            }
            else
            {
                listViewLevel.ItemsSource = await App.Database.GetLevelsAsync();
                listLevel = await App.Database.GetLevelsAsync();
            }

        }

        /// <summary>
        /// Number level paased 
        /// </summary>
        /// <param name="action">difficulty level</param>
        private async void LevelPassed(String action)
        {
            if (action == "Easy" || action == "Medium" || action == "Hard" || action == "Legend")
            {
                List<Level> level = await App.Database.GetLevelDifficultyAsync(action);
                lblLevelPassed.Text = $"{App.Database.NumberOfLevelsPassed(level)}/{level.Count}";
            }
            else
            {
                List<Level> level = await App.Database.GetLevelsAsync();
                lblLevelPassed.Text = $"{App.Database.NumberOfLevelsPassed(level)}/{level.Count}";
            }
            
        }


    }
}