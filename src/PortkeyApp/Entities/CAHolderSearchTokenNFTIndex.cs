using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderSearchTokenNFTIndex: AeFinderEntity, IAeFinderEntity
{
    [Keyword]public string CAAddress { get; set; }
    
    public long Balance { get; set; }
    
    public long TokenId { get; set; }

    public TokenSearchInfo TokenInfo { get; set; }

    public NFTSearchInfo NftInfo { get; set; }
}

public class TokenSearchInfo
{
    [Wildcard]public string Symbol { get; set; }
    public TokenType Type { get; set; }
    [Wildcard]public string TokenContractAddress { get; set; }
    public int Decimals { get; set; }
    public long Supply { get; set; }
    public long TotalSupply { get; set; }
    [Wildcard]public string TokenName { get; set; }
    [Wildcard]public string Issuer { get; set; }
    public bool IsBurnable { get; set; }
    public int IssueChainId { get; set; }
}

public class NFTSearchInfo
{
    [Wildcard]public string CollectionSymbol { get; set; }
    [Wildcard]public string CollectionName { get; set; }
    [Wildcard]public string Symbol { get; set; }
    public TokenType Type { get; set; }
    [Wildcard]public string TokenContractAddress { get; set; }
    public int Decimals { get; set; }
    public long Supply { get; set; }
    public long TotalSupply { get; set; }
    [Wildcard]public string TokenName { get; set; }
    [Wildcard]public string Issuer { get; set; }
    public bool IsBurnable { get; set; }
    public int IssueChainId { get; set; }
    public string ImageUrl { get; set; }
}