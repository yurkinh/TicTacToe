using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using Tictactoe.Constants;
using Tictactoe.Enums;
using Android.Views.Animations;
using Android.Animation;
using Tictactoe.Extensions;
using Android.Support.V4.View.Animation;

namespace Tictactoe
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        Button buttonStartGameX;
        Button buttonStartGameO;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main_activity);

            //Init buttons
            buttonStartGameX = FindViewById<Button>(Resource.Id.buttonStartGameX);
            buttonStartGameO = FindViewById<Button>(Resource.Id.buttonStartGameO);
        }
        

        protected override void OnResume()
        {
            base.OnResume();
            buttonStartGameX.Click += StartGameHandler;
            buttonStartGameO.Click += StartGameHandler;
        }        

        protected override void OnRestart()
        {
            base.OnRestart();
            buttonStartGameX.Click -= StartGameHandler;
            buttonStartGameO.Click -= StartGameHandler;
        }

        private async void StartGameHandler(object sender, System.EventArgs e)
        {
            var btn = sender as Button;
            Intent intent = new Intent(this, typeof(GameActivity));
            ObjectAnimator rotation;

            if (btn.Text == Resources.GetString(Resource.String.startGameX))
            {
                intent.PutExtra(StringConstants.FIGURE, (int)Figures.X);
                rotation = ObjectAnimator.OfFloat(buttonStartGameX, "RotationY", 0f, 360f);
            }
            else
            {
                rotation = ObjectAnimator.OfFloat(buttonStartGameO, "RotationY", 0f, 360f);
                intent.PutExtra(StringConstants.FIGURE, (int)Figures.O);
            }

            
            //Animate buttons
            rotation.SetDuration(1000);
            rotation.SetInterpolator(new FastOutSlowInInterpolator());
            await rotation.StartAsync();           


            StartActivity(intent);
            Finish();
        }
    }
}