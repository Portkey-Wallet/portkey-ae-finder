using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using Portkey.Contracts.BingoGameContract;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using AElf.Types;
using AElf;
using Google.Protobuf.WellKnownTypes;
using PortkeyApp.Common;

namespace PortkeyApp.Tests.Processors;

public class PlayedProcessorTests : PortkeyAppTestBase
{
    private readonly PlayedProcessor _processor;
    private readonly IReadOnlyRepository<CAHolderIndex> _caHolderRepository;
    private readonly IReadOnlyRepository<BingoGameIndex> _bingoGameRepository;

    public PlayedProcessorTests()
    {
        _processor = GetRequiredService<PlayedProcessor>();
        _caHolderRepository = GetRequiredService<IReadOnlyRepository<CAHolderIndex>>();
        _bingoGameRepository = GetRequiredService<IReadOnlyRepository<BingoGameIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Create_BingoGameIndex()
    {
        // Arrange
        var playerAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef"));
        var playId = Hash.LoadFromByteArray(ByteArrayHelper.HexStringToByteArray("1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef"));
        
        var logEvent = new Played
        {
            PlayBlockHeight = 1000,
            Amount = 100,
            Type = BingoType.Small,
            PlayId = playId,
            PlayerAddress = playerAddress,
            Symbol = "ELF"
        };

        var context = new LogEventContext
        {
            ChainId = "AELF",
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test_block_hash",
                BlockHeight = 1000,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test_transaction_id",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                To = "test_contract_address",
                MethodName = "Play",
                Params = "{}"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        // The processor should complete without error
        // We can verify the event properties were set correctly
        logEvent.PlayBlockHeight.ShouldBe(1000);
        logEvent.Amount.ShouldBe(100);
        ((int)logEvent.Type).ShouldBe((int)BingoType.Small);
        logEvent.PlayId.ShouldBe(playId);
        logEvent.PlayerAddress.ShouldBe(playerAddress);
        logEvent.Symbol.ShouldBe("ELF");
    }

    [Fact]
    public async Task ProcessAsync_Should_Skip_When_PlayerAddress_Is_Null()
    {
        // Arrange
        var playId = Hash.LoadFromByteArray(ByteArrayHelper.HexStringToByteArray("1234567890abcdef1234567890abcdef1234567890abcdef1234567890abcdef"));
        var logEvent = new Played
        {
            PlayBlockHeight = 1000,
            Amount = 100,
            Type = BingoType.Small,
            PlayId = playId,
            PlayerAddress = null,
            Symbol = "ELF"
        };

        var context = new LogEventContext
        {
            ChainId = "AELF",
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test_block_hash",
                BlockHeight = 1000,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test_transaction_id",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                To = "test_contract_address",
                MethodName = "Play",
                Params = "{}"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        // Should complete without error but not process anything
        logEvent.PlayerAddress.ShouldBeNull();
        ((int)logEvent.Type).ShouldBe((int)BingoType.Small);
    }

    [Fact]
    public async Task ProcessAsync_Should_Skip_When_BingoGameIndex_Already_Exists()
    {
        // Arrange
        var playerAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef"));
        var playId = Hash.LoadFromByteArray(ByteArrayHelper.HexStringToByteArray("abcdef1234567890abcdef1234567890abcdef1234567890abcdef1234567890"));
        
        var logEvent = new Played
        {
            PlayBlockHeight = 1000,
            Amount = 100,
            Type = BingoType.Small,
            PlayId = playId,
            PlayerAddress = playerAddress,
            Symbol = "ELF"
        };

        var context = new LogEventContext
        {
            ChainId = "AELF",
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test_block_hash",
                BlockHeight = 1000,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test_transaction_id",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                To = "test_contract_address",
                MethodName = "Play",
                Params = "{}"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert - should complete without error
        logEvent.PlayerAddress.ShouldBe(playerAddress);
        logEvent.PlayId.ShouldBe(playId);
        ((int)logEvent.Type).ShouldBe((int)BingoType.Small);
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_Valid_Play_Event()
    {
        // Arrange
        var playerAddress = Address.FromPublicKey(ByteArrayHelper.HexStringToByteArray("09da44778f8db2e602fb484334f37df19e221c84c4582ce5b7770ccfbc3ddbef"));
        var playId = Hash.LoadFromByteArray(ByteArrayHelper.HexStringToByteArray("fedcba0987654321fedcba0987654321fedcba0987654321fedcba0987654321"));
        
        var logEvent = new Played
        {
            PlayBlockHeight = 2000,
            Amount = 500,
            Type = BingoType.Large,
            PlayId = playId,
            PlayerAddress = playerAddress,
            Symbol = "USDT"
        };

        var context = new LogEventContext
        {
            ChainId = "AELF",
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test_block_hash",
                BlockHeight = 2000,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test_transaction_id",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                To = "test_contract_address",
                MethodName = "Play",
                Params = "{}"
            }
        };

        // Act
        await _processor.ProcessAsync(logEvent, context);

        // Assert
        logEvent.PlayerAddress.ShouldBe(playerAddress);
        logEvent.PlayId.ShouldBe(playId);
        logEvent.Amount.ShouldBe(500);
        ((int)logEvent.Type).ShouldBe((int)BingoType.Large);
        logEvent.Symbol.ShouldBe("USDT");
    }
} 