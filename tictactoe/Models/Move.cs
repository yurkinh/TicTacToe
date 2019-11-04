using Tictactoe.Enums;

namespace Tictactoe.Models
{
    public class Move
    {
        bool isEndingMove;

        public Move(int y, int x, Figures figure)
        {
            Y = y;
            X = x;
            Figure = figure;
        }

        public int Y
        {
            get; private set;
        }
        public int X
        {
            get; private set;
        }
        public Figures Figure
        {
            get; set;
        }
       
        public string EndingMessage
        {
            get;
            private set;
        }
        public bool IsEndingMove() => isEndingMove;

        public void SetEndingMove(string message)
        {
            isEndingMove = true;
            EndingMessage = message;
        }
    }
}
