using System;
using Libplanet.Action;
using Libplanet.Unity;
using Scripts.States;
using UnityEngine;

namespace Scripts.Actions
{
    // Used for reflection when deserializing a stored action.
    [ActionType("click_action")]
    public class ClickAction : ActionBase
    {
        private ClickActionPlainValue _plainValue;

        // Used for reflection when deserializing a stored action.
        public ClickAction()
        {
        }

        // Used for creating a new action.
        public ClickAction(long count)
        {
            _plainValue = new ClickActionPlainValue(count);
        }

        // Used for serialzing an action.
        public override Bencodex.Types.IValue PlainValue => _plainValue.Encode();

        // Used for deserializing a stored action.
        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
            if (plainValue is Bencodex.Types.Dictionary bdict)
            {
                _plainValue = new ClickActionPlainValue(bdict);
            }
            else
            {
                throw new ArgumentException(
                    $"Invalid {nameof(plainValue)} type: {plainValue.GetType()}");
            }
        }

        // Executes an action.
        // This is what gets called when a block containing an action is mined
        // or appended to a blockchain.
        public override IAccountStateDelta Execute(IActionContext context)
        {
            // Retrieves the previously stored state.
            IAccountStateDelta states = context.PreviousStates;
            CountState countState =
                states.GetState(context.Signer) is Bencodex.Types.Dictionary countStateEncoded
                    ? new CountState(countStateEncoded)
                    : new CountState(0L);

            // Mutates the loaded state, logs the result, and stores the resulting state.
            long prevCount = countState.Count;
            countState = countState.AddCount(_plainValue.Count);
            long nextCount = countState.Count;
            Debug.Log($"click_action: PrevCount: {prevCount}, NextCount: {nextCount}");
            return states.SetState(context.Signer, countState.Encode());
        }
    }
}