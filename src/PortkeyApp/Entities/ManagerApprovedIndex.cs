using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class ManagerApprovedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string Symbol { get; set; }
    [Keyword] public string CaHash { get; set; }
    [Keyword] public string Spender { get; set; }
    public long Amount { get; set; }
}