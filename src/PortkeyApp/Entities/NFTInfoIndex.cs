using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class NFTInfoIndex : TokenInfoBase, IAeFinderEntity
{
    [Text(Index = false)] public string ImageUrl { get; set; }
    
    [Keyword] public string CollectionSymbol { get; set; }
    
    [Keyword] public string CollectionName { get; set; }
    
    [Keyword] public string Traits { get; set; }
    
    [Keyword] public string Lim { get; set; }
    
    [Keyword] public string InscriptionName { get; set; }
    
    [Keyword] public string Generation { get; set; }
    
    [Keyword] public string SeedOwnedSymbol { get; set; }
    
    [Keyword] public string Expires { get; set; }
    public int TraitsLength => Traits == null ? 0 : Traits.Length;
}