using System;
using Libplanet.Action;
using Libplanet.Blockchain.Policies;
using Libplanet.Unity;
using Scripts.Actions;


namespace Scripts.Policy
{
    public partial class BlockPolicySource
    {
        public static readonly TimeSpan BlockInterval = TimeSpan.FromSeconds(15);

        public IBlockPolicy<PolymorphicAction<ActionBase>> GetPolicy()
        {
            return new BlockPolicy(new ProceedRoundAction(), BlockInterval);
        }
    }
}
