
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
using Tictactoe.Constants;
using Tictactoe.Enums;

namespace Tictactoe
{
    [Activity(Label = "EndGameActivity")]
    public class EndGameActivity : Activity
    {
        TextView textViewWinnerName;
        Button buttonRestart;
        Figures figure;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.endgame_activity);

            string winnerText = Intent.GetStringExtra(StringConstants.END_MESSAGE);
            figure = (Figures)Intent.GetIntExtra(StringConstants.FIGURE, (int)Figures.X);

            buttonRestart = FindViewById<Button>(Resource.Id.RestartButton);
            textViewWinnerName = FindViewById<TextView>(Resource.Id.TextViewWinnerName);
            textViewWinnerName.Text = winnerText;

        }

        protected override void OnResume()
        {
            base.OnResume();
            buttonRestart.Click += ButtonRestartClickHandler;
        }

        protected override void OnPause()
        {
            base.OnRestart();
            buttonRestart.Click -= ButtonRestartClickHandler;
        }

        private void ButtonRestartClickHandler(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            intent.PutExtra(StringConstants.FIGURE, (int)figure);
            StartActivity(intent);
        }

        public override void OnBackPressed()
        {
            
        }
    }
}
