using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TokenApprovedProcessor: CAHolderTransactionProcessorBase<Approved>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(Approved logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        if (logEvent.Symbol.Equals("*") || (logEvent.Symbol.Contains("-") && !logEvent.Symbol.Contains("-*")))
        {
            return;
        }

        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.Owner.ToBase58()));
        if (holder == null) return;
        var batchApprovedIndexId = IdGenerateHelper.GetId(context.ChainId, logEvent.Owner.ToBase58(), logEvent.Spender.ToBase58(), logEvent.Symbol);
        var batchApprovedIndex = await GetEntityAsync<CAHolderTokenApprovedIndex>(batchApprovedIndexId);

        batchApprovedIndex ??= new CAHolderTokenApprovedIndex
        {
            Id = batchApprovedIndexId,
            Spender = logEvent.Spender.ToBase58(),
            CAAddress = logEvent.Owner.ToBase58(),
            Symbol = logEvent.Symbol
        };

        batchApprovedIndex.BatchApprovedAmount += logEvent.Amount;
        await SaveEntityAsync(batchApprovedIndex);
    }

    protected override async Task HandlerTransactionIndexAsync(Approved eventValue, LogEventContext context)
    {
        if (!IsValidTransaction(context.ChainId, context.Transaction.To, context.Transaction.MethodName, context.Transaction.Params)) return;
        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, eventValue.Owner.ToBase58()));
        if (holder == null) return;
        
        var index = new CAHolderTransactionIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = eventValue.Owner.ToBase58(),
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };
        index.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        await SaveEntityAsync(index);
    }
}