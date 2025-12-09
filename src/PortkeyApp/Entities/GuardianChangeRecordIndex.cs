using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class GuardianChangeRecordIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string CAHash { get; set; }
    [Keyword] public string CAAddress { get; set; }
    [Keyword] public string ChangeType { get; set; }

    public Guardian Guardian { get; set; }
}