using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Configs;

namespace PortkeyApp.Processors;

public class LoginGuardianUnboundProcessor : LoginGuardianProcessorBase<LoginGuardianUnbound>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(LoginGuardianUnbound logEvent, LogEventContext context)
    {
        await HandlerTransactionIndexAsync(logEvent, context);

        await AddChangeRecordAsync(logEvent.CaAddress.ToBase58(), logEvent.CaHash.ToHex(),
            logEvent.Manager.ToBase58(), new Entities.Guardian
            {
                IdentifierHash = logEvent.LoginGuardianIdentifierHash.ToHex()
            }, nameof(LoginGuardianUnbound), context);
    }

    protected override async Task HandlerTransactionIndexAsync(LoginGuardianUnbound eventValue, LogEventContext context)
    {
        await ProcessCAHolderTransactionAsync(context, eventValue.CaAddress.ToBase58(), eventValue.Platform);
    }
}