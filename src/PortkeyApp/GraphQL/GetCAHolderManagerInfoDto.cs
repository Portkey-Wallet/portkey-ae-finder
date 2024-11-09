using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderManagerInfoDto: PagedResultRequestDto
{
    public string? ChainId { get; set; }
    
    [Name("caHash")]
    public string? CAHash { get; set; }
    
    [Name("caAddresses")]
    public List<string?>? CAAddresses { get; set; }
    
    public string? Manager { get; set; }
}