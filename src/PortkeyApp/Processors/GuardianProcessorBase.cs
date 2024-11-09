using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using PortkeyApp.Common;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public abstract class GuardianProcessorBase<TEvent> : CAHolderTransactionProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected async Task AddChangeRecordAsync(string caAddress, string caHash,
        string changeType, Entities.Guardian guardian, LogEventContext context)
    {
        var changeRecordId = IdGenerateHelper.GetId(context.ChainId, caAddress, context.Transaction.TransactionId);
        var changeRecordIndex = await GetEntityAsync<GuardianChangeRecordIndex>(changeRecordId);
        if (changeRecordIndex != null) return;
        
        changeRecordIndex = new GuardianChangeRecordIndex
        {
            Id = changeRecordId,
            ChangeType = changeType,
            CAAddress = caAddress,
            CAHash = caHash,
            Guardian = guardian
        };
        await SaveEntityAsync(changeRecordIndex);
    }
}