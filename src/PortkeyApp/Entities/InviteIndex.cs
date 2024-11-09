using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class InviteIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string CaHash { get; set; }
    [Keyword] public string MethodName { get; set; }
    [Keyword] public string ReferralCode { get; set; }
    [Keyword] public string ProjectCode { get; set; }
    public long Timestamp { get; set; }
}