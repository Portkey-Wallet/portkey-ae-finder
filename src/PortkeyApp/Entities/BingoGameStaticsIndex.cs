using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class BingoGameStaticsIndex : AeFinderEntity, IAeFinderEntity
{
    public long Amount { get; set; }
    public long Award { get; set; }
    [Keyword] public string PlayerAddress { get; set; }
    public long TotalWins { get; set; }
    public long TotalPlays { get; set; }
}