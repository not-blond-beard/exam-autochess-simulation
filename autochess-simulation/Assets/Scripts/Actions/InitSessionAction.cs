using System;
using System.Collections.Immutable;
using Libplanet.Action;
using Libplanet.Unity;
using Scripts.States;
using UnityEngine;

namespace Scripts.Actions
{
    [ActionType("init_session")]
    public class InitSessionAction : ActionBase
    {
        private InitSessionActionPlainValue _plainValue;

        public InitSessionAction(ImmutableList<int> count)
        {
            _plainValue = new InitSessionActionPlainValue(count);
        }

        public override Bencodex.Types.IValue PlainValue => _plainValue.Encode();

        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
            if (plainValue is Bencodex.Types.Dictionary bdict)
            {
                _plainValue = new InitSessionActionPlainValue(bdict);
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

            Debug.LogError($"click_action: PrevCount: {sessionState}");
            return states.SetState(ctx.Signer, sessionState.Encode());
        }
    }
}
