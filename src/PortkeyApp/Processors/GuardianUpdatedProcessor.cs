using System.Linq.Expressions;
using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Guardian = Portkey.Contracts.CA.Guardian;

namespace PortkeyApp.Processors;

public class GuardianUpdatedProcessor : GuardianProcessorBase<GuardianUpdated>
{
    private readonly IAeFinderLogger _logger;

    public GuardianUpdatedProcessor(IAeFinderLogger logger)
    {
        _logger = logger;
    }

    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(GuardianUpdated logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        //check ca address if already exist in caHolderIndex
        var id = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(id);

        if (logEvent.GuardianUpdatedPre == null) return;
        if (logEvent.GuardianUpdatedPre == null ||
            logEvent.GuardianUpdatedPre.IdentifierHash == null ||
            logEvent.GuardianUpdatedPre.IdentifierHash.Value == null ||
            logEvent.GuardianUpdatedPre.VerifierId == null ||
            logEvent.GuardianUpdatedPre.VerifierId.Value == null)
        {
            _logger.LogInformation("[ProcessGuardianUpdatedProcessor] fail, transactionId:{0}",
                context.Transaction.TransactionId);
            
            return;
        }
        
        var guardian = caHolderIndex?.Guardians?.FirstOrDefault(g =>
            g.IdentifierHash == logEvent.GuardianUpdatedPre.IdentifierHash.ToHex() &&
            g.VerifierId == logEvent.GuardianUpdatedPre.VerifierId.ToHex() &&
            g.Type == (int)logEvent.GuardianUpdatedPre.Type);

        if (guardian == null || guardian.VerifierId == logEvent.GuardianUpdatedNew.VerifierId.ToHex()) return;

        guardian.VerifierId = logEvent.GuardianUpdatedNew.VerifierId.ToHex();
        guardian.TransactionId = context.Transaction.TransactionId;

        await SaveEntityAsync(caHolderIndex);

        await AddChangeRecordAsync(logEvent.CaAddress.ToBase58(), logEvent.CaHash.ToHex(), nameof(GuardianUpdated),
            ObjectMapper.Map<Guardian, Entities.Guardian>(logEvent.GuardianUpdatedNew), context);
    }

    protected override async Task HandlerTransactionIndexAsync(GuardianUpdated eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58());
        ;
    }
}