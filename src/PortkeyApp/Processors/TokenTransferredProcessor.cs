using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenTransferredProcessor : CAHolderTokenBalanceProcessorBase<Transferred>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(Transferred logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        var from = await GetEntityAsync<CAHolderIndex>(
            IdGenerateHelper.GetId(context.ChainId, logEvent.From.ToBase58()));
        if (from != null)
        {
            await ModifyBalanceAsync(from.CAAddress, logEvent.Symbol, -logEvent.Amount, context);
        }
        else
        {
            await ModifyBalanceAsync(logEvent.From.ToBase58(), logEvent.Symbol, -logEvent.Amount, context);
        }

        var to = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.To.ToBase58()));
        if (to != null)
        {
            await ModifyBalanceAsync(to.CAAddress, logEvent.Symbol, logEvent.Amount, context);
        }
        else
        {
            await ModifyBalanceAsync(logEvent.To.ToBase58(), logEvent.Symbol, logEvent.Amount, context);
        }
    }

    protected override async Task HandlerTransactionIndexAsync(Transferred eventValue, LogEventContext context)
    {
        var from = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId,
            eventValue.From.ToBase58()));
        var tokenInfoIndex = await GetTokenInfoIndexFromStateOrChainAsync(eventValue.Symbol, context);
        var nftInfoIndex = await GetNftInfoIndexFromStateOrChainAsync(eventValue.Symbol, context);

        if (from != null)
        {
            await AddCAHolderTransactionAddressAsync(from.CAAddress, eventValue.To.ToBase58(), context.ChainId,
                context);
            await SaveEntityAsync(await GetCaHolderTransactionIndexAsync(eventValue, tokenInfoIndex, nftInfoIndex,
                context));
        }

        var to = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, eventValue.To.ToBase58()));
        if (to == null) return;
        await AddCAHolderTransactionAddressAsync(to.CAAddress, eventValue.From.ToBase58(), context.ChainId, context);
        if (from != null) return;
        await SaveEntityAsync(
            await GetCaHolderTransactionIndexAsync(eventValue, tokenInfoIndex, nftInfoIndex, context));
    }

    private async Task<CAHolderTransactionIndex> GetCaHolderTransactionIndexAsync(Transferred transferred,
        TokenInfoIndex tokenInfoIndex, NFTInfoIndex nftInfoIndex, LogEventContext context)
    {
        var id = IsMultiTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName)
            ? IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId, transferred.To.ToBase58())
            : IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var index = await GetEntityAsync<CAHolderTransactionIndex>(id);
        index ??= new CAHolderTransactionIndex
        {
            Id = id,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = context.Transaction.From,
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            ToContractAddress = GetToContractAddress(context.ChainId, context.Transaction.To, context.Transaction.MethodName, context.Transaction.Params),
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };
        
        if (index.TransferInfo != null)
        {
            index.TokenTransferInfos.Add(new TokenTransferInfo
            {
                TransferInfo = index.TransferInfo,
                TokenInfo = index.TokenInfo,
                NftInfo = index.NftInfo
            });
            index.TransferInfo = null;
            index.NftInfo = null;
            index.TokenInfo = null;
        }

        var transferInfo = new TransferInfo
        {
            Amount = transferred.Amount,
            FromAddress = transferred.From.ToBase58(),
            FromCAAddress = transferred.From.ToBase58(),
            ToAddress = transferred.To.ToBase58(),
            FromChainId = context.ChainId,
            ToChainId = context.ChainId
        };
        if (index.TokenTransferInfos.Count > 0)
        {
            index.TokenTransferInfos.Add(new TokenTransferInfo
            {
                TransferInfo = transferInfo,
                TokenInfo = tokenInfoIndex,
                NftInfo = nftInfoIndex
            });
        }
        else
        {
            index.TransferInfo = transferInfo;
            index.TokenInfo = tokenInfoIndex;
            index.NftInfo = nftInfoIndex;
        }
        
        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        return index;
    }
}