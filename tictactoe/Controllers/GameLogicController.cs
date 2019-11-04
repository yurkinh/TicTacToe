using Tictactoe.Enums;
using Tictactoe.Models;

namespace Tictactoe.Controllers
{
    public class GameLogicController
    {         
        
        Board board = new Board();
        Figures turn;
        int turnCount = 1;

        public GameLogicController(Figures figure)
        {            
            turn = figure;
        }

        public Figures GetTurn() => (turn != Figures.X) ? Figures.X : Figures.O;        

        public Move MakeMove(int y, int x)
        {
            Move move = new Move(y, x, turn);
            board.UpdateBoard(move);
            move = IsEndingMove(move);
            return move;
        }        

        Move IsEndingMove(Move move)
        {
            if (board.IsWinningMove(move))
            {
                move.SetEndingMove(turn.ToString());
            }
            else if (turnCount >= 9)
            {
                move.SetEndingMove("The Game Ended In A Tie");
            }
            else NextTurn();
            return move;
        }

        void NextTurn()
        {
            turnCount++;
            turn = (turn != Figures.X) ? Figures.X : Figures.O;
        }        
    }
}
