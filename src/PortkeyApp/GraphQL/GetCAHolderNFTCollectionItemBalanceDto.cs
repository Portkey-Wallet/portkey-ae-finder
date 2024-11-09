using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderNFTCollectionItemBalanceDto : PagedResultRequestDto
{
    public string? ChainId { get; set; }

    [Name("caAddresses")] public List<string?>? CAAddresses { get; set; }

    public string? Symbol { get; set; }
}