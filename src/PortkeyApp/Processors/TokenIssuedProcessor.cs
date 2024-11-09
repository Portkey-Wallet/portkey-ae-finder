using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenIssuedProcessor : CAHolderTokenBalanceProcessorBase<Issued>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(Issued logEvent, LogEventContext context)
    {
        await UpdateTokenSupply(logEvent, context);
        var holder =
            await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.To.ToBase58()));
        if (holder == null) return;

        await ModifyBalanceAsync(holder.CAAddress, logEvent.Symbol, logEvent.Amount, context);
        await HandlerTransactionIndexAsync(logEvent, context);
    }

    protected override async Task HandlerTransactionIndexAsync(Issued eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var transIndex = await GetEntityAsync<CAHolderTransactionIndex>(id);
        transIndex ??= new CAHolderTransactionIndex
        {
            Id = id,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = context.Transaction.From,
            ToContractAddress = GetToContractAddress(context.ChainId, context.Transaction.To,
                context.Transaction.MethodName, context.Transaction.Params),
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };
        if (transIndex.TransferInfo == null)
        {
            transIndex.TokenInfo = await GetTokenInfoIndexFromStateOrChainAsync(eventValue.Symbol, context);
            transIndex.NftInfo = await GetNftInfoIndexFromStateOrChainAsync(eventValue.Symbol, context);
            transIndex.TransferInfo = new TransferInfo
            {
                FromAddress = CommonConstants.EmptyAddress,
                FromCAAddress = CommonConstants.EmptyAddress,
                Amount = eventValue.Amount,
                ToAddress = eventValue.To.ToBase58(),
                FromChainId = context.ChainId,
                ToChainId = context.ChainId
            };
        }

        transIndex.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        await SaveEntityAsync(transIndex);
    }

    private async Task UpdateTokenSupply(Issued eventValue, LogEventContext context)
    {
        TokenType tokenType = TokenHelper.GetTokenType(eventValue.Symbol);

        if (tokenType == TokenType.Token)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, eventValue.Symbol);
            var tokenInfoIndex = await GetEntityAsync<TokenInfoIndex>(id);
            if (tokenInfoIndex != null)
            {
                tokenInfoIndex.Supply += eventValue.Amount;
                await SaveEntityAsync(tokenInfoIndex);
            }
        }

        if (tokenType == TokenType.NFTCollection)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, eventValue.Symbol);
            var nftCollectionInfoIndex = await GetEntityAsync<NFTCollectionInfoIndex>(id);
            if (nftCollectionInfoIndex != null)
            {
                nftCollectionInfoIndex.Supply += eventValue.Amount;
                await SaveEntityAsync(nftCollectionInfoIndex);
            }
        }

        if (tokenType == TokenType.NFTItem)
        {
            var id = IdGenerateHelper.GetId(context.ChainId, eventValue.Symbol);
            var nftInfoIndex = await GetEntityAsync<NFTInfoIndex>(id);
            if (nftInfoIndex != null)
            {
                nftInfoIndex.Supply += eventValue.Amount;
                await SaveEntityAsync(nftInfoIndex);
            }
        }
    }
}