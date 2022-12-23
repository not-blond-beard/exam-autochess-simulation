using System.Collections.Immutable;
using Libplanet.Store;

namespace Scripts.Actions
{
    public class InitSessionActionPlainValue : DataModel
    {
        public ImmutableList<int> Count { get; private set; }

        public InitSessionActionPlainValue(ImmutableList<int> count)
            : base()
        {
            Count = count;
        }

        public InitSessionActionPlainValue(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }
    }
}