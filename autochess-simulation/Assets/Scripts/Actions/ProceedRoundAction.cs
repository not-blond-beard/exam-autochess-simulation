using System;
using Libplanet.Action;
using Libplanet.Store;
using Libplanet.Unity;
using Scripts.States.Session;
using UnityEngine;

namespace Scripts.Actions
{
    [ActionType("proceed_round")]
    public class ProceedRoundAction : ActionBase
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

        public ProceedRoundAction()
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

            AllSessionState allSessionState =
                states.GetState(AllSessionState.Address) is Bencodex.Types.Dictionary allSessionStateEncoded
                    ? new AllSessionState(allSessionStateEncoded)
                    : new AllSessionState();

            Debug.LogError($"All session: Count: {allSessionState.Sessions.Count}");

            // 모든 세션을 Get 하는 코드는 엄청난 부하를 주지만 당장은 전부 가져옵니다.
            foreach (var address in allSessionState.Sessions)
            {
                SessionState sessionState =
                    states.GetState(address) is Bencodex.Types.Dictionary sessionStateEncoded
                        ? new SessionState(sessionStateEncoded)
                        : throw new Exception($"[temp] session not found, {address}");

                sessionState.Next();
                Debug.LogError($"Next Round, Address: {address}");
                states = states.SetState(ctx.Signer, sessionState.Encode());
            }

            return states;
        }
    }
}
