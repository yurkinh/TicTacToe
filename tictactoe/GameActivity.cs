
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using Tictactoe.Constants;
using Tictactoe.Controllers;
using Tictactoe.Enums;
using Tictactoe.Helpers;
using Tictactoe.Models;

namespace Tictactoe
{
    [Activity(Label = "GameActivity")]
    public class GameActivity : AppCompatActivity
    {
        ImageButton a1, a2, a3;
        ImageButton b1, b2, b3;
        ImageButton c1, c2, c3;
        ImageButton[] rowA, rowB, rowC;
        ImageButton[][] boardTiles;
        ImageButton[] allButtons;
        GameLogicController gameLogicController;
        Figures selectedFigure;

        TextView textViewFigure;
        TextView textViewPlayerTurn;       


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.game_activity);

            selectedFigure = (Figures)Intent.GetIntExtra(StringConstants.FIGURE, (int)Figures.X);

            textViewFigure = FindViewById<TextView>(Resource.Id.textViewPlayerFigure);
            textViewPlayerTurn = FindViewById<TextView>(Resource.Id.textViewPlayerTurn);

            // Initialize Game
            InitGameBoard();

            //Init cells
            a1 = FindViewById<ImageButton>(Resource.Id.buttonA1);
            a2 = FindViewById<ImageButton>(Resource.Id.buttonA2);
            a3 = FindViewById<ImageButton>(Resource.Id.buttonA3);
            b1 = FindViewById<ImageButton>(Resource.Id.buttonB1);
            b2 = FindViewById<ImageButton>(Resource.Id.buttonB2);
            b3 = FindViewById<ImageButton>(Resource.Id.buttonB3);
            c1 = FindViewById<ImageButton>(Resource.Id.buttonC1);
            c2 = FindViewById<ImageButton>(Resource.Id.buttonC2);
            c3 = FindViewById<ImageButton>(Resource.Id.buttonC3);

            //Init rows
            rowA = new[] { a1, a2, a3 };
            rowB = new[] { b1, b2, b3 };
            rowC = new[] { c1, c2, c3 };
            boardTiles = new ImageButton[][] { rowA, rowB, rowC };
            allButtons = new[] { a1, a2, a3, b1, b2, b3, c1, c2, c3 };

            //PreDownload images into cache
            
            Glide.With(this).Load(StringConstants.X_URL);
            Glide.With(this).Load(StringConstants.O_URL);
            //This is not memory leak cuase game activity is always on activity stack
            for (int y = 0; y < boardTiles.Length; y++) //row  
            {
                for (int x = 0; x < boardTiles[y].Length; x++)//column
                {
                    // Remember To Makes Changes To Button
                    int tileNumberY = y;
                    int tileNumberX = x;

                    boardTiles[y][x].Click +=(o,e) =>
                    {
                        textViewPlayerTurn.Text = $"{StringConstants.WIAING_FOR}  {gameLogicController.GetTurn()}";
                        Move playerMove = gameLogicController.MakeMove(tileNumberY, tileNumberX);                        

                        if (playerMove.Figure == Figures.X)
                        {
                            //Get X image from cache and set to ImageButton
                            Glide.With(this).Load(StringConstants.X_URL).Into(boardTiles[playerMove.Y][playerMove.X]);                            
                        }
                        else
                        {
                            //Get O image from cache and set to ImageButton                           
                            Glide.With(this).Load(StringConstants.O_URL).Into(boardTiles[playerMove.Y][playerMove.X]);                            
                        }

                        boardTiles[playerMove.Y][playerMove.X].Enabled = false;

                        if (playerMove.IsEndingMove())
                        {
                            ToEndingScreen(playerMove.EndingMessage);
                            return;
                        }
                    };
                }
            }
        }

        private void InitGameBoard()
        {
            gameLogicController = new GameLogicController(selectedFigure);
            textViewFigure.Text = $"{StringConstants.YOU_ARE_PLAYER} {selectedFigure}";
            textViewPlayerTurn.Text = $"{StringConstants.WIAING_FOR} {selectedFigure}";
        }

        private void ToEndingScreen(string message)
        {
            Intent intent = new Intent(this, typeof(EndGameActivity));
            intent.PutExtra(StringConstants.END_MESSAGE, message);
            intent.PutExtra(StringConstants.FIGURE, (int)selectedFigure);
            StartActivityForResult(intent, IntegerConstants.RequestCode);            
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == IntegerConstants.RequestCode)
            {
                //restart game
                for (int y = 0; y < boardTiles.Length; y++) //row  
                {
                    for (int x = 0; x < boardTiles[y].Length; x++)//column
                    {
                        boardTiles[y][x].Enabled = true;
                        boardTiles[y][x].SetImageDrawable(null);
                    }
                }
                InitGameBoard();
            }
        }


        public override void OnBackPressed()
        {

        }
    }
}
