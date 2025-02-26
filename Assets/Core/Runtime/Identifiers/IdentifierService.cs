namespace Redpenguin.Core.Identifiers
{
    public class IdentifierService : IIdentifierService
    {
        private int _lastId = 1;

        public int Next() =>
            ++_lastId;

        public void Reset() =>
            _lastId = 1;
    }
}