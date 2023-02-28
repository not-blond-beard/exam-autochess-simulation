using System.Collections.Immutable;
using Libplanet;
using Libplanet.Store;

namespace Scripts.States.Session
{
    public class SessionState : DataModel
    {
        public ImmutableList<Address> Boards { get; private set; }
        public int Round { get; private set; }
        public long StartedBlockIndex { get; private set; }

        public SessionState(long startedBlockIndex)
            : base()
        {
            Boards = ImmutableList<Address>.Empty;
            Round = 1;
            StartedBlockIndex = startedBlockIndex;
        }

        public SessionState(ImmutableList<Address> boards, int round, long startedBlockIndex)
            : base()
        {
            Boards = boards;
            Round = round;
            StartedBlockIndex = startedBlockIndex;
        }

        public SessionState(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }

        public void JoinSession(Address address)
        {
            Boards.Add(address);
        }

        public void Next()
        {
            Round++;
        }
    }
}