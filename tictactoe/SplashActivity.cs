
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Tictactoe.Constants;
using Tictactoe.Enums;
using Tictactoe.Helpers;

namespace Tictactoe
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            bool isGameStarted = Settings.ContainsKey(StringConstants.GAME_STATE);

            if (isGameStarted)
            {
                Figures selectedFigure = (Figures)Settings.Get<int>(StringConstants.FIGURE, (int)Figures.X);

                Intent intent = new Intent(this, typeof(GameActivity));
                intent.PutExtra(StringConstants.FIGURE, (int)selectedFigure);
                intent.PutExtra(StringConstants.INIT_FROM_STATE, true);
                StartActivity(intent, ActivityOptions.MakeSceneTransitionAnimation(this).ToBundle());
            }
            else
            {
                StartActivity(new Intent(this, typeof(MainActivity)), ActivityOptions.MakeSceneTransitionAnimation(this).ToBundle());
            }

        }
    }
}
