using System;
using Libplanet.Action;
using Libplanet.Blockchain.Policies;
using Libplanet.Unity;

namespace Scripts.Policy
{   

    public class BlockPolicy : BlockPolicy<PolymorphicAction<ActionBase>>
    {
        public BlockPolicy(
            IAction blockAction,
            TimeSpan blockInterval)
        : base(
            blockAction: blockAction,
            blockInterval: blockInterval)
        {
        }
    }
}