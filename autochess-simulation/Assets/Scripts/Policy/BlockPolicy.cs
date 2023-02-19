using Libplanet;
using Libplanet.Action;
using Libplanet.Blockchain.Policies;
using Libplanet.Unity;


namespace Scripts.Policy
{
    public class ASBlockPolicy : BlockPolicy<PolymorphicAction<ActionBase>>
    {
        public ASBlockPolicy()
        {

        }
    }
}