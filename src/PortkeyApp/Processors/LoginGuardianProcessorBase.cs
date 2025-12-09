using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using PortkeyApp.Common;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public abstract class LoginGuardianProcessorBase<TEvent> : CAHolderTransactionProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected async Task AddChangeRecordAsync(string caAddress, string caHash, string manager,
        Entities.Guardian loginGuardian, string changeType, LogEventContext context)
    {
        var changeRecordId = IdGenerateHelper.GetId(context.ChainId, caAddress,
            loginGuardian.IdentifierHash, changeType, context.Transaction.TransactionId);
        var changeRecordIndex = await GetEntityAsync<LoginGuardianChangeRecordIndex>(changeRecordId);
        if (changeRecordIndex != null) return;
        changeRecordIndex = new LoginGuardianChangeRecordIndex
        {
            Id = changeRecordId,
            CAAddress = caAddress,
            CAHash = caHash,
            Manager = manager,
            LoginGuardian = loginGuardian,
            ChangeType = changeType
        };
        
        await SaveEntityAsync(changeRecordIndex);
    }
}