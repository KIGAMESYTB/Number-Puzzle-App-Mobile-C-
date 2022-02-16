using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Number_Puzzle_Android.Services;
using Number_Puzzle_Android.Models;

namespace Number_Puzzle_Android
{
    public partial class App : Application
    {
        public static MoveButtonGame MoveButtonGame { get; set; } = new MoveButtonGame();

        public static LevelDatabase Database { get; set; }

        public static Level Level { get; set; }

        public static Coins Coins { get; set; }

        public static List<Button> ButtonGame { get; set; } = new List<Button>();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.PapayaWhip,
                BarTextColor = Color.Black
            };
        }

        protected async override void OnStart()
        {
            if (Database == null)
            {
                Database = new LevelDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "level.db3"));
                List<Level> listLevel = await Database.GetLevelsAsync();
                if (listLevel.Count == 0)
                {
                    Database.AddLevelBase(20, "3x3", "Easy", 3, true, false, 20, "#32cd32", 30);
                    Database.AddLevelBase(17, "4x4", "Medium", 4, true, false, 30, "#ff7f00", 30);
                    Database.AddLevelBase(13, "5x5", "Hard", 5, true, false, 50, "#ff0000", 30);
                    Database.AddLevelBase(8, "6x6", "Legend", 6, true, false, 70, "#000000", 20);
                }

                Coins coins = new Coins
                {
                    coins = 0,
                };
                await Database.SaveCoinsAsync(coins);

            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
