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

namespace _2048.Saving
{
    class AndroidFileSystemSaver : IScoreSaver
    {
        readonly string _folderPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "2048");

        string _filePath;


        public int LoadBestScore(DifficultyLevel? difficultyLevel)
        {
            _filePath = Path.Combine(_folderPath, difficultyLevel.ToString());

            if (File.Exists(_filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(_filePath);

                    return JsonConvert.DeserializeObject<int>(jsonString);
                }
                catch (Exception)
                {
                }
            }

            return 0;
        }

        public void SaveBestScore(DifficultyLevel? difficultyLevel, int bestScore)
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

            string jsonString = JsonConvert.SerializeObject(bestScore);


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