using AeFinder.Sdk.Entities;
using AeFinder.Sdk.Processor;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderTransactionIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword]public override string Id { get; set; }
    [Keyword]public string TransactionId { get; set; }
    [Keyword] public string MethodName { get; set; }
    public TokenInfoIndex TokenInfo { get; set; }
    public NFTInfoIndex NftInfo { get; set; }
    public TransactionStatus Status { get; set; }
    public long Timestamp { get; set; }
    public TransferInfo TransferInfo { get; set; }
    
    [Nested(Name = "TokenTransferInfos",Enabled = true,IncludeInParent = true,IncludeInRoot = true)]
    public List<TokenTransferInfo> TokenTransferInfos { get; set; } = new();
    [Keyword]
    public string FromAddress { get; set; }
    [Keyword]
    public string ToContractAddress { get; set; }
    public Dictionary<string, long> TransactionFee { get; set; } = new();
    public int Platform { get; set; }
}

public class TransferInfo
{
    [Keyword]
    public string FromAddress { get; set; }
    [Keyword]
    public string FromCAAddress { get; set; }
    [Keyword]
    public string ToAddress { get; set; }
    public long Amount { get; set; }
    [Keyword]
    public string FromChainId { get; set; }
    [Keyword]
    public string ToChainId { get; set; }
    [Keyword]
    public string TransferTransactionId { get; set; }
}