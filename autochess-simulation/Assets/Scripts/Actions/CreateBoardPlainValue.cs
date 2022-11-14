using Libplanet.Store;

namespace Scripts.Actions
{
    public class CreateBoardActionPlainValue : DataModel
    {
        public int[,] Count { get; private set; }

        public CreateBoardActionPlainValue(int[,] count)
            : base()
        {
            Count = count;
        }

        // Used for deserializing stored action.
        public CreateBoardActionPlainValue(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }
    }
}