using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderTransactionAddressDto : PagedResultRequestDto
{
    public string? ChainId { get; set; }
    
    [Name("caAddressInfos")]
    public List<CAAddressInfo?>? CAAddressInfos { get; set; }
}