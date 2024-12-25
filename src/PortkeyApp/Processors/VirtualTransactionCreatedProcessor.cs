using AeFinder.Sdk.Processor;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class VirtualTransactionCreatedProcessor : CAHolderTransactionProcessorBase<VirtualTransactionCreated>
{
    private readonly List<string> _skipMethodNames = new(){"transfer", "transferfrom", "approve"};
    
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(VirtualTransactionCreated logEvent, LogEventContext context)
    {
        BreakHelper.CheckBreak(context.ChainId, context.Block.BlockHeight);
        if (_skipMethodNames.Contains(logEvent.MethodName.ToLower()))
        {
            return;
        }

        var holder = await GetEntityAsync<CAHolderIndex>(IdGenerateHelper.GetId(context.ChainId, logEvent.From.ToBase58()));
        if (holder == null) return;
        var id = IdGenerateHelper.GetId(context.Block.BlockHash, context.Transaction.TransactionId);
        var transIndex = await GetEntityAsync<CAHolderTransactionIndex>(id);
        if (transIndex != null)
        {
            return;
        }

        transIndex = new CAHolderTransactionIndex
        {
            Id = id,
            Timestamp = context.Block.BlockTime.ToTimestamp().Seconds,
            FromAddress = logEvent.From.ToBase58(),
            ToContractAddress = GetToContractAddress(context.ChainId, context.Transaction.To, context.Transaction.MethodName, context.Transaction.Params),
            TransactionFee = GetTransactionFee(context.Transaction.ExtraProperties),
            TransactionId = context.Transaction.TransactionId,
            Status = context.Transaction.Status
        };

        transIndex.MethodName = logEvent.MethodName;
        await SaveEntityAsync(transIndex);
    }
}