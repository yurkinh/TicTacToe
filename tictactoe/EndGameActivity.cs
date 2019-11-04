﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View.Animation;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Tictactoe.Constants;
using Tictactoe.Enums;
using Tictactoe.Extensions;

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

        private async void ButtonRestartClickHandler(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(GameActivity));
            intent.PutExtra(StringConstants.FIGURE, (int)figure);
            //Animate button
            var rotation = ObjectAnimator.OfFloat(buttonRestart, "RotationX", 0f, 360f);
            rotation.SetDuration(1000);
            rotation.SetInterpolator(new FastOutSlowInInterpolator());
            await rotation.StartAsync();
            StartActivity(intent);
        }

        public override void OnBackPressed()
        {
            
        }
    }
}
