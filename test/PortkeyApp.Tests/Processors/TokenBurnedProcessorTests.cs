using System.Threading.Tasks;
using Xunit;
using Shouldly;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using AElf.Types;
using PortkeyApp.Processors;
using PortkeyApp.Entities;
using PortkeyApp.Common;
using System.Collections.Generic;
using System;

namespace PortkeyApp.Tests.Processors;

public class TokenBurnedProcessorTests : PortkeyAppTestBase
{
    private readonly TokenBurnedProcessor _processor;
    private readonly IRepository<CAHolderIndex> _caHolderRepository;

    public TokenBurnedProcessorTests()
    {
        _processor = GetRequiredService<TokenBurnedProcessor>();
        _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Update_TokenBalance()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var burnAmount = 30L;
        
        // 创建有效的测试地址
        var burnerAddress = Address.FromPublicKey(new byte[33]);
        
        // 创建CAHolderIndex实体
        var caHolder = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, burnerAddress.ToBase58()),
            CAHash = "test-hash",
            CAAddress = burnerAddress.ToBase58(),
            Creator = "creator-address"
        };
        await GetRequiredService<IRepository<CAHolderIndex>>().AddOrUpdateAsync(caHolder);
        
        // 创建TokenInfoIndex实体  
        var tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(chainId, symbol),
            Symbol = symbol,
            Supply = 1000000,
            TotalSupply = 1000000,
            TokenName = "ELF Token",
            Decimals = 8,
            IsBurnable = true,
            IssueChainId = 1,
            Type = TokenType.Token,
            TokenContractAddress = "token-contract-address"
        };
        await GetRequiredService<IRepository<TokenInfoIndex>>().AddOrUpdateAsync(tokenInfoIndex);
        
        // 创建TokenBalance
        var tokenBalance = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, burnerAddress.ToBase58(), symbol),
            CAAddress = burnerAddress.ToBase58(),
            Balance = 100,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(tokenBalance);
        
        // Act - 测试销毁
        var logEvent = new Burned
        {
            Symbol = symbol,
            Amount = burnAmount,
            Burner = burnerAddress
        };
        var context = new LogEventContext 
        { 
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test-block-hash",
                BlockHeight = 12345L,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test-transaction-id",
                From = burnerAddress.ToBase58(),
                To = "test-to-address",
                MethodName = "Burn",
                Params = "test-params",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
        
        await _processor.ProcessAsync(logEvent, context);
        
        // Assert - 验证销毁后的余额变化
        var updatedBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, burnerAddress.ToBase58(), symbol));
        
        updatedBalance.ShouldNotBeNull();
        updatedBalance.Balance.ShouldBe(70); // 100 - 30 = 70
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_NonCAHolder_Burn()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var burnAmount = 40L;
        
        // 创建有效的测试地址（非CAHolder）
        var burnerAddress = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        
        // 创建TokenInfoIndex实体  
        var tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(chainId, symbol),
            Symbol = symbol,
            Supply = 1000000,
            TotalSupply = 1000000,
            TokenName = "ELF Token",
            Decimals = 8,
            IsBurnable = true,
            IssueChainId = 1,
            Type = TokenType.Token,
            TokenContractAddress = "token-contract-address"
        };
        await GetRequiredService<IRepository<TokenInfoIndex>>().AddOrUpdateAsync(tokenInfoIndex);
        
        // 创建TokenBalance（使用原始地址）
        var tokenBalance = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, burnerAddress.ToBase58(), symbol),
            CAAddress = burnerAddress.ToBase58(),
            Balance = 80,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(tokenBalance);
        
        // Act - 测试非CAHolder销毁
        var logEvent = new Burned
        {
            Symbol = symbol,
            Amount = burnAmount,
            Burner = burnerAddress
        };
        var context = new LogEventContext 
        { 
            ChainId = chainId,
            Block = new AeFinder.Sdk.Processor.Block
            {
                BlockHash = "test-block-hash-2",
                BlockHeight = 12346L,
                BlockTime = DateTime.UtcNow
            },
            Transaction = new AeFinder.Sdk.Processor.Transaction
            {
                TransactionId = "test-transaction-id-2",
                From = burnerAddress.ToBase58(),
                To = "test-to-address",
                MethodName = "Burn",
                Params = "test-params",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
        
        await _processor.ProcessAsync(logEvent, context);
        
        // Assert - 验证非CAHolder销毁后余额不变（处理器只处理CAHolder）
        var updatedBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, burnerAddress.ToBase58(), symbol));
        
        updatedBalance.ShouldNotBeNull();
        updatedBalance.Balance.ShouldBe(80); // 余额应该保持不变，因为处理器只处理CAHolder
    }
} 