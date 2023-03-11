using System.Collections.Generic;
using Libplanet.Action;
using Libplanet.Blockchain.Renderers;
using Libplanet.Blocks;
using Libplanet.Unity;
using Scripts.Actions;
using Scripts.States.Session;
using Serilog.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Libplanet.Blockchain.Policies;
using Scripts.Policy;


namespace Scripts
{
    public class BlockUpdatedEvent : UnityEvent<Block<PolymorphicAction<ActionBase>>>
    {
    }

    public class RoundUpdatedEvent : UnityEvent<SessionState>
    {
    }

    public class Game : MonoBehaviour
    {
        public Text BlockHashText;
        public Text BlockIndexText;
        public Text AddressText;

        public Text RoundText;

        private BlockUpdatedEvent _blockUpdatedEvent;
        private RoundUpdatedEvent _roundUpdatedEvent;
        private IEnumerable<IRenderer<PolymorphicAction<ActionBase>>> _renderers;
        private Agent _agent;

        private UserManager userManager = new UserManager();

        public void Awake()
        {
            userManager.ReadTempUserFile();
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
            SerilogController.WriteSerilog("debug.txt", LogEventLevel.Verbose, "test");

            _blockUpdatedEvent = new BlockUpdatedEvent();
            _blockUpdatedEvent.AddListener(UpdateBlockTexts);
            _roundUpdatedEvent = new RoundUpdatedEvent();
            _roundUpdatedEvent.AddListener(UpdateRoundText);

            _renderers = new List<IRenderer<PolymorphicAction<ActionBase>>>()
            {
                new AnonymousRenderer<PolymorphicAction<ActionBase>>()
                {
                    BlockRenderer = (oldTip, newTip) =>
                    {
                        if (newTip.Index > 0)
                        {
                            _agent.RunOnMainThread(() => _blockUpdatedEvent.Invoke(newTip));
                        }
                    }
                },
                new AnonymousActionRenderer<PolymorphicAction<ActionBase>>()
                {
                    ActionRenderer = (action, context, nextStates) =>
                    {
                        if (nextStates.GetState(context.Signer) is Bencodex.Types.Dictionary bdict)
                        {
                            _agent.RunOnMainThread(() => _roundUpdatedEvent.Invoke(new SessionState(bdict)));
                        }
                    }
                }
            };

            var blockPolicy = new BlockPolicySource().GetPolicy();
            var stagePolicy = new VolatileStagePolicy<PolymorphicAction<ActionBase>>();
            _agent = Agent.AddComponentTo(gameObject, _renderers, blockPolicy, stagePolicy);
        }

        public void Start()
        {
            BlockHashText.text = "Block Hash: 0000";
            BlockIndexText.text = "Block Index: 0";

            AddressText.text = $"My Address: {_agent.Address.ToHex().Substring(0, 4)}";
            Bencodex.Types.IValue initialState = _agent.GetState(_agent.Address);
            Debug.Log($"init state is null: {initialState is null}");
            if (initialState is Bencodex.Types.Dictionary bdict)
            {
                _roundUpdatedEvent.Invoke(new SessionState(bdict));
            }
            else
            {
                _roundUpdatedEvent.Invoke(new SessionState(-1L));
            }
        }

        public void CreateSession()
        {
            List<PolymorphicAction<ActionBase>> actions =
                new List<PolymorphicAction<ActionBase>>()
                {
                    new CreateSessionAction()
                };
            _agent.MakeTransaction(actions);
        }

        private void UpdateBlockTexts(Block<PolymorphicAction<ActionBase>> tip)
        {
            BlockHashText.text = $"Block Hash: {tip.Hash.ToString().Substring(0, 4)}";
            BlockIndexText.text = $"Block Index: {tip.Index}";
        }

        private void UpdateRoundText(SessionState state)
        {
            RoundText.text = $"Current Round: {state.Round} and StartedBlockIndex: {state.StartedBlockIndex}";
        }
    }
}