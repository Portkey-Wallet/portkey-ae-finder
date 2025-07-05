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

public class TokenIssuedProcessorTests : PortkeyAppTestBase
{
    private readonly TokenIssuedProcessor _processor;
    private readonly IRepository<CAHolderIndex> _caHolderRepository;

    public TokenIssuedProcessorTests()
    {
        _processor = GetRequiredService<TokenIssuedProcessor>();
        _caHolderRepository = GetRequiredService<IRepository<CAHolderIndex>>();
    }

    [Fact]
    public async Task ProcessAsync_Should_Update_TokenSupply_And_Balance()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var issueAmount = 1000L;
        
        // 创建有效的测试地址
        var toAddress = Address.FromPublicKey(new byte[33]);
        
        // 创建CAHolderIndex实体
        var caHolderIndex = new CAHolderIndex
        {
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58()),
            CAHash = "test-hash",
            CAAddress = toAddress.ToBase58(),
            Creator = "creator-address"
        };
        await GetRequiredService<IRepository<CAHolderIndex>>().AddOrUpdateAsync(caHolderIndex);
        
        // 创建TokenInfoIndex实体  
        var tokenInfoIndex = new TokenInfoIndex
        {
            Id = IdGenerateHelper.GetId(chainId, symbol),
            Symbol = symbol,
            Supply = 500000,
            TotalSupply = 1000000,
            TokenName = "ELF Token",
            Decimals = 8,
            IsBurnable = true,
            IssueChainId = 1,
            Type = TokenType.Token,
            TokenContractAddress = "token-contract-address"
        };
        await GetRequiredService<IRepository<TokenInfoIndex>>().AddOrUpdateAsync(tokenInfoIndex);
        
        // 创建CAHolderTokenBalanceIndex实体
        var balanceIndex = new CAHolderTokenBalanceIndex
        {
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol),
            CAAddress = toAddress.ToBase58(),
            Balance = 200,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(balanceIndex);
        
        // Act - 测试Token发行
        var logEvent = new Issued
        {
            Symbol = symbol,
            Amount = issueAmount,
            To = toAddress
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
                From = "test-from-address",
                To = toAddress.ToBase58(),
                MethodName = "Issue",
                Params = "test-params",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
        
        await _processor.ProcessAsync(logEvent, context);
        
        // Assert - 验证Token供应量增加
        var updatedTokenInfo = await GetRequiredService<IRepository<TokenInfoIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, symbol));
        
        updatedTokenInfo.ShouldNotBeNull();
        updatedTokenInfo.Supply.ShouldBe(501000); // 500000 + 1000 = 501000
        
        // 验证Token余额增加
        var updatedBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol));
        
        updatedBalance.ShouldNotBeNull();
        updatedBalance.Balance.ShouldBe(1200); // 200 + 1000 = 1200
    }

    [Fact]
    public async Task ProcessAsync_Should_Handle_NonCAHolder_Issue()
    {
        // Arrange
        var chainId = "AELF";
        var symbol = "ELF";
        var issueAmount = 75L;
        
        // 创建有效的测试地址（非CAHolder）
        var toAddress = Address.FromPublicKey(new byte[32] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 });
        
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
            Id = IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol),
            CAAddress = toAddress.ToBase58(),
            Balance = 25,
            TokenInfo = tokenInfoIndex
        };
        await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>().AddOrUpdateAsync(tokenBalance);
        
        // Act - 测试非CAHolder发行
        var logEvent = new Issued
        {
            Symbol = symbol,
            Amount = issueAmount,
            To = toAddress
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
                From = "test-from-address",
                To = toAddress.ToBase58(),
                MethodName = "Issue",
                Params = "test-params",
                Status = AeFinder.Sdk.Processor.TransactionStatus.Mined,
                ExtraProperties = new Dictionary<string, string>
                {
                    ["TransactionFee"] = "1000000"
                }
            }
        };
        
        await _processor.ProcessAsync(logEvent, context);
        
        // Assert - 验证非CAHolder发行后余额不变（处理器只处理CAHolder）
        var updatedBalance = await GetRequiredService<IRepository<CAHolderTokenBalanceIndex>>()
            .GetAsync(IdGenerateHelper.GetId(chainId, toAddress.ToBase58(), symbol));
        
        updatedBalance.ShouldNotBeNull();
        updatedBalance.Balance.ShouldBe(25); // 余额应该保持不变，因为处理器只处理CAHolder
    }
} 