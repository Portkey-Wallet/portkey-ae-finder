using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderNFTBalanceIndex : AeFinderEntity, IAeFinderEntity
{
    public NFTInfoIndex NftInfo { get; set; }
    [Keyword] public string CAAddress { get; set; }

    [Keyword] public long Balance { get; set; }
}