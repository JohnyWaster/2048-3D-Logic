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
using _2048.GameFieldLogic;

namespace _2048.Saving
{
    /// <summary>
    /// class for saving game score and game state in android 
    /// file system
    /// </summary>
    class AndroidFileSystemSaver : IScoreSaver, IGameStateSaver
    {
        readonly string _folderPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "2048");

        string _filePath;

        readonly string _lastGameFileName = "LastGame";


        public int LoadBestScore(DifficultyLevel? difficultyLevel)
        {
            _filePath = FilePathForBestScore(difficultyLevel);

            if (File.Exists(_filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(_filePath);

                    return JsonConvert.DeserializeObject<int>(jsonString);
                }
                catch (Exception)
                {
                    return 0;
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
                    return;
                }
            }

            _filePath = FilePathForBestScore(difficultyLevel);

            string jsonString = JsonConvert.SerializeObject(bestScore);


            try
            {
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception)
            {

            }
        }

        public State LoadGameState()
        {
            _filePath = FilePathForLastGame();

            if (File.Exists(_filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(_filePath);
                    return JsonConvert.DeserializeObject<State>(jsonString);
                }
                catch (Exception e)
                {
                   // "Unable to find a constructor to use for type _2048.GameFieldLogic.State. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute. Path 'Field', line 1, position 9."
                    var mes = e.Message;
                    return null;
                }
            }

            return null;
        }

        public void SaveGameState(State state)
        {
            if (!Directory.Exists(_folderPath))
            {
                try
                {
                    Directory.CreateDirectory(_folderPath);
                }
                catch (Exception)
                {
                    return;
                }
            }

            _filePath = FilePathForLastGame();

            string jsonString = JsonConvert.SerializeObject(state);


            try
            {
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception)
            {

            }
        }

        public void DeleteSavedGame()
        {
            _filePath = FilePathForLastGame();

            if (File.Exists(_filePath))
            {
                try
                {
                    File.Delete(_filePath);
                }
                catch (Exception)
                {
                }
            }
        }

        private string FilePathForLastGame()
        {
            return Path.Combine(_folderPath, _lastGameFileName);
        }

        private string FilePathForBestScore(DifficultyLevel? difficultyLevel)
        {
            return Path.Combine(_folderPath, difficultyLevel.ToString());
        }
    }
}