using AeFinder.Sdk.Processor;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class TransferLimitChangedProcessor : CAHolderTransactionEventBase<TransferLimitChanged>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(TransferLimitChanged logEvent, LogEventContext context)
    {
        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaHash.ToHex(), nameof(TransferLimitChanged),
            logEvent.Symbol);
        var index = new TransferLimitIndex
        {
            Id = indexId,
            CaHash = logEvent.CaHash.ToHex(),
            Symbol = logEvent.Symbol,
            SingleLimit = logEvent.SingleLimit,
            DailyLimit = logEvent.DailyLimit
        };
        await SaveEntityAsync(index);

        var caAddress =
            ConvertVirtualAddressToContractAddress(logEvent.CaHash, GetContractAddress(context.ChainId).ToAddress());
        if (caAddress == null)
        {
            return;
        }

        var id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var transactionIndex = await GetEntityAsync<CAHolderTransactionIndex>(id);
        var transactionFee = GetTransactionFee(context.Transaction.ExtraProperties);
        if (transactionIndex != null)
        {
            transactionFee = transactionIndex.TransactionFee.IsNullOrEmpty() ? transactionFee : transactionIndex.TransactionFee;
        }

        var transIndex = new CAHolderTransactionIndex
        {
            Id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId),
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = caAddress.ToBase58(),
            TransactionFee = transactionFee,
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };

        transIndex.MethodName = context.Transaction.MethodName;
        await SaveEntityAsync(transIndex);
    }
}