using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;

namespace Number_Puzzle_Android.Models
{
    public class Level
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string NameGameArea { get; set; }

        public string NameLevel { get; set; }

        public string Difficulty { get; set; }

        public string ColorBackLevel { get; set; } = "#ebf2da";

        public string ColorFrontLevel { get; set; } = "Black";

        public string Time { get; set; } = "None";

        public string ImageSourceWin { get; set; } = "validate.png";

        public int NbRowCol { get; set; }

        public bool DidHePlay { get; set; } = false;

        public bool Win { get; set; } = true;

        public int Coin { get; set; }

        public int NbFirst { get; set; }

        public string ColorLevelDifficulty { get; set; }

        public int FontSize { get; set; }

       
    }
}
