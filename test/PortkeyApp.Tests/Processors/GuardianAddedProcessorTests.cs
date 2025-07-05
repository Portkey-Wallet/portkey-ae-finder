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
using Portkey.Contracts.CA;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using System.Collections.Generic;
using System;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Domain.Repositories;

namespace PortkeyApp.Tests.Processors;

public class GuardianAddedProcessorTests : PortkeyAppTestBase
{
    private readonly GuardianAddedProcessor _processor;
    private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;

    public GuardianAddedProcessorTests()
    {
        _processor = GetRequiredService<GuardianAddedProcessor>();
        _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Update_CAHolderIndex()
    {
        // Arrange
        var chainId = "AELF";
        var caAddress = Address.FromPublicKey(new byte[33]);
        var caHash = new Hash { Value = ByteString.CopyFromUtf8("test_ca_hash") };
        var identifierHash = new Hash { Value = ByteString.CopyFromUtf8("test_identifier_hash") };
        var verifierId = new Hash { Value = ByteString.CopyFromUtf8("test_verifier_id") };
        
        // Pre-create CAHolderIndex
        var caHolderIndex = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, caAddress.ToBase58()),
            CAHash = caHash.ToHex(),
            CAAddress = caAddress.ToBase58(),
            Guardians = new List<PortkeyApp.Entities.Guardian>()
        };
        await _caHolderRepository.AddOrUpdateAsync(caHolderIndex);

        var logEvent = new GuardianAdded
        {
            CaAddress = caAddress,
            CaHash = caHash,
            GuardianAdded_ = new Portkey.Contracts.CA.Guardian
            {
                Type = GuardianType.OfEmail,
                VerifierId = verifierId,
                IdentifierHash = identifierHash,
                Salt = "test_salt",
                IsLoginGuardian = true
            }
        };

        var context = new LogEventContext
        {
            ChainId = chainId,
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test_transaction_id",
                To = "test_to",
                MethodName = "test_method",
                Params = "test_params",
                Status = TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>()
            },
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test_block_hash",
                BlockHeight = 12345,
                BlockTime = DateTime.UtcNow
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        var updatedIndex = await _caHolderRepository.GetAsync(IdGenerateHelper.GetId(chainId, caAddress.ToBase58()));
        updatedIndex.ShouldNotBeNull();
        updatedIndex.Guardians.Count.ShouldBe(1);
        updatedIndex.Guardians[0].Type.ShouldBe((int)GuardianType.OfEmail);
        updatedIndex.Guardians[0].VerifierId.ShouldBe(verifierId.ToHex());
        updatedIndex.Guardians[0].IdentifierHash.ShouldBe(identifierHash.ToHex());
        updatedIndex.Guardians[0].IsLoginGuardian.ShouldBe(true);
    }
} 