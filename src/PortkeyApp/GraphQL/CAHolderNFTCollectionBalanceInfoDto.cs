using GraphQL;

namespace PortkeyApp.GraphQL;

public class CAHolderNFTCollectionBalanceInfoDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    [Name("caAddress")]
    public string CAAddress { get; set; }
    public List<long?>? TokenIds { get; set; }
    public NFTCollectionDto NftCollectionInfo { get; set; }
}
