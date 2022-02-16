using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Number_Puzzle_Android.Models
{
    public class Coins
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int coins { get; set; }
    }
}
