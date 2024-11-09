using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class CAHolderManagerIndex:  AeFinderEntity, IAeFinderEntity
{
    [Keyword]public override string Id { get; set; }
    
    [Keyword]public string Manager { get; set; }
    
    public List<string> CAAddresses { get; set; }
    
    public int Platform { get; set; }
}