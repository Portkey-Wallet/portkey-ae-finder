using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class TokenInfoIndex : TokenInfoBase, IAeFinderEntity
{
    [Wildcard] public string SymbolSearch { get; set; }
}

