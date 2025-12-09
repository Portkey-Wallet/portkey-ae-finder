using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class LoginGuardianChangeRecordIndex : LoginGuardianBase, IAeFinderEntity
{
    [Keyword] public string ChangeType { get; set; }
}