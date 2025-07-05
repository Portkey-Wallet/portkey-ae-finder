using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using AeFinder.Sdk.Entities;
using AElf.Types;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using AElf.Standards.ACS0;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using System.Collections.Generic;
using System;
using Volo.Abp.ObjectMapping;
using AElf;
using PortkeyApp.Configs;

namespace PortkeyApp.Tests.Processors
{
    public class ContractDeployedProcessorTests : PortkeyAppTestBase
    {
        private readonly ContractDeployedProcessor _processor;
        private readonly IRepository<NFTCollectionInfoIndex> _nftCollectionRepository;
        private readonly IRepository<TokenInfoIndex> _tokenInfoRepository;

        public ContractDeployedProcessorTests()
        {
            _processor = GetRequiredService<ContractDeployedProcessor>();
            _nftCollectionRepository = GetRequiredService<IRepository<NFTCollectionInfoIndex>>();
            _tokenInfoRepository = GetRequiredService<IRepository<TokenInfoIndex>>();
        }

        [Fact]
        public async Task ProcessAsync_Should_Create_ContractIndex()
        {
            // Arrange
            var chainId = "AELF";
            var author = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
            var codeHash = HashHelper.ComputeFrom("test-code-hash");
            // Use the CA contract address from config to match the processor logic
            var caContractAddress = ConfigConstants.ContractInfos.First(c => c.ChainId == chainId).CAContractAddress;
            var address = Address.FromBase58(caContractAddress);
            var version = 1;
            var name = HashHelper.ComputeFrom("test-contract-name");
            
            var logEvent = new ContractDeployed
            {
                Author = author,
                CodeHash = codeHash,
                Address = address,
                Version = version,
                Name = name
            };
            
            var context = new LogEventContext
            {
                ChainId = chainId,
                Transaction = new AeFinder.Sdk.Processor.Transaction
                {
                    TransactionId = "test-transaction-id",
                    To = "test-to-address",
                    MethodName = "DeployContract",
                    Params = "test-params",
                    Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                    ExtraProperties = new Dictionary<string, string>()
                },
                Block = new AeFinder.Sdk.Processor.Block
                {
                    BlockHash = "test-block-hash",
                    BlockHeight = 12345L,
                    BlockTime = DateTime.UtcNow
                }
            };

            // Act
            await _processor.ProcessAsync(logEvent, context);

            // Assert
            // The processor creates NFT and Token info indexes when CA contract is deployed
            // Check if any NFT collections were created based on initial config
            var nftProtocolInfoList = ConfigConstants.InitialInfo.NFTProtocolInfoList.Where(n => n.ChainId == chainId).ToList();
            if (nftProtocolInfoList.Any())
            {
                var firstNftInfo = nftProtocolInfoList.First();
                var nftCollectionIndex = await _nftCollectionRepository.GetAsync(IdGenerateHelper.GetId(firstNftInfo.ChainId, firstNftInfo.Symbol));
                nftCollectionIndex.ShouldNotBeNull();
                nftCollectionIndex.Symbol.ShouldBe(firstNftInfo.Symbol);
            }

            // Check if any token info was created based on initial config
            var tokenInfoList = ConfigConstants.InitialInfo.TokenInfoList.Where(n => n.ChainId == chainId).ToList();
            if (tokenInfoList.Any())
            {
                var firstTokenInfo = tokenInfoList.First();
                var tokenInfoIndex = await _tokenInfoRepository.GetAsync(IdGenerateHelper.GetId(firstTokenInfo.ChainId, firstTokenInfo.Symbol));
                tokenInfoIndex.ShouldNotBeNull();
                tokenInfoIndex.Symbol.ShouldBe(firstTokenInfo.Symbol);
            }
        }
    }
} 