using Libplanet.Store;

namespace Scripts.States
{
    public class BoardState : DataModel
    {
        public int[,] Board { get; private set; }

        public BoardState(int[,] board)
            : base()
        {
            Board = board;
        }

        public BoardState(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }

        public static BoardState InitBoard(int size)
        {
            return new BoardState(new int[size, size]);
        }
    }
}