using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using _2048.DifficultyLevels;

namespace _2048.GameFieldLogic
{
    public class Score
    {
        int _scoreValue;
        public int ScoreValue
        {
            get { return _scoreValue; }
            set
            {
                _scoreValue = value;
                if (ScoreValue > BestScore)
                {
                    BestScore = ScoreValue;
                }
            } 
        }

        public int BestScore { get; private set; }

        string _folderPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "2048");

        string _filePath;



        public Score()
        {
            
        }

    
        public Score(Score copy)
        {
            ScoreValue = copy.ScoreValue;
        }

        public void LoadBestScore(DifficultyLevel? difficultyLevel)
        {
            _filePath = Path.Combine(_folderPath, difficultyLevel.ToString());

            if (File.Exists(_filePath))
            {
                string jsonString = File.ReadAllText(_filePath);

                BestScore = JsonConvert.DeserializeObject<int>(jsonString);
            }
        }

        public void SaveBestScore(DifficultyLevel? difficultyLevel)
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }

            _filePath = Path.Combine(_folderPath, difficultyLevel.ToString());

            string jsonString = JsonConvert.SerializeObject(BestScore);

            File.WriteAllText(_filePath, jsonString);
        }
    }
}