using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace _2048
{
    [Activity(Label = "2048"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        Game2048 game;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            game = new Game2048();
            SetContentView((View)game.Services.GetService(typeof(View)));
            game.Run();
        }


        protected override void OnPause()
        {
            game.myScore.SaveBestScore(game.FirstScreen.DifficultyLevel);
            base.OnPause();
        }
    }
}

