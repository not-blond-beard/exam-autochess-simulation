using System.Collections.Immutable;
using Libplanet.Store;

namespace Scripts.States
{
    public class CountState : DataModel
    {
        public ImmutableList<int> Count { get; private set; }

        // Used for creating a new state.
        public CountState()
            : base()
        {
            Count = ImmutableList<int>.Empty;
        }

        public CountState(ImmutableList<int> count)
            : base()
        {
            Count = count;
        }

        // Used for deserializing a stored state.
        // This must be declared as base constructor cannot be inherited.
        public CountState(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }

        // Used for adding `count` to the current state.
        public static CountState InitState()
        {
            var empty = ImmutableList<int>.Empty.Add(0).Add(0).Add(0);
            return new CountState(empty);
        }
    }
}