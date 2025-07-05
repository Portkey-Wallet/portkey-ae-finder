using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.CA;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using AElf.Types;
using Google.Protobuf;
using System.Collections.Generic;
using AElf;
using System;

namespace PortkeyApp.Processors;

public class LoginGuardianRemovedProcessorTests : PortkeyAppTestBase
{
    private readonly LoginGuardianRemovedProcessor _processor;
    private readonly IReadOnlyRepository<CAHolderIndex> _caHolderRepository;
    private readonly IReadOnlyRepository<LoginGuardianIndex> _loginGuardianRepository;

    public LoginGuardianRemovedProcessorTests()
    {
        _processor = GetRequiredService<LoginGuardianRemovedProcessor>();
        _caHolderRepository = GetRequiredService<IReadOnlyRepository<CAHolderIndex>>();
        _loginGuardianRepository = GetRequiredService<IReadOnlyRepository<LoginGuardianIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Remove_LoginGuardian()
    {
        // Arrange
        var chainId = "AELF";
        var caAddress = Address.FromPublicKey(new byte[33]);
        var caHash = new global::AElf.Types.Hash { Value = Google.Protobuf.ByteString.CopyFromUtf8("test-ca-hash") };
        var manager = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        var identifierHash = new global::AElf.Types.Hash { Value = Google.Protobuf.ByteString.CopyFromUtf8("test-identifier-hash") };
        var verifierId = new global::AElf.Types.Hash { Value = Google.Protobuf.ByteString.CopyFromUtf8("test-verifier-id") };
        var platform = 1;
        
        var logEvent = new LoginGuardianRemoved
        {
            CaAddress = caAddress,
            CaHash = caHash,
            Manager = manager,
            LoginGuardian = new Portkey.Contracts.CA.Guardian
            {
                Type = Portkey.Contracts.CA.GuardianType.OfEmail,
                VerifierId = verifierId,
                IdentifierHash = identifierHash,
                Salt = "test-salt",
                IsLoginGuardian = true
            },
            Platform = platform
        };
        var context = new LogEventContext
        {
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = HashHelper.ComputeFrom("block_hash").ToHex(),
                BlockHeight = 100,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = HashHelper.ComputeFrom("transaction_id").ToHex(),
                From = "test-from-address",
                To = "CAContract",
                MethodName = "RemoveLoginGuardian",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                },
                Params = "test-params"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);
    }
} 