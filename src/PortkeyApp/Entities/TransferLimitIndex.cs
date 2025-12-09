using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class TransferLimitIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string CaHash { get; set; }
    [Keyword] public string Symbol { get; set; }
    public long SingleLimit { get; set; }
    public long DailyLimit { get; set; }
}