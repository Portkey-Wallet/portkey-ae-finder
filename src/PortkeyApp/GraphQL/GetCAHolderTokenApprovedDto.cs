using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderTokenApprovedDto : PagedResultRequestDto
{
    public string? ChainId { get; set; }
    
    [Name("caAddresses")] public List<string?>? CAAddresses { get; set; }
}