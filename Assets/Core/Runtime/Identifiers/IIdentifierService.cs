namespace Redpenguin.Core.Identifiers
{
    public interface IIdentifierService
    {
        int Next();
        void Reset();
    }
}