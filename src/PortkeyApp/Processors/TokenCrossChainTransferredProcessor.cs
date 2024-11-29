using AeFinder.Sdk.Processor;
using AElf;
using AElf.Contracts.MultiToken;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenCrossChainTransferredProcessor : CAHolderTransactionProcessorBase<CrossChainTransferred>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(CrossChainTransferred logEvent, LogEventContext context)
    {
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName,
                context.Transaction.Params)) return;

        var methodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        var toCaHolder =
            await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.To.ToBase58()));

        var fromCaAddress = methodName == CommonConstants.InlineCrossChainTransfer ? logEvent.From.ToBase58() : string.Empty;
        if (fromCaAddress.IsNullOrEmpty())
        {
            var fromManager = await GetEntityAsync<CAHolderManagerIndex>(IdGenerateHelper.GetId(
                context.ChainId, logEvent.From.ToBase58()));
            fromCaAddress = fromManager?.CAAddresses?.FirstOrDefault();
        }

        if (fromCaAddress.IsNullOrEmpty() && toCaHolder == null)
        {
            return;
        }

        var tokenInfoIndex = await GetTokenInfoIndexFromStateOrChainAsync(logEvent.Symbol, context);
        var nftInfoIndex = await GetNftInfoIndexFromStateOrChainAsync(logEvent.Symbol, context);

        if (!fromCaAddress.IsNullOrEmpty())
        {
            await AddCAHolderTransactionAddressAsync(fromCaAddress, logEvent.To.ToBase58(),
                ChainHelper.ConvertChainIdToBase58(logEvent.ToChainId), context);
        }
        else
        {
            await AddCompatibleCrossChainTransferAsync(context);
        }

        await SaveEntityAsync(GetCaHolderTransactionIndex(logEvent,
            tokenInfoIndex, nftInfoIndex, fromCaAddress, context));
    }

    private CAHolderTransactionIndex GetCaHolderTransactionIndex(CrossChainTransferred transferred,
        TokenInfoIndex tokenInfoIndex,
        NFTInfoIndex nftInfoIndex, string fromCaAddress, LogEventContext context)
    {
        var index = new CAHolderTransactionIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = context.Transaction.From,
            TokenInfo = tokenInfoIndex,
            NftInfo = nftInfoIndex,
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransferInfo = new TransferInfo
            {
                Amount = transferred.Amount,
                FromAddress = transferred.From.ToBase58(),
                FromCAAddress = fromCaAddress ?? string.Empty,
                ToAddress = transferred.To.ToBase58(),
                FromChainId = context.ChainId,
                ToChainId = ChainHelper.ConvertChainIdToBase58(transferred.ToChainId)
            },
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };

        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        return index;
    }

    private async Task AddCompatibleCrossChainTransferAsync(LogEventContext context)
    {
        var index = new CompatibleCrossChainTransferIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = context.Transaction.From,
            ToAddress = context.Transaction.To
        };

        await SaveEntityAsync(index);
    }
}