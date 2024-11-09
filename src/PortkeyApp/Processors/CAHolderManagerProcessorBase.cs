using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using PortkeyApp.Common;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public abstract class CAHolderManagerProcessorBase<TEvent> : CAHolderTransactionProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected async Task AddChangeRecordAsync(string caAddress, string caHash, string manager, string changeType,
        LogEventContext context)
    {
        var changeRecordId = IdGenerateHelper.GetId(context.ChainId, caAddress,
            manager, context.Transaction.TransactionId);
        var changeRecordIndex = await GetEntityAsync<CAHolderManagerChangeRecordIndex>(changeRecordId);
        if (changeRecordIndex != null) return;
        changeRecordIndex = new CAHolderManagerChangeRecordIndex
        {
            Id = changeRecordId,
            Manager = manager,
            ChangeType = changeType,
            CAAddress = caAddress,
            CAHash = caHash
        };

        await SaveEntityAsync(changeRecordIndex);
    }
}