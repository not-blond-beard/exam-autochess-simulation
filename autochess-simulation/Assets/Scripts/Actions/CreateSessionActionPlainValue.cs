using Libplanet.Store;

namespace Scripts.Actions
{
    public class CreateSessionActionPlainValue : DataModel
    {
        public string Test { get; private set; }

        public CreateSessionActionPlainValue(string test)
            : base()
        {
            Test = test;
        }

        // Used for deserializing stored action.
        public CreateSessionActionPlainValue(Bencodex.Types.Dictionary encoded)
            : base(encoded)
        {
        }
    }
}
