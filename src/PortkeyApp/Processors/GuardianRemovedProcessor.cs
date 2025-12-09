using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Guardian = Portkey.Contracts.CA.Guardian;

namespace PortkeyApp.Processors;

public class GuardianRemovedProcessor : GuardianProcessorBase<GuardianRemoved>
{
    private readonly IAeFinderLogger _logger;

    public GuardianRemovedProcessor(IAeFinderLogger logger)
    {
        _logger = logger;
    }

    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(GuardianRemoved logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        //check ca address if already exist in caHolderIndex
        var id = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(id);

        if (logEvent.GuardianRemoved_ == null || 
            logEvent.GuardianRemoved_.IdentifierHash == null ||
            logEvent.GuardianRemoved_.IdentifierHash.Value == null ||
            logEvent.GuardianRemoved_.VerifierId == null ||
            logEvent.GuardianRemoved_.VerifierId.Value == null)
        {
            _logger.LogInformation("[ProcessGuardianRemoved] fail, transactionId:{0}",
                context.Transaction.TransactionId);
            
            return;
        }

        var guardian = caHolderIndex?.Guardians?.FirstOrDefault(g =>
            g.IdentifierHash == logEvent.GuardianRemoved_.IdentifierHash.ToHex() &&
            g.VerifierId == logEvent.GuardianRemoved_.VerifierId.ToHex() &&
            g.Type == (int)logEvent.GuardianRemoved_.Type);

        if (guardian == null) return;

        caHolderIndex.Guardians.Remove(guardian);
        await SaveEntityAsync(caHolderIndex);
        await AddChangeRecordAsync(logEvent.CaAddress.ToBase58(), logEvent.CaHash.ToHex(), nameof(GuardianRemoved),
            ObjectMapper.Map<Guardian, Entities.Guardian>(logEvent.GuardianRemoved_), context);
    }

    protected override async Task HandlerTransactionIndexAsync(GuardianRemoved eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58(), eventValue.Platform);
    }
}