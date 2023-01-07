using Libplanet.Action;
using Libplanet.Unity;
using Scripts.States;
using UnityEngine;

namespace Scripts.Actions
{
    [ActionType("create_session")]
    public class CreateSessionAction : ActionBase
    {
        public CreateSessionAction()
        {
        }

        public override Bencodex.Types.IValue PlainValue => PlainValue;

        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
        }

        public override IAccountStateDelta Execute(IActionContext ctx)
        {
            IAccountStateDelta states = ctx.PreviousStates;

            SessionState sessionState =
                states.GetState(ctx.Signer) is Bencodex.Types.Dictionary sessionStateEncoded
                    ? new SessionState(sessionStateEncoded)
                    : new SessionState(ctx.BlockIndex);

            Debug.LogError($"create session: Started Block Index: {sessionState.StartedBlockIndex}");
            return states.SetState(ctx.Signer, sessionState.Encode());
        }
    }
}
