using System;
namespace Tictactoe.Helpers
{
    public class CustomEventArgs : EventArgs
    {
        public (int,int) CurrentIndex
        {
            get; set;
        }

        public CustomEventArgs((int,int) currentIndex)
        {
            CurrentIndex = currentIndex;
        }
    }
}
