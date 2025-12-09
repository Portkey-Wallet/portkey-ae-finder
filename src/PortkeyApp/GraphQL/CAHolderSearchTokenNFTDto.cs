using GraphQL;

namespace PortkeyApp.GraphQL;

public class CAHolderSearchTokenNFTDto
{
    public string ChainId { get; set; }
    [Name("caAddress")]
    public string CAAddress { get; set; }
    public long Balance { get; set; }
    public long TokenId { get; set; }
    public TokenInfoDto? TokenInfo { get; set; }
    public NFTItemInfoDto? NftInfo { get; set; }
}