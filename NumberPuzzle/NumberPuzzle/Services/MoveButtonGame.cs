using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Number_Puzzle_Android.Models;

namespace Number_Puzzle_Android.Services
{
    public class MoveButtonGame
    {

        private readonly List<Button> ListMoveButton;

        private bool BoolChangeButton { get; set; }

        private Level Level { get; set; }

        private Frame FrameWin { get; set; }

        private Frame FrameGame { get; set; }

        private Timer Timer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MoveButtonGame()
        {
            ListMoveButton = new List<Button>() { null, null};
            BoolChangeButton = false;
            Level = new Level();
        }

        /// <summary>
        /// Click button game area
        /// </summary>
        /// <param name="tableDatabaseLevel">Level</param>
        /// <param name="frameWin">Frame to </param>
        /// <param name="frameGame"></param>
        /// <param name="timer">Timer game</param>
        public void BtnGame_Clicked(Level tableDatabaseLevel, Frame frameWin, Frame frameGame, Timer timer)
        {
            Level = tableDatabaseLevel;
            FrameWin = frameWin;
            FrameGame = frameGame;
            Timer = timer;
            foreach (var btn in App.ButtonGame)
                btn.Clicked += MoveGame;
            WinningWay();
        }

        private void MoveGame(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            int column = Grid.GetColumn(btn);
            int row = Grid.GetRow(btn);
            ListMoveButton[0] = btn;
            FindButtonEmply(column, row);
            WinningWay();
        }
        
        /// <summary>
        /// Find button empty
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private void FindButtonEmply(int column, int row)
        {
            foreach(var b in App.ButtonGame)
            {
                if (Grid.GetColumn(b) == column - 1 && Grid.GetRow(b) == row && b.AutomationId == "empty")
                    ButtonEmpty(b);
                else if (Grid.GetColumn(b) == column + 1 && Grid.GetRow(b) == row && b.AutomationId == "empty")
                    ButtonEmpty(b);
                else if (Grid.GetColumn(b) == column && Grid.GetRow(b) == row - 1 && b.AutomationId == "empty")
                    ButtonEmpty(b);
                else if (Grid.GetColumn(b) == column && Grid.GetRow(b) == row + 1 && b.AutomationId == "empty")
                    ButtonEmpty(b);
                else if(b.AutomationId == "empty")
                    BoolChangeButton = false;
            }
            if(BoolChangeButton)
                ChangeButton();
        }

        /// <summary>
        /// Change variable button empty
        /// </summary>
        /// <param name="btn"></param>
        private void ButtonEmpty(Button btn)
        {
            ListMoveButton[1] = btn;
            BoolChangeButton = true;
        }

        /// <summary>
        /// Change place button
        /// </summary>
        private void ChangeButton()
        {
            int btnBeforeColumn = Grid.GetColumn(ListMoveButton[0]);
            int btnBeforeRow = Grid.GetRow(ListMoveButton[0]);

            int btnAfterColumn = Grid.GetColumn(ListMoveButton[1]);
            int btnAfterRow = Grid.GetRow(ListMoveButton[1]);


            Grid.SetColumn(ListMoveButton[0], btnAfterColumn);
            Grid.SetRow(ListMoveButton[0], btnAfterRow);

            Grid.SetColumn(ListMoveButton[1], btnBeforeColumn);
            Grid.SetRow(ListMoveButton[1], btnBeforeRow);

            int indexBtnBefore = App.ButtonGame.IndexOf(ListMoveButton[0]);
            int indexBtnAfter = App.ButtonGame.IndexOf(ListMoveButton[1]);
            App.ButtonGame[indexBtnBefore] = ListMoveButton[1];
            App.ButtonGame[indexBtnAfter] = ListMoveButton[0];

            ListMoveButton[0] = null;
            ListMoveButton[1] = null;
            BoolChangeButton = false;
        }

        /// <summary>
        /// Way button puzzle
        /// </summary>
        private void WinningWay()
        {
            int goodPositionBtn = 0;
            for(int i=Level.NbFirst; i < Level.NbFirst + App.ButtonGame.Count; i++)
            {
                if (App.ButtonGame[i-Level.NbFirst].AutomationId == (i).ToString())
                {
                    App.ButtonGame[i - Level.NbFirst].TextColor = Color.Gold;
                    goodPositionBtn++;
                }
                else
                    App.ButtonGame[i - Level.NbFirst].TextColor = Color.Black;
            }
            if (goodPositionBtn == (Level.NbRowCol*Level.NbRowCol)-1)
                BloquedButton();
        }

        /// <summary>
        /// Bloqued button if win
        /// </summary>
        private async void BloquedButton()
        {
            foreach (var b in App.ButtonGame)
                b.IsEnabled = false;

            Timer.Stop();
            Timer.GameWinStopTimer = true;

            Level.Win = true;
            Level.DidHePlay = false;
            Level.ColorBackLevel = "#90ee90";

            Level.Time = $"{Timer.Minutes}:{Timer.Seconds}";

            Coins c = await App.Database.GetCoinsAsync(1);
            c.coins += Level.Coin;

            Level.Coin = 0;

            await App.Database.SaveCoinsAsync(c);
            await App.Database.SaveLevelAsync(Level);

            FrameGame.IsVisible = false;
            FrameWin.IsVisible = true;
        }
    }
}
