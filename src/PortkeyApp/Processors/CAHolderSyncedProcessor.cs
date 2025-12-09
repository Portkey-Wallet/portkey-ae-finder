using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using AElf;
using AElf.Types;
using Nest;
using Portkey.Contracts.CA;
using PortkeyApp.Common;
using PortkeyApp.Configs;
using PortkeyApp.Entities;
using Guardian = PortkeyApp.Entities.Guardian;
using ManagerInfo = PortkeyApp.Entities.ManagerInfo;

namespace PortkeyApp.Processors;

public class CAHolderSyncedProcessor : LogEventProcessorBase<CAHolderSynced>
{
    private readonly IReadOnlyRepository<CAHolderIndex> _caHolderRepository;

    public CAHolderSyncedProcessor(IReadOnlyRepository<CAHolderIndex> caHolderRepository)
    {
        _caHolderRepository = caHolderRepository;
    }

    public override string GetContractAddress(string chainId)
    {
        return ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
    }

    public override async Task ProcessAsync(CAHolderSynced logEvent, LogEventContext context)
    {
        //check ca address if already exist in caHolderIndex
        var caHolderIndexId = IdGenerateHelper.GetId(context.ChainId, logEvent.CaAddress.ToBase58());
        var caHolderIndex = await GetEntityAsync<CAHolderIndex>(caHolderIndexId);
        if (caHolderIndex == null)
        {
            //CaHolder Create
            await CreateCAHolderAysnc(logEvent, context);
        }
        else
        {
            //Add or Remove manager
            await AddOrRemoveManager(caHolderIndex, logEvent, context);
        }

        //Add LoginGuardians
        await AddLoginGuardians(logEvent, context);

        //Unbound LoginGuardians
        await UnboundLoginGuardians(logEvent, context);
    }

    private async Task CreateCAHolderAysnc(CAHolderSynced eventValue,
        LogEventContext context)
    {
        var managerList = new List<ManagerInfo>();
        if (eventValue.ManagerInfosAdded.ManagerInfos.Count > 0)
        {
            foreach (var item in eventValue.ManagerInfosAdded.ManagerInfos)
            {
                //check manager is already exist in caHolderManagerIndex
                var managerIndexId = IdGenerateHelper.GetId(context.ChainId, item.Address.ToBase58());
                var caHolderManagerIndex =
                    await GetEntityAsync<CAHolderManagerIndex>(managerIndexId);
                if (caHolderManagerIndex == null)
                {
                    caHolderManagerIndex = new CAHolderManagerIndex
                    {
                        Id = managerIndexId,
                        Manager = item.Address.ToBase58(),
                        CAAddresses = new List<string>()
                        {
                            eventValue.CaAddress.ToBase58()
                        }
                    };
                }
                else
                {
                    if (!caHolderManagerIndex.CAAddresses.Contains(eventValue.CaAddress.ToBase58()))
                    {
                        caHolderManagerIndex.CAAddresses.Add(eventValue.CaAddress.ToBase58());
                    }
                }

                await SaveEntityAsync(caHolderManagerIndex);

                //add manager info to manager list
                managerList.Add(new ManagerInfo
                {
                    Address = item.Address.ToBase58(),
                    ExtraData = item.ExtraData
                });
            }
        }

        var caHolderIndex = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(context.ChainId, eventValue.CaAddress.ToBase58()),
            CAHash = eventValue.CaHash.ToHex(),
            CAAddress = eventValue.CaAddress.ToBase58(),
            Creator = eventValue.Creator.ToBase58(),
            ManagerInfos = managerList
        };
        caHolderIndex.OriginChainId = eventValue.CreateChainId == 0
            ? await GetOriginChainIdAsync(eventValue.CaHash.ToHex())
            : ChainHelper.ConvertChainIdToBase58(eventValue.CreateChainId);

