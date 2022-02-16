using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Number_Puzzle_Android.Models
{
    public class AreaGame
    {
        public double WidthArea { get; set; }

        public int NbRolCol { get; set; }

        public int NbFirst { get; set; }

        public int FontSizeButton { get; set; }

        private List<int> ListNum { get; set;} = new List<int>();

        private Random Random { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Width Area Game</param>
        /// <param name="nbRolCol">Number row / column</param>
        /// <param name="nbFirst">first number button</param>
        /// <param name="fontSize">font size text button</param>
        public AreaGame(double width, int nbRolCol, int nbFirst, int fontSize)
        {
            WidthArea = width;
            NbRolCol = nbRolCol;
            NbFirst = nbFirst;
            FontSizeButton = fontSize;
            Random = new Random();
        }

        /// <summary>
        /// Area Game (Grid)
        /// </summary>
        /// <returns>Grid</returns>
        public Grid CreateArea()
        {
            Grid grid = CreateGrid(NbRolCol); 
            int nameBtn = NbFirst;

            App.ButtonGame.Clear();

            for (int i=NbFirst; i < (NbFirst + (NbRolCol * NbRolCol) - 1); i++)
                ListNum.Add(i);

            AddButtonGrid(grid, nameBtn);

            return grid;
        }

        /// <summary>
        /// Add button to grid
        /// </summary>
        /// <param name="grid">grid (area game)</param>
        /// <param name="nb">number Row/Column</param>
        private void AddButtonGrid(Grid grid, int nb)
        {
            for (int i = 0; i < NbRolCol; i++)
            {
                for (int j = 0; j < NbRolCol; j++)
                {
                    int entier = Random.Next(ListNum.Count);
                    Button btn = CreateButton();
                    if (i != NbRolCol - 1 || j != NbRolCol - 1)
                    {
                        btn.Text = ListNum[entier].ToString();
                        btn.AutomationId = ListNum[entier].ToString();
                        ListNum.Remove(ListNum[entier]);
                    }
                    else
                    {
                        btn.AutomationId = "empty";
                        btn.CornerRadius = 20;
                        btn.BackgroundColor = Color.Khaki;
                    }

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                    grid.Children.Add(btn);
                    App.ButtonGame.Add(btn);
                    nb++;
                }
            }
        }

        /// <summary>
        /// Create Button
        /// </summary>
        /// <returns>button</returns>
        private Button CreateButton()
        {
            Button btn = new Button
            {
                WidthRequest = (WidthArea / NbRolCol) - 10,
                HeightRequest = (WidthArea / NbRolCol) - 10,
                FontSize = FontSizeButton,
                CornerRadius = 5,
                BackgroundColor = Color.FromHex("ebf2da"),
                BorderColor = Color.Black,
                BorderWidth = 1.5,
                TextColor = Color.Black
            };

            return btn;
        }

        /// <summary>
        /// Create Grid
        /// </summary>
        /// <param name="nb">number row/column</param>
        /// <returns></returns>
        private Grid CreateGrid(int nb)
        {
            Grid grid = new Grid()
            {
                HeightRequest = WidthArea,
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
            };

            for(int i=0; i < nb; i++)
            {
                grid.RowDefinitions.Add(CreateRowDefinition());
                grid.ColumnDefinitions.Add(CreateColumnDefinition());
            }

            return grid;
        }

        /// <summary>
        /// Create row definition
        /// </summary>
        /// <returns>RowDefinition</returns>
        private RowDefinition CreateRowDefinition()
        {
            RowDefinition rowDefinition = new RowDefinition(){ Height = new GridLength(1, GridUnitType.Star)};

            return rowDefinition;
        }

        /// <summary>
        /// Create column definition
        /// </summary>
        /// <returns>ColumnDefinition</returns>
        private ColumnDefinition CreateColumnDefinition()
        {
            ColumnDefinition columnDefinition = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };

            return columnDefinition;
        }
    }
}
