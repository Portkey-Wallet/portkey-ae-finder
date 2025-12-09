using PortkeyApp.Entities;

namespace PortkeyApp.Configs;

public class InitialInfo
{
    public List<TokenInitInfo> TokenInfoList { get; set; } = new();
    public List<NFTProtocolInitInfo> NFTProtocolInfoList { get; set; } = new ();
}

public class NFTProtocolInitInfo
{
    public string ChainId { get; set; }
    public string BlockHash { get; set; }
    public long BlockHeight { get; set; }
    public string TokenName { get; set; }
    public string Symbol { get; set; }
    public string Creator { get; set; }
    public string NftType { get; set; }
    public string ProtocolName { get; set; }
    public string BaseUri { get; set; }
    public bool IsTokenIdReuse { get; set; }
    public long Supply { get; set; }
    public long TotalSupply { get; set; }
    public int IssueChainId { get; set; }
    public bool IsBurnable { get; set; }
    public string ImageUrl { get; set; }
}

public class TokenInitInfo
{
    public string ChainId { get; set; }
    public string BlockHash { get; set; }
    public long BlockHeight { get; set; }
    public string Symbol { get; set; }
    public TokenType Type { get; set; }
    public string TokenContractAddress { get; set; }
    public int Decimals { get; set; }
    public long Supply { get; set; }
    public long TotalSupply { get; set; }
    public string TokenName { get; set; }
    public string Issuer { get; set; }
    public bool IsBurnable { get; set; }
    public int IssueChainId { get; set; }
    public Dictionary<string, string> ExternalInfoDictionary { get; set; } = new();
}