using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Guardian = Portkey.Contracts.CA.Guardian;

namespace PortkeyApp.Processors;

public class LoginGuardianAddedProcessor : LoginGuardianProcessorBase<LoginGuardianAdded>
{
    private readonly IAeFinderLogger _logger;

    public LoginGuardianAddedProcessor(IAeFinderLogger logger)
    {
        _logger = logger;
    }

    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(LoginGuardianAdded logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);
        
        if (logEvent.LoginGuardian == null ||
            logEvent.LoginGuardian.IdentifierHash == null ||
            logEvent.LoginGuardian.IdentifierHash.Value == null ||
            logEvent.LoginGuardian.VerifierId == null ||
            logEvent.LoginGuardian.VerifierId.Value == null)
        {
            _logger.LogInformation("[ProcessLoginGuardianAddedProcessor] fail, transactionId:{0}",
                context.Transaction.TransactionId);
            return;
        }
        
        var indexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58(),
            logEvent.LoginGuardian.IdentifierHash.ToHex(), logEvent.LoginGuardian.VerifierId.ToHex());
        var loginGuardianIndex = await GetEntityAsync<LoginGuardianIndex>(indexId);
        if (loginGuardianIndex != null)
        {
            return;
        }

        loginGuardianIndex = new LoginGuardianIndex
        {
            Id = indexId,
            CAHash = logEvent.CaHash.ToHex(),
            CAAddress = logEvent.CaAddress.ToBase58(),
            Manager = logEvent.Manager.ToBase58(),
            LoginGuardian = new Entities.Guardian
            {
                Type = (int)logEvent.LoginGuardian.Type,
                VerifierId = logEvent.LoginGuardian.VerifierId.ToHex(),
                Salt = logEvent.LoginGuardian.Salt,
                IsLoginGuardian = logEvent.LoginGuardian.IsLoginGuardian,
                IdentifierHash = logEvent.LoginGuardian.IdentifierHash.ToHex()
            }
        };

        await SaveEntityAsync(loginGuardianIndex);
        await AddChangeRecordAsync(loginGuardianIndex.CAAddress, loginGuardianIndex.CAHash,
            loginGuardianIndex.Manager, loginGuardianIndex.LoginGuardian, nameof(LoginGuardianAdded), context);

        //check ca address if already exist in caHolderIndex
        var id = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(id);
        if (caHolderIndex == null) return;

        var guardian = caHolderIndex.Guardians.FirstOrDefault(g =>
            g.IdentifierHash == logEvent.LoginGuardian.IdentifierHash.ToHex() &&
            g.VerifierId == logEvent.LoginGuardian.VerifierId.ToHex() &&
            g.Type == (int)logEvent.LoginGuardian.Type);

        if (guardian == null)
        {
            guardian = ObjectMapper.Map<Guardian, Entities.Guardian>(logEvent.LoginGuardian);
            guardian.TransactionId = context.Transaction.TransactionId;
            caHolderIndex.Guardians.Add(guardian);
        }
        else
        {
            if (guardian.IsLoginGuardian) return;
            guardian.IsLoginGuardian = true;
        }
        
        await SaveEntityAsync(caHolderIndex);
    }

    protected override async Task HandlerTransactionIndexAsync(LoginGuardianAdded eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58());
    }
}