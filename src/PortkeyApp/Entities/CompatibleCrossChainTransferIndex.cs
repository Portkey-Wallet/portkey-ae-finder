using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CompatibleCrossChainTransferIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string TransactionId { get; set; }
    public long Timestamp { get; set; }
    [Keyword]  public string FromAddress { get; set; }
    [Keyword]  public string ToAddress { get; set; }
}