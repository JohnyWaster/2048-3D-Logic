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
                if (_scoreValue > BestScore)
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
            BestScore = copy.BestScore;
        }

        public void LoadBestScore(DifficultyLevel? difficultyLevel)
        {
            _filePath = Path.Combine(_folderPath, difficultyLevel.ToString());

            if (File.Exists(_filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(_filePath);

                    BestScore = JsonConvert.DeserializeObject<int>(jsonString);
                }
                catch (Exception)
                {
                }

                
            }
        }

        public void SaveBestScore(DifficultyLevel? difficultyLevel)
        {
            if (!Directory.Exists(_folderPath))
            {
                try
                {
                    Directory.CreateDirectory(_folderPath);
                }
                catch (Exception)
                {
                    
                }            
            }

            _filePath = Path.Combine(_folderPath, difficultyLevel.ToString());

            string jsonString = JsonConvert.SerializeObject(BestScore);


            try
            {
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception)
            {

            }
        }
    }
}