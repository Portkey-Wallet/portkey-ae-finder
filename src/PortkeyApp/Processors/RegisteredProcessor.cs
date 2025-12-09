using AeFinder.Sdk.Processor;
using Portkey.Contracts.BingoGameContract;
using PortkeyApp.Configs;

namespace PortkeyApp.Processors;

public class RegisteredProcessor : CAHolderTransactionProcessorBase<Registered>
{
    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(Registered logEvent, LogEventContext context)
    {
        if (logEvent.PlayerAddress == null || logEvent.PlayerAddress.Value == null)
        {
            return;
        }

        await ProcessCAHolderTransactionAsync(context, logEvent.PlayerAddress.ToBase58());
    }
}