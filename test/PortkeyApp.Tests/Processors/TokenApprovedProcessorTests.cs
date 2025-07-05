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
using AElf.Contracts.MultiToken;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using System.Collections.Generic;
using System;
using Volo.Abp.ObjectMapping;
using AElf;

namespace PortkeyApp.Tests.Processors
{
    public class TokenApprovedProcessorTests : PortkeyAppTestBase
    {
        private readonly TokenApprovedProcessor _processor;
        private readonly IRepository<CAHolderTokenApprovedIndex> _tokenApprovedRepository;
        private readonly IRepository<CAHolderIndex> _caHolderRepository;

        public TokenApprovedProcessorTests()
        {
            _processor = GetRequiredService<TokenApprovedProcessor>();
            _tokenApprovedRepository = GetRequiredService<IRepository<CAHolderTokenApprovedIndex>>();
            _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
        }

        [Fact]
        public async Task ProcessAsync_Should_Create_TokenApprovedIndex()
        {
            // Arrange
            var chainId = "AELF";
            var owner = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
            var spender = Address.FromPublicKey(new byte[32] { 32, 31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21, 20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 });
            var symbol = "ELF";
            var amount = 1000000000L;
            
            // Create required CAHolderIndex first
            var caHolderIndex = new CAHolderIndex
            {
                Id = IdGenerateHelper.GetId(chainId, owner.ToBase58()),
                CAAddress = owner.ToBase58(),
                CAHash = HashHelper.ComputeFrom(owner.ToBase58()).ToHex()
            };
            await _caHolderRepository.AddOrUpdateAsync(caHolderIndex);
            
            var logEvent = new Approved
            {
                Owner = owner,
                Spender = spender,
                Symbol = symbol,
                Amount = amount
            };
            
            var context = new LogEventContext
            {
                ChainId = chainId,
                Transaction = new AeFinder.Sdk.Processor.Transaction
                {
                    TransactionId = "test-transaction-id",
                    To = "test-to-address",
                    MethodName = "ApproveToken",
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
            var tokenApprovedIndex = await _tokenApprovedRepository.GetAsync(IdGenerateHelper.GetId(chainId, owner.ToBase58(), spender.ToBase58(), symbol));
            tokenApprovedIndex.ShouldNotBeNull();
            tokenApprovedIndex.CAAddress.ShouldBe(owner.ToBase58());
            tokenApprovedIndex.Spender.ShouldBe(spender.ToBase58());
            tokenApprovedIndex.Symbol.ShouldBe(symbol);
            tokenApprovedIndex.BatchApprovedAmount.ShouldBe(amount);
        }
    }
} 