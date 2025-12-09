using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderTransactionAddressIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword]
    public string CAAddress { get; set; }
    
    [Keyword]
    public string Address { get; set; }
    
    [Keyword]
    public string AddressChainId { get; set; }
    
    /// <summary>
    /// Latest transaction time
    /// </summary>
    public long TransactionTime { get; set; }
}