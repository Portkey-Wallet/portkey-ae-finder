using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class TokenInfoBase: AeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string Symbol { get; set; }

    public TokenType Type { get; set; }
    
    /// <summary>
    /// token contract address
    /// </summary>
    [Keyword] public string TokenContractAddress { get; set; }
    
    public int Decimals { get; set; }
    
    public long Supply { get; set; }
    
    public long TotalSupply { get; set; }

    [Keyword] public string TokenName { get; set; }

    [Keyword] public string Issuer { get; set; }

    public bool IsBurnable { get; set; }

    public int IssueChainId { get; set; }

    public Dictionary<string, string> ExternalInfoDictionary { get; set; } = new();
}

public enum TokenType
{
    Token,
    NFTCollection,
    NFTItem
}