using AeFinder.Sdk.Processor;
using PortkeyApp.Entities;

namespace PortkeyApp.GraphQL;

public class CAHolderTransactionDto
{
    public string Id { get; set; }
    
    public string? ChainId { get; set; }

    public string? BlockHash { get; set; }

    public long? BlockHeight { get; set; }

    public string? PreviousBlockHash { get; set; }
    
    public string TransactionId { get; set; }

    public string MethodName { get; set; }
      
    public TokenInfoDto? TokenInfo { get; set; }
    
    public NFTItemInfoDto? NftInfo { get; set; }
    
    public TransactionStatus Status { get; set; }

    public long Timestamp { get; set; }

    public TransferInfo? TransferInfo { get; set; }
    
    public List<TokenTransferInfoDto>? TokenTransferInfos { get; set; }
    
    public string FromAddress { get; set; }

    public string? ToContractAddress { get; set; }

    public List<TransactionFee>? TransactionFees { get; set; }
    public bool IsManagerConsumer { get; set; } = false;
    
    public int? Platform { get; set; }
}

public class TokenTransferInfoDto
{
    public TokenInfoDto? TokenInfo { get; set; }
    
    public NFTItemInfoDto? NftInfo { get; set; }

    public TransferInfo? TransferInfo { get; set; }
}

public class TransactionFee
{
    public string? Symbol { get; set; }
    
    public long? Amount { get; set; }
}