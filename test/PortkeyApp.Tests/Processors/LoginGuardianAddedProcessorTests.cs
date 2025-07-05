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
using PortkeyApp.Common;
using PortkeyApp.Entities;
using PortkeyApp.Processors;
using Volo.Abp.ObjectMapping;
using System.Collections.Generic;
using System;
using Volo.Abp.Domain.Repositories;

namespace PortkeyApp.Tests.Processors;

public class LoginGuardianAddedProcessorTests : PortkeyAppTestBase
{
    private readonly LoginGuardianAddedProcessor _processor;
    private readonly AeFinder.Sdk.IRepository<CAHolderIndex> _caHolderRepository;
    private readonly AeFinder.Sdk.IRepository<LoginGuardianIndex> _loginGuardianRepository;

    public LoginGuardianAddedProcessorTests()
    {
        _processor = GetRequiredService<LoginGuardianAddedProcessor>();
        _caHolderRepository = GetRequiredService<AeFinder.Sdk.IRepository<CAHolderIndex>>();
        _loginGuardianRepository = GetRequiredService<AeFinder.Sdk.IRepository<LoginGuardianIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Update_LoginGuardianIndex()
    {
        // Arrange
        var chainId = "AELF";
        var caAddress = Address.FromPublicKey(new byte[33]);
        var caHash = new Hash { Value = ByteString.CopyFromUtf8("test_ca_hash") };
        var manager = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
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

        var logEvent = new LoginGuardianAdded
        {
            CaAddress = caAddress,
            CaHash = caHash,
            Manager = manager,
            LoginGuardian = new Portkey.Contracts.CA.Guardian
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
        var loginGuardianIndex = await _loginGuardianRepository.GetAsync(
            IdGenerateHelper.GetId(chainId, caAddress.ToBase58(), identifierHash.ToHex(), verifierId.ToHex()));
        loginGuardianIndex.ShouldNotBeNull();
        loginGuardianIndex.CAHash.ShouldBe(caHash.ToHex());
        loginGuardianIndex.CAAddress.ShouldBe(caAddress.ToBase58());
        loginGuardianIndex.Manager.ShouldBe(manager.ToBase58());
        loginGuardianIndex.LoginGuardian.Type.ShouldBe((int)GuardianType.OfEmail);
        loginGuardianIndex.LoginGuardian.VerifierId.ShouldBe(verifierId.ToHex());
        loginGuardianIndex.LoginGuardian.IdentifierHash.ShouldBe(identifierHash.ToHex());
        loginGuardianIndex.LoginGuardian.IsLoginGuardian.ShouldBe(true);
    }
} 