using Libplanet.Store;
using System.Collections.Immutable;

namespace Scripts.Actions
{
    public class ClickActionPlainValue : DataModel
    {
        public ImmutableList<int> Count { get; private set; }

        public ClickActionPlainValue(ImmutableList<int> count)
            : base()
        {
            Count = count;
        }

        // Used for deserializing stored action.
        public ClickActionPlainValue(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }
    }
}