        await SaveEntityAsync(caHolderIndex);
    }

    private async Task AddOrRemoveManager(CAHolderIndex caHolderIndex, CAHolderSynced eventValue,
        LogEventContext context)
    {
        //Add manager
        if (eventValue.ManagerInfosAdded.ManagerInfos.Count > 0)
        {
            foreach (var item in eventValue.ManagerInfosAdded.ManagerInfos)
            {
                if (caHolderIndex.ManagerInfos.Count(m =>
                        m.Address == item.Address.ToBase58() && m.ExtraData == item.ExtraData) == 0)
                {
                    caHolderIndex.ManagerInfos.Add(new Entities.ManagerInfo
                    {
                        Address = item.Address.ToBase58(),
                        ExtraData = item.ExtraData
                    });
                }

                //check manager is already exist in caHolderManagerIndex
                var managerIndexId = IdGenerateHelper.GetId(context.ChainId, item.Address.ToBase58());
                var caHolderManagerIndex =
                    await GetEntityAsync<CAHolderManagerIndex>(managerIndexId);
                if (caHolderManagerIndex == null)
                {
                    caHolderManagerIndex = new CAHolderManagerIndex
                    {
                        Id = managerIndexId,
                        Manager = item.Address.ToBase58(),
                        CAAddresses = new List<string>()
                        {
                            eventValue.CaAddress.ToBase58()
                        }
                    };
                }
                else
                {
                    if (!caHolderManagerIndex.CAAddresses.Contains(eventValue.CaAddress.ToBase58()))
                    {
                        caHolderManagerIndex.CAAddresses.Add(eventValue.CaAddress.ToBase58());
                    }
                }

                await SaveEntityAsync(caHolderManagerIndex);
            }
        }

        // TODO When deploy new CA contract, remove this part
        var managerInfosRemoved = eventValue.ManagerInfosRemoved.ManagerInfos;

        //Remove manager
        if (managerInfosRemoved.Count > 0)
        {
            foreach (var item in managerInfosRemoved)
            {
                var removeItem = caHolderIndex.ManagerInfos.FirstOrDefault(m =>
                    m.Address == item.Address.ToBase58() && m.ExtraData == item.ExtraData);
                if (removeItem != null)
                {
                    caHolderIndex.ManagerInfos.Remove(removeItem);
                }

                //check manager is already exist in caHolderManagerIndex
                var managerIndexId = IdGenerateHelper.GetId(context.ChainId, item.Address.ToBase58());
                var caHolderManagerIndex =
                    await GetEntityAsync<CAHolderManagerIndex>(managerIndexId);
                if (caHolderManagerIndex != null)
                {
                    if (caHolderManagerIndex.CAAddresses.Contains(eventValue.CaAddress.ToBase58()))
                    {
                        caHolderManagerIndex.CAAddresses.Remove(eventValue.CaAddress.ToBase58());
                    }

                    if (caHolderManagerIndex.CAAddresses.Count == 0)
                    {
                        await DeleteEntityAsync(caHolderManagerIndex);
                    }
                    else
                    {
                        await SaveEntityAsync(caHolderManagerIndex);
                    }
                }
            }
        }

        if (caHolderIndex.OriginChainId.IsNullOrWhiteSpace())
            caHolderIndex.OriginChainId = eventValue.CreateChainId == 0
                ? await GetOriginChainIdAsync(eventValue.CaHash.ToHex())
                : ChainHelper.ConvertChainIdToBase58(eventValue.CreateChainId);

        await SaveEntityAsync(caHolderIndex);
    }

    private async Task AddLoginGuardians(CAHolderSynced eventValue,
        LogEventContext context)
    {
        if (eventValue.LoginGuardiansAdded.LoginGuardians.Count > 0)
        {
            foreach (var item in eventValue.LoginGuardiansAdded.LoginGuardians)
            {
                var indexId = IdGenerateHelper.GetId(context.ChainId, eventValue.CaAddress.ToBase58(),
                    item, Hash.Empty.ToHex());
                var loginGuardianIndex =
                    await GetEntityAsync<LoginGuardianIndex>(indexId);
                if (loginGuardianIndex != null)
                {
                    continue;
                }

                loginGuardianIndex = new LoginGuardianIndex
                {
                    Id = indexId,
                    CAHash = eventValue.CaHash.ToHex(),
                    CAAddress = eventValue.CaAddress.ToBase58(),
                    LoginGuardian = new Guardian
                    {
                        IdentifierHash = item.ToHex(),
                        IsLoginGuardian = true
                    }
                };
                await SaveEntityAsync(loginGuardianIndex);
            }
        }
    }

    private async Task UnboundLoginGuardians(CAHolderSynced eventValue,
        LogEventContext context)
    {
        if (eventValue.LoginGuardiansUnbound.LoginGuardians.Count > 0)
        {
            foreach (var item in eventValue.LoginGuardiansUnbound.LoginGuardians)
            {
                var indexId = IdGenerateHelper.GetId(context.ChainId, eventValue.CaAddress.ToBase58(),
                    item, Hash.Empty.ToHex());
                var loginGuardianIndex = await GetEntityAsync<LoginGuardianIndex>(indexId);
                if (loginGuardianIndex == null)
                {
                    continue;
                }

                await DeleteEntityAsync(loginGuardianIndex);
            }
        }
    }

    private async Task<string> GetOriginChainIdAsync(string caHash)
    {
        var queryable = await _caHolderRepository.GetQueryableAsync();
        queryable = queryable.Where(t => t.CAHash == caHash);
        var holder = queryable.FirstOrDefault();
        return holder?.OriginChainId ?? string.Empty;
    }
}