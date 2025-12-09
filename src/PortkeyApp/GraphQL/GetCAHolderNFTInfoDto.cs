using GraphQL;

namespace PortkeyApp.GraphQL;

public class GetCAHolderNFTInfoDto: PagedResultRequestDto
{
    public string? ChainId { get; set; }
    public string? Symbol { get; set; }
    [Name("caAddressInfos")]
    public List<CAAddressInfo?>? CAAddressInfos { get; set; }
    public string? CollectionSymbol { get; set; }
    
}