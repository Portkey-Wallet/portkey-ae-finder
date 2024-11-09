using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TransactionFeeChargedProcessor: CAHolderTokenBalanceProcessorBase<TransactionFeeCharged>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).TokenContractAddress;
    }

    public override async Task ProcessAsync(TransactionFeeCharged logEvent, LogEventContext context)
    {
        if (logEvent.ChargingAddress == null) return;

        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.ChargingAddress, context.Block.BlockHash);
        var transactionFeeChangedIndex = new TransactionFeeChangedIndex
        {
            Id = indexId,
            ConsumerAddress = logEvent.ChargingAddress.ToBase58(),
        };
        ObjectMapper.Map(logEvent, transactionFeeChangedIndex);

        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(
            context.ChainId, logEvent.ChargingAddress.ToBase58()));
        if (caHolderIndex != null)
        {
            transactionFeeChangedIndex.CAAddress = caHolderIndex.CAAddress;
            await ModifyBalanceAsync(caHolderIndex.CAAddress, logEvent.Symbol, -logEvent.Amount, context);
            await HandlerTransactionIndexAsync(logEvent, context);
        }

        await SaveEntityAsync(transactionFeeChangedIndex);
    }

    protected override async Task HandlerTransactionIndexAsync(TransactionFeeCharged eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var transIndex = await GetEntityAsync<CAHolderTransactionIndex>(id);
        transIndex ??= new CAHolderTransactionIndex
        {
            Id = id,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = eventValue.ChargingAddress.ToBase58(),
            ToContractAddress = GetToContractAddress(context.ChainId, context.Transaction.To, context.Transaction.MethodName, context.Transaction.Params),
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };
        if (transIndex.TransactionFee.TryGetValue(eventValue.Symbol, out _))
        {
            transIndex.TransactionFee[eventValue.Symbol] += eventValue.Amount;
        }
        else
        {
            transIndex.TransactionFee[eventValue.Symbol] = eventValue.Amount;
        }
        
        transIndex.MethodName = GetMethodName(context.Transaction.MethodName, context.Transaction.Params);
        await SaveEntityAsync(transIndex);
    }
}