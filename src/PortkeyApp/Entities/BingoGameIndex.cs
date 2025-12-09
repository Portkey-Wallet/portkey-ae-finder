using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class BingoGameIndex : AeFinderEntity, IAeFinderEntity
{
    public long PlayBlockHeight { get; set; }
    public long BingoBlockHeight { get; set; }
    [Keyword] public string PlayBlockHash { get; set; }
    [Keyword] public string BingoBlockHash { get; set; }
    public long Amount { get; set; }
    public long Award { get; set; }
    public bool IsComplete { get; set; }
    [Keyword] public string PlayId { get; set; }
    [Keyword] public string BingoId { get; set; }
    public int BingoType { get; set; }
    public List<int> Dices { get; set; }
    [Keyword] public string PlayerAddress { get; set; }
    public long PlayTime { get; set; }
    public long BingoTime { get; set; }
    public List<TransactionFee> PlayTransactionFee { get; set; }
    public List<TransactionFee> BingoTransactionFee { get; set; }
}