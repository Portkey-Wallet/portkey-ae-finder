using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class LoginGuardianBase : AeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string CAHash { get; set; }
    [Keyword] public string CAAddress { get; set; }
    [Keyword] public string Manager { get; set; }
    public Guardian LoginGuardian { get; set; }
}