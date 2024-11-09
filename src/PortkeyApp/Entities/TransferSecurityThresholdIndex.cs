using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class TransferSecurityThresholdIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string Symbol { get; set; }
    public long BalanceThreshold { get; set; }
    public long GuardianThreshold { get; set; }
}