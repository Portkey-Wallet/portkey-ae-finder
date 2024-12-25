namespace PortkeyApp.Common;

public static class BreakHelper
{
    private const long MainChainBreakHeight = 246973747; //2024-10-31T00:39:52
    private const long SideChainBreakHeight = 146579744; //2024-10-31T00:55:53

    public static void CheckBreak(string chainId, long currentHeight)
    {
        if (chainId == "AELF")
        {
            MainChainCheckBreak(currentHeight);
        }
        else
        {
            SideChainCheckBreak(currentHeight);
        }
    }

    private static void MainChainCheckBreak(long currentHeight)
    {
        if (MainChainBreakHeight <= 0)
        {
            return;
        }

        if (MainChainBreakHeight <= currentHeight)
        {
            throw new Exception("Used to wait for node data rollback.");
        }
    }

    private static void SideChainCheckBreak(long currentHeight)
    {
        if (SideChainBreakHeight <= 0)
        {
            return;
        }

        if (SideChainBreakHeight <= currentHeight)
        {
            throw new Exception("Used to wait for node data rollback.");
        }
    }
}