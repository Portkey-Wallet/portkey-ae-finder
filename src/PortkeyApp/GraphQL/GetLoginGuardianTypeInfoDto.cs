using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetLoginGuardianInfoDto : PagedResultRequestDto
{
    public string? ChainId { get; set; }
    
    [Name("caHash")]
    public string? CAHash { get; set; }
    
    [Name("caAddress")]
    public string? CAAddress { get; set; }
    
    public string? LoginGuardian { get; set; }
}