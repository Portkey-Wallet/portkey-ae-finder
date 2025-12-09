using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class TokenInfoIndex : TokenInfoBase, IAeFinderEntity
{
    [Text(Index = false)] public string ImageUrl { get; set; }
    [Wildcard] public string SymbolSearch { get; set; }
}

