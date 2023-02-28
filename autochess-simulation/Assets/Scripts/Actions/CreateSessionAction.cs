using System;
using Libplanet.Action;
using Libplanet.Store;
using Libplanet.Unity;
using Scripts.States.Session;
using UnityEngine;

namespace Scripts.Actions
{
    [ActionType("create_session")]
    public class CreateSessionAction : ActionBase
    {

        class ActionPlainValue : DataModel
        {

            public ActionPlainValue()
                : base()
            {
            }


            public ActionPlainValue(Bencodex.Types.Dictionary encoded)
                : base(encoded)
            {
            }
        }

        private ActionPlainValue _plainValue;

        public CreateSessionAction()
        {
            _plainValue = new ActionPlainValue();
        }

        public override Bencodex.Types.IValue PlainValue => _plainValue.Encode();

        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
            if (plainValue is Bencodex.Types.Dictionary bdict)
            {
                _plainValue = new ActionPlainValue(bdict);
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

            AllSessionState allSessionState =
                states.GetState(AllSessionState.Address) is Bencodex.Types.Dictionary allSessionStateEncoded
                    ? new AllSessionState(allSessionStateEncoded)
                    : new AllSessionState();

            if (!allSessionState.Sessions.Contains(ctx.Signer))
            {
                states = states.SetState(AllSessionState.Address, allSessionState.AddSession(ctx.Signer).Encode());
            }

            return states
                .SetState(ctx.Signer, sessionState.Encode());
        }
    }
}
