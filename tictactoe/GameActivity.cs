using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Widget;
using Bumptech.Glide;
using Tictactoe.Constants;
using Tictactoe.Controllers;
using Tictactoe.Enums;
using Tictactoe.Helpers;
using Tictactoe.Models;
using Utf8Json;

namespace Tictactoe
{
    [Activity(Label = "GameActivity", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
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
        Dictionary<int, int> gameState = new Dictionary<int, int>();
        AdView adView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //MobileAds.Initialize(this, "ca-app-pub-8471384010278314~5446313310");
            SetContentView(Resource.Layout.game_activity);            

            textViewFigure = FindViewById<TextView>(Resource.Id.textViewPlayerFigure);
            textViewPlayerTurn = FindViewById<TextView>(Resource.Id.textViewPlayerTurn);

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

            //Add init
            adView = FindViewById<AdView>(Resource.Id.adView);            
            var adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            //Init rows
            rowA = new[] { a1, a2, a3 };
            rowB = new[] { b1, b2, b3 };
            rowC = new[] { c1, c2, c3 };
            boardTiles = new ImageButton[][] { rowA, rowB, rowC };
            allButtons = new[] { a1, a2, a3, b1, b2, b3, c1, c2, c3 };

            // Initialize Game
            selectedFigure = (Figures)Intent.GetIntExtra(StringConstants.FIGURE, (int)Figures.X);
            Settings.Set(StringConstants.FIGURE, (int)selectedFigure);

            //This is not memory leak cuase game activity is always on activity stack
            for (int y = 0; y < boardTiles.Length; y++) //row  
            {
                for (int x = 0; x < boardTiles[y].Length; x++)//column
                {
                    int tileNumberY = y;
                    int tileNumberX = x;
                    boardTiles[y][x].Click += (o, e) =>
                     {
                         textViewPlayerTurn.Text = $"{gameLogicController.GetTurn()}{StringConstants.TURN}";
                         Move playerMove = gameLogicController.MakeMove(tileNumberY, tileNumberX);

                         if (playerMove.Figure == Figures.X)
                         {
                            //Get X image from cache and set to ImageButton
                            Glide.With(this).Load(StringConstants.X_URL).Into(boardTiles[playerMove.Y][playerMove.X]);

                            //Add current gamestate to array
                            gameState.Add(boardTiles[playerMove.Y][playerMove.X].Id, (int)Figures.X);                             
                         }
                         else
                         {
                            //Get O image from cache and set to ImageButton                           
                            Glide.With(this).Load(StringConstants.O_URL).Into(boardTiles[playerMove.Y][playerMove.X]);

                            //Add current gamestate to array
                            gameState.Add(boardTiles[playerMove.Y][playerMove.X].Id, (int)Figures.O);

                             
                         }
                         //Save gamestate to persistent storage
                         //Not the best and most performant way but for small amout of data it is ok
                         // It could be replaced with SQLite or some other persistent storage
                         Settings.Set(StringConstants.GAME_STATE, JsonSerializer.ToJsonString(gameState));
                         boardTiles[playerMove.Y][playerMove.X].Enabled = false;

                         if (playerMove.IsEndingMove())
                         {                             
                             gameState.Clear();
                             Settings.Set(StringConstants.GAME_STATE, JsonSerializer.ToJsonString(gameState));
                             ToEndingScreen(playerMove.EndingMessage);
                             return;
                         }
                     };
                }
            }

            bool initFromState = Intent.GetBooleanExtra(StringConstants.INIT_FROM_STATE, false);
            InitGameBoard(initFromState);
        }        

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == IntegerConstants.REQUEST_CODE)
            {
                gameState.Clear();
                foreach (var button in allButtons)
                {
                    button.Enabled = true;
                    button.SetImageDrawable(null);
                }                
                
                InitGameBoard();
                Settings.Set(StringConstants.GAME_STATE, JsonSerializer.ToJsonString(gameState));
            }
        }

        private void InitGameBoard(bool initFromState = false)
        {            
            gameLogicController = new GameLogicController(selectedFigure);
            textViewFigure.Text = $"{StringConstants.YOU_ARE_PLAYER} {selectedFigure}";
            textViewPlayerTurn.Text = $"{selectedFigure}{StringConstants.TURN}";

            //Restore game from saved state
            if (initFromState)
            {
                string gameStateJson = Settings.Get(StringConstants.GAME_STATE, string.Empty);                
                gameState = JsonSerializer.Deserialize<Dictionary<int, int>>(gameStateJson);

                for (int y = 0; y < boardTiles.Length; y++) //row  
                {
                    for (int x = 0; x < boardTiles[y].Length; x++)//column
                    {                      
                        
                        if (gameState.ContainsKey(boardTiles[y][x].Id))
                        {
                            textViewPlayerTurn.Text = $"{gameLogicController.GetTurn()}{StringConstants.TURN}";
                            Move playerMove = gameLogicController.MakeMove(y, x);
                            Glide.With(this).Load(playerMove.Figure == Figures.X ? StringConstants.X_URL : StringConstants.O_URL).Into(boardTiles[playerMove.Y][playerMove.X]);
                            boardTiles[y][x].Enabled = false;                            
                        }
                    }
                }

            }
        }

        private void ToEndingScreen(string message)
        {
            Intent intent = new Intent(this, typeof(EndGameActivity));
            intent.PutExtra(StringConstants.END_MESSAGE, message);
            intent.PutExtra(StringConstants.FIGURE, (int)selectedFigure);
            StartActivityForResult(intent, IntegerConstants.REQUEST_CODE);
        }

        public override void OnBackPressed()
        {
            //prevent back button press
        }
        
    }
}
