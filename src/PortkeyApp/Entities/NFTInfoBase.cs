using AeFinder.Sdk.Entities;
using Nest;

namespace PortkeyApp.Entities;

public class NFTInfoBase : AeFinderEntity
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string ProtocolName { get; set; }

    [Keyword] public string Symbol { get; set; }

    public long TokenId { get; set; }

    /// <summary>
    /// NFT contract address
    /// </summary>
    [Keyword]
    public string NftContractAddress { get; set; }

    [Keyword] public string Owner { get; set; }

    [Keyword] public string Minter { get; set; }

    public long Quantity { get; set; }

    [Keyword] public string Alias { get; set; }

    [Keyword] public string BaseUri { get; set; }

    [Keyword] public string Uri { get; set; }

    [Keyword] public string Creator { get; set; }

    [Keyword] public string NftType { get; set; }

    public long TotalQuantity { get; set; }

    [Keyword] public string TokenHash { get; set; }

    [Text(Index = false)] public string ImageUrl { get; set; }
}