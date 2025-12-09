using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class NFTProtocolInfoBase : AeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string Symbol { get; set; }
    
    [Keyword] public string Creator { get; set; }
    
    [Keyword] public string NftType { get; set; }

    [Keyword] public string ProtocolName { get; set; }
    
    [Keyword] public string BaseUri { get; set; }
    
    public bool IsTokenIdReuse { get; set; }
    
    public long Supply { get; set; }
    
    public long TotalSupply { get; set; }
    
    public int IssueChainId { get; set; }
    
    public bool IsBurnable { get; set; }
  
    [Text(Index = false)] public string ImageUrl { get; set; }
}