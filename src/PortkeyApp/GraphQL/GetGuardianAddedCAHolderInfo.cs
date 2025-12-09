using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetGuardianAddedCAHolderInfo : PagedResultRequestDto
{
    [Name("loginGuardianIdentifierHash")] public string? LoginGuardianIdentifierHash { get; set; }
}