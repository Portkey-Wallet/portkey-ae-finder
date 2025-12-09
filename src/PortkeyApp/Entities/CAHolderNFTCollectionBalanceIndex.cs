using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderNFTCollectionBalanceIndex : AeFinderEntity, IAeFinderEntity
{
    public NFTCollectionInfoIndex NftCollectionInfo { get; set; }
    [Keyword] public string CAAddress { get; set; }

    public long Balance { get; set; }
    public List<long> TokenIds { get; set; }
    public int TokenIdCount => TokenIds == null ? 0 : TokenIds.Count;
}