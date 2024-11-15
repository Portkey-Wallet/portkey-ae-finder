using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class NFTCollectionInfoIndex : TokenInfoBase, IAeFinderEntity
{
    [Text(Index = false)] public string ImageUrl { get; set; }
    [Keyword] public string InscriptionName { get; set; } 
    
    [Keyword] public string Generation { get; set; }
    
    [Keyword] public string Lim { get; set; }
}