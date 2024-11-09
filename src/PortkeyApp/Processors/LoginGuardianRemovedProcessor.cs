using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;

namespace PortkeyApp.Processors;

public class LoginGuardianRemovedProcessor : LoginGuardianProcessorBase<LoginGuardianRemoved>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(LoginGuardianRemoved logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        
        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58(),
            logEvent.LoginGuardian.IdentifierHash.ToHex(), logEvent.LoginGuardian.VerifierId.ToHex());
        var loginGuardianIndex = await GetEntityAsync<LoginGuardianIndex>(indexId);
        if (loginGuardianIndex == null)
        {
            return;
        }
        
        await DeleteEntityAsync(loginGuardianIndex);
        
        await AddChangeRecordAsync(loginGuardianIndex.CAAddress, loginGuardianIndex.CAHash,
            loginGuardianIndex.Manager, loginGuardianIndex.LoginGuardian, nameof(LoginGuardianRemoved), context);

        //check ca address if already exist in caHolderIndex
        var id = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(id);
        if (caHolderIndex == null) return;

        if (logEvent.LoginGuardian == null ||
            logEvent.LoginGuardian.IdentifierHash == null ||
            logEvent.LoginGuardian.IdentifierHash.Value == null ||
            logEvent.LoginGuardian.VerifierId == null ||
            logEvent.LoginGuardian.VerifierId.Value == null)
        {
            return;
        }
        
        var guardian = caHolderIndex.Guardians.FirstOrDefault(g =>
            g.IdentifierHash == logEvent.LoginGuardian.IdentifierHash.ToHex() &&
            g.VerifierId == logEvent.LoginGuardian.VerifierId.ToHex() &&
            g.Type == (int)logEvent.LoginGuardian.Type);

        if (guardian == null || !guardian.IsLoginGuardian) return;

        guardian.IsLoginGuardian = false;
        guardian.TransactionId = context.Transaction.TransactionId;

        await SaveEntityAsync(caHolderIndex);
    }
    
    protected override async Task HandlerTransactionIndexAsync(LoginGuardianRemoved eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58());;
    }
}