using System;
using Libplanet.Action;
using Libplanet.Unity;
using Scripts.States;
using UnityEngine;

namespace Scripts.Actions
{
    [ActionType("create_session")]
    public class CreateSessionAction : ActionBase
    {
        private CreateSessionActionPlainValue _plainValue;

        public CreateSessionAction()
        {
        }

        // Used for creating a new action.
        public CreateSessionAction(string test)
        {
            _plainValue = new CreateSessionActionPlainValue(test);
        }

        public override Bencodex.Types.IValue PlainValue => _plainValue.Encode();

        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
            if (plainValue is Bencodex.Types.Dictionary bdict)
            {
                _plainValue = new CreateSessionActionPlainValue(bdict);
            }
            else
            {
                throw new ArgumentException(
                    $"Invalid {nameof(plainValue)} type: {plainValue.GetType()}");
            }
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
