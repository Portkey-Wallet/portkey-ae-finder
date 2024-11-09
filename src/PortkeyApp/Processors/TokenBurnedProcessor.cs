using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenBurnedProcessor:  CAHolderTokenBalanceProcessorBase<Burned>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(Burned logEvent, LogEventContext context)
    {
        await UpdateTokenSupply(logEvent, context);
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId,
            logEvent.Burner.ToBase58()));
        
        if (holder == null) return;
        await ModifyBalanceAsync(holder.CAAddress, logEvent.Symbol, -logEvent.Amount, context);
    }
    
    private async Task UpdateTokenSupply(Burned eventValue, LogEventContext context)
    {
        var tokenType = TokenHelper.GetTokenType(eventValue.Symbol);
        if (tokenType == TokenType.Token)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, eventValue.Symbol);
            var tokenInfoIndex = await GetEntityAsync<TokenInfoIndex>(id);
            if (tokenInfoIndex != null)
            {
                tokenInfoIndex.Supply -= eventValue.Amount;
                await SaveEntityAsync(tokenInfoIndex);
            }
        }

        if (tokenType == TokenType.NFTCollection)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, eventValue.Symbol);
            var nftCollectionInfoIndex = await GetEntityAsync<NFTCollectionInfoIndex>(id);
            if (nftCollectionInfoIndex != null)
            {
                nftCollectionInfoIndex.Supply -= eventValue.Amount;
                await SaveEntityAsync(nftCollectionInfoIndex);
            }
        }

        if (tokenType == TokenType.NFTItem)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, eventValue.Symbol);
            var nftInfoIndex = await GetEntityAsync<NFTInfoIndex>(id);
            if (nftInfoIndex != null)
            {
                nftInfoIndex.Supply -= eventValue.Amount;
                await SaveEntityAsync(nftInfoIndex);
            }
        }
        
    }
}