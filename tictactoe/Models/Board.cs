using Tictactoe.Constants;
using Tictactoe.Enums;

namespace Tictactoe.Models
{
    public class Board
    {
        readonly Figures[,] board;

        public Board()
        {
            board = new Figures[IntegerConstants.BOARD_SIZE, IntegerConstants.BOARD_SIZE];
            IsGameOver = false;
            
            for (int y = 0; y < IntegerConstants.BOARD_SIZE; y++)
              for (int x = 0; x < IntegerConstants.BOARD_SIZE; x++)
                    board[y,x] = Figures.Blank;
        }
        

        public Figures[,] GetBoard()
        {
            return board;
        }

        public bool IsGameOver
        {
            get;
            private set;
        }

        public void UpdateBoard(Move move)
        {
            board[move.Y,move.X] = move.Figure;
        }

        public bool IsWinningMove(Move move)
        {
            int column = 0;
            int row = 0;

            for (int i = 0; i < IntegerConstants.BOARD_SIZE; i++)
            {
                // check column
                if (board[i,move.X] == move.Figure)
                {
                    column++;
                }

                // check row
                if (board[move.Y,i] == move.Figure)
                {
                    row++;
                }
            }

            // validate row & column
            if (column == IntegerConstants.BOARD_SIZE || row == IntegerConstants.BOARD_SIZE)
            {
                IsGameOver = true;
                return true;
            }

            // check diagonal
            if (board[0,0] == board[1,1] &&
                board[0,0] == board[2,2] &&
                board[1,1] != Figures.Blank)
            {
                IsGameOver = true;
                return true;
            }

            // check anti diagonal
            if (board[2,0] == board[1,1] &&
                board[2,0] == board[0,2] &&
                board[1,1] != Figures.Blank)
            {
                IsGameOver = true;
                return true;
            }

            return false;
        }
    }
}
