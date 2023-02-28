using System.Collections.Immutable;
using Libplanet;
using Libplanet.Store;

namespace Scripts.States.Session
{
    public class AllSessionState : DataModel
    {
        public static readonly Address Address = Addresses.AllState;
        public ImmutableList<Address> Sessions { get; private set; }

        public AllSessionState()
            : base()
        {
            Sessions = ImmutableList<Address>.Empty;
        }

        public AllSessionState(ImmutableList<Address> sessions)
            : base()
        {
            Sessions = sessions;
        }

        public AllSessionState(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }
        
        public AllSessionState AddSession(Address address)
        {
            return new AllSessionState(Sessions.Add(address));
        }
    }
}