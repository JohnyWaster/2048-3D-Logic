using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using _2048.DifficultyLevels;

namespace _2048.Saving
{
    public interface IScoreSaver
    {
        int LoadBestScore(DifficultyLevel? difficultyLevel);

        void SaveBestScore(DifficultyLevel? difficultyLevel, int bestScore);
    }
}