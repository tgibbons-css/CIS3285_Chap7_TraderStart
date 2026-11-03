using Microsoft.Data.SqlClient;
using System.Reflection;

namespace CIS3285_Chap7_TraderStart.Tests;

[TestClass]
[DoNotParallelize]
public class TradeProcessorTests
{
    // Credentials generally should not be hard-coded because source may be shared or committed.
    // This teaching integration test keeps the connection details together to demonstrate its
    // dependency on a real database; replace the password locally and never commit that change.

    private static int CountDbRecords()
    {
        string azureConnectString = @"Server=tcp:cis3285-sql-server.database.windows.net,1433; Initial Catalog = Unit8_TradesDatabase; Persist Security Info=False; User ID=cis3285;Password=Saints2026SQL; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 60;";

        using var connection = new SqlConnection(azureConnectString);
        connection.Open();

        using var command = new SqlCommand("SELECT COUNT(*) FROM trade", connection);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static void ProcessTradeFile(string fileName)
    {
        using var tradeStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"CIS3285_Chap7_TraderStart.Tests.{fileName}");
        Assert.IsNotNull(tradeStream, $"The embedded {fileName} test data was not found.");

        new TradeProcessor().ProcessTrades(tradeStream);

    }

    [TestMethod]
    public void TestNormalFile()
    {
        int countBefore = CountDbRecords();

        ProcessTradeFile("goodtrades.txt");

        int countAfter = CountDbRecords();
        Assert.AreEqual(countBefore + 4, countAfter);
    }

    [TestMethod]
    public void TestNegativeAmountIsNotProcessed()
    {
        int countBefore = CountDbRecords();

        ProcessTradeFile("badtrade_neg_amount.txt");

        int countAfter = CountDbRecords();
        Assert.AreEqual(countBefore, countAfter);
    }
}
