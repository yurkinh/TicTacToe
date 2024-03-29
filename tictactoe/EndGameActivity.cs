﻿
using System;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View.Animation;
using Android.Widget;
using Tictactoe.Constants;
using Tictactoe.Enums;
using Tictactoe.Extensions;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share.Widget;

namespace Tictactoe
{
    [Activity(Label = "EndGameActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EndGameActivity : Activity
    {
        TextView textViewWinnerName;
        Button buttonRestart;
        Figures figure;
        ShareButton shareButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.endgame_activity);            

            string winnerText = Intent.GetStringExtra(StringConstants.END_MESSAGE);
            figure = (Figures)Intent.GetIntExtra(StringConstants.FIGURE, (int)Figures.X);

            buttonRestart = FindViewById<Button>(Resource.Id.RestartButton);
            textViewWinnerName = FindViewById<TextView>(Resource.Id.TextViewWinnerName);
            shareButton = FindViewById<ShareButton>(Resource.Id.fb_share_button);
            textViewWinnerName.Text = winnerText;

            //Share content to FB
            ShareLinkContent.Builder builder = new ShareLinkContent.Builder();
            builder.SetContentUrl(Android.Net.Uri.Parse("DI.FM"));
            ShareContent content = builder.Build();
            shareButton.ShareContent = content;            
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

            Finish();
        }

        public override void OnBackPressed()
        {
            
        }
    }
}
