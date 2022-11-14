using System;
using Libplanet.Action;
using Libplanet.Unity;
using Scripts.States;
using UnityEngine;

namespace Scripts.Actions
{
    [ActionType("create_board_action")]
    public class CreateBoard : ActionBase
    {
        private CreateBoardActionPlainValue _plainValue;

        public CreateBoard()
        {
        }

        public CreateBoard(int[,] board)
        {
            _plainValue = new CreateBoardActionPlainValue(board);
        }

        public override Bencodex.Types.IValue PlainValue => _plainValue.Encode();

        public override void LoadPlainValue(Bencodex.Types.IValue plainValue)
        {
            if (plainValue is Bencodex.Types.Dictionary bdict)
            {
                _plainValue = new CreateBoardActionPlainValue(bdict);
            }
            else
            {
                throw new ArgumentException(
                    $"Invalid {nameof(plainValue)} type: {plainValue.GetType()}");
            }
        }

        public override IAccountStateDelta Execute(IActionContext context)
        {
            // 이전 상태 불러오기
            IAccountStateDelta states = context.PreviousStates;
            BoardState boardState =
                states.GetState(context.Signer) is Bencodex.Types.Dictionary boardStateEncoded
                    ? new BoardState(boardStateEncoded)
                    : BoardState.InitBoard(8);

            Debug.Log($"['create_board_action'] Create new board: {boardState.Board}");
            return states.SetState(context.Signer, boardState.Encode());
        }
    }
}