using AElf.EntityMapping.Elasticsearch.Linq;

namespace PortkeyApp.Entities;

[NestedAttributes("TokenTransferInfos")]
public class TokenTransferInfo
{
    public TokenInfoIndex TokenInfo { get; set; }
    
    public NFTInfoIndex NftInfo { get; set; }

    public TransferInfo TransferInfo { get; set; }
}