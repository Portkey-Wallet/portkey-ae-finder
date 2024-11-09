using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class TransactionFeeChangedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string CAAddress { get; set; }
    [Keyword] public string Symbol { get; set; }
    public long Amount { get; set; }
    [Keyword] public string TransactionId { get; set; }
    [Keyword] public string ConsumerAddress { get; set; }
}