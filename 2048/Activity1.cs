using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using LightInject;
using _2048.Saving;
using _2048.UserActions;

namespace _2048
{
    [Activity(Label = "2048 3D Logic"
        , MainLauncher = true
        , Icon = "@drawable/IconCube"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        Game2048 game;

        public static ServiceContainer IoCContainer { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
        }

        protected override void OnResume()
        {         
            IoCContainer = new ServiceContainer();

            IoCContainer.Register<IScoreSaver>(factory => new AndroidFileSystemSaver());
            IoCContainer.Register<IGameStateSaver>(factory => new AndroidFileSystemSaver());

            game = new Game2048();
            SetContentView((View)game.Services.GetService(typeof(View)));
            game.Run();
            base.OnResume();
        }

        protected override void OnPause()
        {
            game.myScore.SaveBestScore(game.FirstScreen.DifficultyLevel);
            game.GetGameState().SaveGameState();       
            base.OnPause();
        }
    }
}

