using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderTokenBalanceIndex :AeFinderEntity, IAeFinderEntity
{
    [Keyword]
    public string CAAddress { get; set; }

    public TokenInfoIndex TokenInfo { get; set; }
    
    public long Balance { get; set; }
}