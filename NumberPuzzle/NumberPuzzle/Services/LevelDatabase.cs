using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Number_Puzzle_Android.Models;

using SQLite;

namespace Number_Puzzle_Android.Services
{
    public class LevelDatabase
    {
        readonly SQLiteAsyncConnection _database;

        private readonly Random random = new Random();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbPath">path database</param>
        public LevelDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Level>().Wait();
            _database.CreateTableAsync<Coins>().Wait();
        }

        /// <summary>
        /// Get list table level async
        /// </summary>
        /// <returns>List level</returns>
        public Task<List<Level>> GetLevelsAsync()
        {
            return _database.Table<Level>().ToListAsync();
        }

        /// <summary>
        /// Get list table coin async
        /// </summary>
        /// <returns>list coin</returns>
        public Task<List<Coins>> GetCoinsAsync()
        {
            return _database.Table<Coins>().ToListAsync();
        }

        /// <summary>
        /// Get Level with variable id
        /// </summary>
        /// <param name="id">id Level</param>
        /// <returns>level</returns>
        public Task<Level> GetLevelAsync(int id)
        {
            return _database.Table<Level>()
                .Where(i => i.ID == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get list table level with variable difficulty
        /// </summary>
        /// <param name="difficulty">difficulty level</param>
        /// <returns>list level</returns>
        public Task<List<Level>> GetLevelDifficultyAsync(string difficulty)
        {
            return _database.Table<Level>()
                .Where(i => i.Difficulty == difficulty)
                .ToListAsync();
        }

        /// <summary>
        /// Get coins with variable id
        /// </summary>
        /// <param name="id">id coins</param>
        /// <returns>coins</returns>
        public Task<Coins> GetCoinsAsync(int id)
        {
            return _database.Table<Coins>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Update or Insert level
        /// </summary>
        /// <param name="level">level</param>
        /// <returns></returns>
        public Task<int> SaveLevelAsync(Level level)
        {
            if (level.ID != 0)
                return _database.UpdateAsync(level);
            else
            {
                return _database.InsertAsync(level);
            }
        }

        /// <summary>
        /// Update or Insert Coins
        /// </summary>
        /// <param name="coins">coins</param>
        /// <returns></returns>
        public Task<int> SaveCoinsAsync(Coins coins)
        {
            if (coins.Id != 0)
                return _database.UpdateAsync(coins);
            else
            {
                return _database.InsertAsync(coins);
            }
        }

        /// <summary>
        /// Delete level
        /// </summary>
        /// <param name="level">level</param>
        /// <returns></returns>
        public Task<int> DeleteLevelAsync(Level level)
        {
            return _database.DeleteAsync(level);
        }
        
        /// <summary>
        /// Number of level passed
        /// </summary>
        /// <param name="listLevel">list level</param>
        /// <returns>number level passed</returns>
        public int NumberOfLevelsPassed(List<Level> listLevel)
        {
            int passed = 0;
            foreach (var e in listLevel)
                if (e.Win == true)
                    passed++;
            return passed;
        }

        /// <summary>
        /// Add level to database
        /// </summary>
        /// <param name="nbLevel">number of level</param>
        /// <param name="nameGameArea">name game area</param>
        /// <param name="difficulty">difficulty</param>
        /// <param name="nRowCol">number row / column</param>
        /// <param name="didHePlay">player play ?</param>
        /// <param name="win">Level passed</param>
        /// <param name="coin">coin</param>
        /// <param name="color">color level</param>
        /// <param name="fontSize">font size text button</param>
        public async void AddLevelBase(int nbLevel , string nameGameArea, string difficulty, int nRowCol, bool didHePlay, 
                                       bool win, int coin, string color, int fontSize)
        {
            int entier;

            for (int i = 0; i < nbLevel; i++)
            {
                if (difficulty == "Easy")
                    entier = random.Next(0, 20);
                else if (difficulty == "Medium")
                    entier = random.Next(10, 30);
                else if (difficulty == "Hard")
                    entier = random.Next(20, 50);
                else
                    entier = random.Next(30, 64);

                Level level = new Level
                {
                    NameGameArea = nameGameArea,
                    Difficulty = difficulty,
                    NbRowCol = nRowCol,
                    DidHePlay = didHePlay,
                    Win = win,
                    Coin = coin,
                    NbFirst = entier,
                    ColorLevelDifficulty = color,
                    FontSize = fontSize
                };
                await SaveLevelAsync(level);
            }
        }
    }
}
