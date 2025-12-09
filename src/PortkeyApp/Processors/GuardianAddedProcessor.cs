using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Guardian = Portkey.Contracts.CA.Guardian;

namespace PortkeyApp.Processors;

public class GuardianAddedProcessor : GuardianProcessorBase<GuardianAdded>
{
    private readonly IAeFinderLogger _logger;

    public GuardianAddedProcessor(IAeFinderLogger logger)
    {
        _logger = logger;
    }

    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(GuardianAdded logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        //check ca address if already exist in caHolderIndex
        var id = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(id);

        // skip accelerate addGuardian
        if (caHolderIndex == null || caHolderIndex.Guardians == null) return;

        if (logEvent.GuardianAdded_ == null ||
            logEvent.GuardianAdded_.IdentifierHash == null ||
            logEvent.GuardianAdded_.IdentifierHash.Value == null ||
            logEvent.GuardianAdded_.VerifierId == null ||
            logEvent.GuardianAdded_.VerifierId.Value == null)
        {
            _logger.LogInformation("[ProcessGuardianAddedProcessor] fail, transactionId:{0}",
                context.Transaction.TransactionId);

            return;
        }

        var guardian = caHolderIndex.Guardians.FirstOrDefault(g =>
            g.IdentifierHash == logEvent.GuardianAdded_.IdentifierHash.ToHex() &&
            g.VerifierId == logEvent.GuardianAdded_.VerifierId.ToHex() &&
            g.Type == (int)logEvent.GuardianAdded_.Type);

        if (guardian != null) return;

        var guardianAdded = ObjectMapper.Map<Guardian, Entities.Guardian>(logEvent.GuardianAdded_);
        guardianAdded.TransactionId = context.Transaction.TransactionId;
        caHolderIndex.Guardians.Add(guardianAdded);

        await SaveEntityAsync(caHolderIndex);
        await AddChangeRecordAsync(logEvent.CaAddress.ToBase58(), logEvent.CaHash.ToHex(),
            nameof(GuardianAdded), guardianAdded, context);
    }

    protected override async Task HandlerTransactionIndexAsync(GuardianAdded eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58(), eventValue.Platform);
    }
}