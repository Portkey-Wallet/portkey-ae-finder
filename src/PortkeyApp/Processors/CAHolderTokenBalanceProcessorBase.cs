using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using AElf.CSharp.Core;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public abstract class CAHolderTokenBalanceProcessorBase<TEvent> : CAHolderTransactionProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected async Task ModifyBalanceAsync(string address, string symbol, long amount, LogEventContext context)
    {
        var tokenType = TokenHelper.GetTokenType(symbol);
        if (tokenType == TokenType.Token)
        {
            var tokenInfoIndex = await GetEntityAsync<TokenInfoIndex>(IdGenerateHelper.GetId(context.ChainId, symbol));
            if (tokenInfoIndex == null)
            {
                tokenInfoIndex = new TokenInfoIndex
                {
                    Id = IdGenerateHelper.GetId(context.ChainId, symbol),
                    TokenContractAddress = GetContractAddress(context.ChainId),
                    Type = TokenType.Token,
                    Symbol = symbol
                };
                await UpdateTokenInfoFromChainAsync(context.ChainId, tokenInfoIndex);
            }

            var id = IdGenerateHelper.GetId(context.ChainId, address, symbol);
            var tokenBalance = await GetEntityAsync<CAHolderTokenBalanceIndex>(id);
            if (tokenBalance == null)
            {
                tokenBalance = new CAHolderTokenBalanceIndex
                {
                    Id = id,
                    TokenInfo = tokenInfoIndex,
                    CAAddress = address
                };
            }

            if (tokenBalance.TokenInfo == null)
            {
                tokenBalance.TokenInfo = tokenInfoIndex;
            }

            tokenBalance.Balance += amount;
            //ObjectMapper.Map(context, tokenBalance);

            await SaveEntityAsync(tokenBalance);
        }

        NFTCollectionInfoIndex nftCollectionInfo = null;
        if (tokenType == TokenType.NFTCollection || tokenType == TokenType.NFTItem)
        {
            var nftCollectionSymbol = TokenHelper.GetNFTCollectionSymbol(symbol);
            var nftCollectionInfoId = IdGenerateHelper.GetId(context.ChainId, nftCollectionSymbol);
            nftCollectionInfo =
                await GetEntityAsync<NFTCollectionInfoIndex>(nftCollectionInfoId);
            if (nftCollectionInfo == null)
            {
                nftCollectionInfo = new NFTCollectionInfoIndex
                {
                    Id = nftCollectionInfoId,
                    Symbol = nftCollectionSymbol,
                    Type = TokenType.NFTCollection,
                    TokenContractAddress = GetContractAddress(context.ChainId)
                };
                //ObjectMapper.Map(context, nftCollectionInfo);
                await UpdateCollectionInfoFromChainAsync(context.ChainId, nftCollectionInfo);
            }
        }

        if (tokenType == TokenType.NFTCollection)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, address, symbol);
            var collectionBalance =
                await GetEntityAsync<CAHolderNFTCollectionBalanceIndex>(id);
            if (collectionBalance == null)
            {
                collectionBalance = new CAHolderNFTCollectionBalanceIndex
                {
                    Id = id,
                    NftCollectionInfo = nftCollectionInfo,
                    CAAddress = address,
                    TokenIds = new List<long>() { }
                };
            }

            collectionBalance.Balance += amount;
            await SaveEntityAsync(collectionBalance);
        }

        if (tokenType == TokenType.NFTItem)
        {
            var nftInfoId = IdGenerateHelper.GetId(context.ChainId, symbol);
            var nftInfo = await GetEntityAsync<NFTInfoIndex>(nftInfoId);
            if (nftInfo == null)
            {
                nftInfo = new NFTInfoIndex()
                {
                    Id = nftInfoId,
                    Symbol = symbol,
                    Type = TokenType.NFTItem,
                    TokenContractAddress = GetContractAddress(context.ChainId),
                    CollectionName = nftCollectionInfo.TokenName,
                    CollectionSymbol = nftCollectionInfo.Symbol
                };
                await UpdateNftInfoFromChainAsync(context.ChainId, nftInfo);
            }
            else if (string.IsNullOrWhiteSpace(nftInfo.CollectionSymbol))
            {
                nftInfo.CollectionName = nftCollectionInfo.TokenName;
                nftInfo.CollectionSymbol = nftCollectionInfo.Symbol;
                await SaveEntityAsync(nftInfo);
            }

            if (ConfigConstants.Inscriptions.Contains(nftInfo.CollectionSymbol) &&
                nftInfo.InscriptionName.IsNullOrWhiteSpace())
            {
                if (nftCollectionInfo.InscriptionName.IsNullOrWhiteSpace())
                {
                    //ObjectMapper.Map(context, nftCollectionInfo);
                    await UpdateCollectionInfoFromChainAsync(context.ChainId, nftCollectionInfo);
                }

                nftInfo.InscriptionName = nftCollectionInfo.InscriptionName;
                nftInfo.Lim = nftCollectionInfo.Lim;
                await SaveEntityAsync(nftInfo);
            }

            var id = IdGenerateHelper.GetId(context.ChainId, address, symbol);
            var nftBalance = await GetEntityAsync<CAHolderNFTBalanceIndex>(id);
            if (nftBalance == null)
            {
                nftBalance = new CAHolderNFTBalanceIndex
                {
                    Id = id,
                    CAAddress = address
                };
            }

            if (nftBalance.NftInfo == null)
            {
                nftBalance.NftInfo = nftInfo;
            }

            if (string.IsNullOrWhiteSpace(nftBalance.NftInfo.CollectionSymbol))
            {
                nftBalance.NftInfo.CollectionSymbol = nftCollectionInfo.Symbol;
                nftBalance.NftInfo.CollectionName = nftCollectionInfo.TokenName;
            }

            nftBalance.Balance += amount;
            await SaveEntityAsync(nftBalance);

            var nftItemId = TokenHelper.GetNFTItemId(symbol);
            var collectionBalanceIndexId = IdGenerateHelper.GetId(context.ChainId, address, nftCollectionInfo.Symbol);
            var collectionBalance =
                await GetEntityAsync<CAHolderNFTCollectionBalanceIndex>(collectionBalanceIndexId);
            if (collectionBalance == null)
            {
                collectionBalance = new CAHolderNFTCollectionBalanceIndex
                {
                    Id = collectionBalanceIndexId,
                    CAAddress = address,
                    TokenIds = new List<long>() { nftItemId }
                };
            }

            if (collectionBalance.NftCollectionInfo == null)
            {
                collectionBalance.NftCollectionInfo = nftCollectionInfo;
            }

            if (collectionBalance.TokenIds == null)
            {
                collectionBalance.TokenIds = new List<long>() { };
            }

            if (amount < 0)
            {
                if (nftBalance.Balance == 0 &&
                    collectionBalance.TokenIds.Contains(nftItemId))
                {
                    collectionBalance.TokenIds.Remove(nftItemId);
                }
            }
            else
            {
                if (!collectionBalance.TokenIds.Contains(nftItemId))
                {
                    collectionBalance.TokenIds.Add(nftItemId);
                }
            }

            await SaveEntityAsync(collectionBalance);
        }

        //Update Search index Balance
        await ModifySearchBalanceAsync(address, symbol, amount, context);
    }

    private async Task UpdateTokenInfoFromChainAsync(string chainId, TokenInfoIndex tokenInfoIndex)
    {
        var tokenFromChain = await BlockChainService.ViewContractAsync<AElf.Contracts.MultiToken.TokenInfo>(
            chainId,
            ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress,
            "GetTokenInfo",
            new GetTokenInfoInput
            {
                Symbol = tokenInfoIndex.Symbol
            });
        if (tokenFromChain.Symbol == tokenInfoIndex.Symbol)
        {
            ObjectMapper.Map(tokenFromChain, tokenInfoIndex);
            if (tokenFromChain.ExternalInfo != null && tokenFromChain.ExternalInfo.Value is { Count: > 0 })
            {
                tokenInfoIndex.ExternalInfoDictionary = tokenFromChain.ExternalInfo.Value
                    .Where(t => !t.Key.IsNullOrWhiteSpace())
                    .ToDictionary(item => item.Key, item => item.Value);
            }

            tokenInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
            await SaveEntityAsync(tokenInfoIndex);
        }
    }

    private async Task UpdateCollectionInfoFromChainAsync(string chainId, NFTCollectionInfoIndex collectionInfoIndex)
    {
        var collectionInfo = await BlockChainService.ViewContractAsync<AElf.Contracts.MultiToken.TokenInfo>(
            chainId,
            ConfigConstants.ContractInfos.First(c => c.ChainId == chainId)
                .TokenContractAddress, "GetTokenInfo",
            new GetTokenInfoInput
            {
                Symbol = collectionInfoIndex.Symbol
            });
        if (collectionInfo.Symbol == collectionInfoIndex.Symbol)
        {
            ObjectMapper.Map(collectionInfo, collectionInfoIndex);
            if (collectionInfo.ExternalInfo != null && collectionInfo.ExternalInfo.Value is { Count: > 0 })
            {
                var externalDictionary = collectionInfo.ExternalInfo.Value;
                var externalInfo = NftExternalInfoHelper.BuildNftExternalInfo(externalDictionary);
                ObjectMapper.Map(externalInfo, collectionInfoIndex);
            }

            collectionInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
            await SaveEntityAsync(collectionInfoIndex);
        }
    }

    private async Task UpdateNftInfoFromChainAsync(string chainId, NFTInfoIndex nftInfoIndex)
    {
        var nftInfo = await BlockChainService.ViewContractAsync<AElf.Contracts.MultiToken.TokenInfo>(
            chainId,
            ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress,
            "GetTokenInfo",
            new GetTokenInfoInput
            {
                Symbol = nftInfoIndex.Symbol
            });
        if (nftInfo.Symbol == nftInfoIndex.Symbol)
        {
            ObjectMapper.Map(nftInfo, nftInfoIndex);
            if (nftInfo.ExternalInfo != null && nftInfo.ExternalInfo.Value is { Count: > 0 })
            {
                var externalDictionary = nftInfo.ExternalInfo;
                var externalInfo = NftExternalInfoHelper.BuildNftExternalInfo(externalDictionary.Value);
                ObjectMapper.Map(externalInfo, nftInfoIndex);
            }

            nftInfoIndex.ExternalInfoDictionary ??= new Dictionary<string, string>();
            await SaveEntityAsync(nftInfoIndex);
        }
    }

    private async Task ModifySearchBalanceAsync(string address, string symbol, long amount, LogEventContext context)
    {
        var tokenType = TokenHelper.GetTokenType(symbol);
        if (tokenType == TokenType.NFTCollection)
        {
            return;
        }

        if (tokenType == TokenType.Token)
        {
            //get token info from token index
            var tokenInfoIndex =
                await GetEntityAsync<TokenInfoIndex>(IdGenerateHelper.GetId(context.ChainId, symbol));

            var id = IdGenerateHelper.GetId(context.ChainId, address, symbol);
            var caHolderSearchTokenNFTIndex = await GetEntityAsync<CAHolderSearchTokenNFTIndex>(id);
            if (caHolderSearchTokenNFTIndex != null)
            {
                caHolderSearchTokenNFTIndex.Balance += amount;
            }
            else
            {
                caHolderSearchTokenNFTIndex = new CAHolderSearchTokenNFTIndex()
                {
                    Id = IdGenerateHelper.GetId(context.ChainId, address, symbol),
                    CAAddress = address,
                    Balance = amount,
                    TokenInfo = ObjectMapper.Map<TokenInfoIndex, TokenSearchInfo>(tokenInfoIndex)
                };
            }

            await SaveEntityAsync(caHolderSearchTokenNFTIndex);
        }

        if (tokenType == TokenType.NFTItem)
        {
            //get nft info from nft index
            var nftInfoIndex =
                await GetEntityAsync<NFTInfoIndex>(IdGenerateHelper.GetId(context.ChainId, symbol));

            var id = IdGenerateHelper.GetId(context.ChainId, address, symbol);
            var caHolderSearchTokenNFTIndex = await GetEntityAsync<CAHolderSearchTokenNFTIndex>(id);
            if (caHolderSearchTokenNFTIndex != null)
            {
                caHolderSearchTokenNFTIndex.Balance += amount;
            }
            else
            {
                caHolderSearchTokenNFTIndex = new CAHolderSearchTokenNFTIndex()
                {
                    Id = IdGenerateHelper.GetId(context.ChainId, address, symbol),
                    CAAddress = address,
                    Balance = amount,
                    TokenId = TokenHelper.GetNFTItemId(symbol),
                    NftInfo = ObjectMapper.Map<NFTInfoIndex, NFTSearchInfo>(nftInfoIndex)
                };
            }

            await SaveEntityAsync(caHolderSearchTokenNFTIndex);
        }
    }
}