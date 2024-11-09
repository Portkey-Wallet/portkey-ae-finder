namespace PortkeyApp.Configs;

public static partial class ConfigFile
{
    public static string MainNetConfigurationJson = """
{
    "ContractInfos": [
        {
            "ChainId": "AELF",
            "GenesisContractAddress": "pykr77ft9UUKJZLVq15wCH8PinBSjVRQ12sD1Ayq92mKFsJ1i",
            "CAContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "AnotherCAContractAddress": "28PcLvP41ouUd6UNGsNRvKpkFTe6am34nPy4YPsWUJnZNwUvzM",
            "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
            "NFTContractAddress": "2VTusxv6BN4SQDroitnWyLyQHWiwEhdWU76PPiGBqt5VbyF27J"
        },
        {
            "ChainId": "tDVV",
            "GenesisContractAddress": "2dtnkWDyJJXeDRcREhKSZHrYdDGMbn3eus5KYpXonfoTygFHZm",
            "CAContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "AnotherCAContractAddress": "2cLA9kJW3gdHuGoYNY16Qir69J3Nkn6MSsuYxRkUHbz4SG2hZr",
            "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
            "BingoGameContractAddress": "fU9csLqXtnSbcyRJs3fPYLFTz2S9EZowUqkYe4zrJgp1avXK2",
            "BeangoTownContractAddress": "C7ZUPUHDwG2q3jR5Mw38YoBHch2XiZdiK6pBYkdhXdGrYcXsb",
            "NFTContractAddress": "2ZpYFeE4yWjrcKLBoj1iwbfYnbo9hK7exvfGTdqcq77QSxpzNH"
        }
    ],
    "Inscriptions": [
        "ELEPHANT-0",
        "CHEFCURRY-0",
        "NEWBEE-0",
        "DADDYCHILL-0",
        "DDDDDDDDDDDDAY-0",
        "ILLUSTRATION-0",
        "SFFSFJSKFJKSJFKSJFKS-0",
        "NAGAS-0"
    ],
    "InitialInfo": {
        "TokenInfoList": [
            {
                "ChainId": "AELF",
                "Symbol": "ELF",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 100000000000000000,
                "TokenName": "AElf Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "ETH",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 11934405100000000,
                "TokenName": "Ethereum",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "BNB",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 15586517900000000,
                "TokenName": "BNB",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "USDC",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 6,
                "TotalSupply": 30044395970599550,
                "TokenName": "USD Coin",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "USDT",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 6,
                "TotalSupply": 81061272679788560,
                "TokenName": "Tether USD",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "DAI",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 505871838980337100,
                "TokenName": "Dai Stablecoin",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "PORT",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 0,
                "TotalSupply": 10000000000000000,
                "TokenName": "Port All Project Token",
                "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "VOTE",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 100000000000000000,
                "TokenName": "VOTE Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "SHARE",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 100000000000000000,
                "TokenName": "SHARE Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "WRITE",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "WRITE Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "RAM",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "RAM Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "CPU",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "CPU Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv10wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "LOT",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 100000000000000000,
                "TokenName": "aelf Lottery Token",
                "Issuer": "2vB6223CorAU79ZMtFpva4LC8DrYuyiSndxvZLCKc61CFvjGbP",
                "IsBurnable": true,
                "IssueChainId": 1866392
            },
            {
                "ChainId": "AELF",
                "Symbol": "READ",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "READ Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv10wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "STORAGE",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "STORAGE Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv11wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "TRAFFIC",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "TRAFFIC Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv12wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "NET",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "NET Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv13wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "AELF",
                "Symbol": "DISK",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "DISK Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv14wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "ELF",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 100000000000000000,
                "TokenName": "AElf Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "ETH",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 11934405100000000,
                "TokenName": "Ethereum",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "BNB",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 15586517900000000,
                "TokenName": "BNB",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "USDC",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 6,
                "TotalSupply": 30044395970599550,
                "TokenName": "USD Coin",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "USDT",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 6,
                "TotalSupply": 81061272679788560,
                "TokenName": "Tether USD",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "DAI",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 505871838980337100,
                "TokenName": "Dai Stablecoin",
                "Issuer": "2Fo6mvHWqhc5w1vBdai2YLdKrTEkdjFFLxXHy9XD8cxxxtfz73",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "PORT",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 0,
                "TotalSupply": 10000000000000000,
                "TokenName": "Port All Project Token",
                "Issuer": "aeXhTqNwLWxCG6AzxwnYKrPMWRrzZBskW3HWVD9YREMx1rJxG",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "WRITE",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "WRITE Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "RAM",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "RAM Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv9wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "CPU",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "CPU Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv10wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "LOT",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 100000000000000000,
                "TokenName": "aelf Lottery Token",
                "Issuer": "2vB6223CorAU79ZMtFpva4LC8DrYuyiSndxvZLCKc61CFvjGbP",
                "IsBurnable": true,
                "IssueChainId": 1866392
            },
            {
                "ChainId": "tDVV",
                "Symbol": "READ",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "READ Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv10wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "STORAGE",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "STORAGE Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv11wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "TRAFFIC",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "TRAFFIC Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv12wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "NET",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "NET Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv13wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            },
            {
                "ChainId": "tDVV",
                "Symbol": "DISK",
                "BlockHash": "73b6d1064013c0b34e6b4783d04a7c550863c95bd78e9b372fe8372577e290e8",
                "PreviousBlockHash": "0000000000000000000000000000000000000000000000000000000000000000",
                "BlockHeight": 1,
                "TokenContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
                "Decimals": 8,
                "TotalSupply": 50000000000000000,
                "TokenName": "DISK Token",
                "Issuer": "cxZuMcWFE7we6CNESgh9Y4M6n7eM5JgFAgVMPrTNEv14wFEvQp",
                "IsBurnable": true,
                "IssueChainId": 9992731
            }
        ],
        "NFTProtocolInfoList": []
    },
    "CAHolderTransactionInfos": [
        {
            "ChainId": "AELF",
            "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
            "MethodName": "Transfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
            "MethodName": "CrossChainTransfer",
            "EventNames": [
                "CrossChainTransferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
            "MethodName": "CrossChainReceiveToken",
            "EventNames": [
                "CrossChainReceived"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2VTusxv6BN4SQDroitnWyLyQHWiwEhdWU76PPiGBqt5VbyF27J",
            "MethodName": "Transfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE",
            "MethodName": "Approve",
            "EventNames": [
                "Approved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ManagerTransfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "AddManagerInfo",
            "EventNames": [
                "ManagerInfoAdded"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "RemoveManagerInfo",
            "EventNames": [
                "ManagerInfoRemoved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "RemoveOtherManagerInfo",
            "EventNames": [
                "ManagerInfoRemoved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "UpdateManagerInfos",
            "EventNames": [
                "ManagerInfoUpdated"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "SocialRecovery",
            "EventNames": [
                "ManagerInfoSocialRecovered"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ManagerForwardCall",
            "EventNames": [
                "CrossChainTransferred",
                "CrossChainReceived",
                "Transferred",
                "GuardianAdded",
                "GuardianRemoved",
                "GuardianUpdated",
                "LoginGuardianAdded",
                "LoginGuardianRemoved",
                "Approved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "AddGuardian",
            "EventNames": [
                "GuardianAdded"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "RemoveGuardian",
            "EventNames": [
                "GuardianRemoved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "UpdateGuardian",
            "EventNames": [
                "GuardianUpdated"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "UnsetGuardianForLogin",
            "EventNames": [
                "LoginGuardianRemoved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "SetGuardianForLogin",
            "EventNames": [
                "LoginGuardianAdded"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "CreateCAHolder",
            "EventNames": [
                "CAHolderCreated"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ManagerApprove",
            "EventNames": [
                "ManagerApproved"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "SetTransferLimit",
            "EventNames": [
                "TransferLimitChanged"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2w13DqbuuiadvaSY2ZyKi2UoXg354zfHLM3kwRKKy85cViw4ZF",
            "MethodName": "TransferToken",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "AtCnocGN47ZCUscwHYxJNh8G8jVmbgjgy1MR62uoXGohd67wu",
            "MethodName": "CreateCryptoBox",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "AtCnocGN47ZCUscwHYxJNh8G8jVmbgjgy1MR62uoXGohd67wu",
            "MethodName": "TransferCryptoBoxes",
            "EventNames": [
                "Transferred"
            ],
            "MultiTransaction": true
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "AtCnocGN47ZCUscwHYxJNh8G8jVmbgjgy1MR62uoXGohd67wu",
            "MethodName": "RefundCryptoBox",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "28PcLvP41ouUd6UNGsNRvKpkFTe6am34nPy4YPsWUJnZNwUvzM",
            "MethodName": "ManagerTransfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "28PcLvP41ouUd6UNGsNRvKpkFTe6am34nPy4YPsWUJnZNwUvzM",
            "MethodName": "ManagerForwardCall",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "AELF",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ReportPreCrossChainSyncHolderInfo",
            "EventNames": [
                "PreCrossChainSyncHolderInfoCreated"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
            "MethodName": "Transfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
            "MethodName": "CrossChainTransfer",
            "EventNames": [
                "CrossChainTransferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
            "MethodName": "CrossChainReceiveToken",
            "EventNames": [
                "CrossChainReceived"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
            "MethodName": "Approve",
            "EventNames": [
                "Approved"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2ZpYFeE4yWjrcKLBoj1iwbfYnbo9hK7exvfGTdqcq77QSxpzNH",
            "MethodName": "Transfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ManagerTransfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "AddManagerInfo",
            "EventNames": [
                "ManagerInfoAdded"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "RemoveManagerInfo",
            "EventNames": [
                "ManagerInfoRemoved"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "RemoveOtherManagerInfo",
            "EventNames": [
                "ManagerInfoRemoved"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "UpdateManagerInfos",
            "EventNames": [
                "ManagerInfoUpdated"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "SocialRecovery",
            "EventNames": [
                "ManagerInfoSocialRecovered"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ManagerForwardCall",
            "EventNames": [
                "CrossChainTransferred",
                "CrossChainReceived",
                "Transferred",
                "GuardianAdded",
                "GuardianRemoved",
                "GuardianUpdated",
                "LoginGuardianAdded",
                "LoginGuardianRemoved",
                "Approved",
                "Played",
                "Bingoed",
                "Registered"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "CreateCAHolder",
            "EventNames": [
                "CAHolderCreated"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "AddGuardian",
            "EventNames": [
                "GuardianAdded"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "RemoveGuardian",
            "EventNames": [
                "GuardianRemoved"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "UpdateGuardian",
            "EventNames": [
                "GuardianUpdated"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "UnsetGuardianForLogin",
            "EventNames": [
                "LoginGuardianRemoved"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "SetGuardianForLogin",
            "EventNames": [
                "LoginGuardianAdded"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ManagerApprove",
            "EventNames": [
                "ManagerApproved"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "SetTransferLimit",
            "EventNames": [
                "TransferLimitChanged"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "fU9csLqXtnSbcyRJs3fPYLFTz2S9EZowUqkYe4zrJgp1avXK2",
            "MethodName": "Bingo",
            "EventNames": [
                "Bingoed"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "fU9csLqXtnSbcyRJs3fPYLFTz2S9EZowUqkYe4zrJgp1avXK2",
            "MethodName": "Play",
            "EventNames": [
                "Played"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "fU9csLqXtnSbcyRJs3fPYLFTz2S9EZowUqkYe4zrJgp1avXK2",
            "MethodName": "Register",
            "EventNames": [
                "Registered"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "C7ZUPUHDwG2q3jR5Mw38YoBHch2XiZdiK6pBYkdhXdGrYcXsb",
            "MethodName": "Bingo",
            "EventNames": [
                "Bingoed"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "C7ZUPUHDwG2q3jR5Mw38YoBHch2XiZdiK6pBYkdhXdGrYcXsb",
            "MethodName": "Play",
            "EventNames": [
                "Played"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "x4CTSuM8typUbpdfxRZDTqYVa42RdxrwwPkXX7WUJHeRmzE6k",
            "MethodName": "TransferToken",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "25TyYPmWgQVSjAG3FnTn1quWCbKgvbFpBxTPnoDkK2xSUtNBey",
            "MethodName": "CreateCryptoBox",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "25TyYPmWgQVSjAG3FnTn1quWCbKgvbFpBxTPnoDkK2xSUtNBey",
            "MethodName": "TransferCryptoBoxes",
            "EventNames": [
                "Transferred"
            ],
            "MultiTransaction": true
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "25TyYPmWgQVSjAG3FnTn1quWCbKgvbFpBxTPnoDkK2xSUtNBey",
            "MethodName": "RefundCryptoBox",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2cLA9kJW3gdHuGoYNY16Qir69J3Nkn6MSsuYxRkUHbz4SG2hZr",
            "MethodName": "ManagerTransfer",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2cLA9kJW3gdHuGoYNY16Qir69J3Nkn6MSsuYxRkUHbz4SG2hZr",
            "MethodName": "ManagerForwardCall",
            "EventNames": [
                "Transferred"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2UthYi7AHRdfrqc1YCfeQnjdChDLaas65bW4WxESMGMojFiXj9",
            "MethodName": "ReportPreCrossChainSyncHolderInfo",
            "EventNames": [
                "PreCrossChainSyncHolderInfoCreated"
            ]
        },
        {
            "ChainId": "tDVV",
            "ContractAddress": "2c4XXgGbtvESxLW3SQTY8zfBtWGT2xDHX14w3r5C4bHTo1LhAj",
            "MethodName": "BatchTransfer",
            "EventNames": [
                "Transferred"
            ],
            "MultiTransaction": true
        }
    ]
}
""";
}