using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderTokenBalanceDto : PagedResultRequestDto
{
    public string? ChainId { get; set; }
    [Name("caAddressInfos")]
    public List<CAAddressInfo?>? CAAddressInfos { get; set; }
    public string? Symbol { get; set; }
}