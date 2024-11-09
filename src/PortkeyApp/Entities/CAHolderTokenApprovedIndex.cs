using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderTokenApprovedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string CAAddress { get; set; }
    [Keyword] public string Spender { get; set; }
    [Keyword] public string Symbol { get; set; }
    public long BatchApprovedAmount { get; set; }
}