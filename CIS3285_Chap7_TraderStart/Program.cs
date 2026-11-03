using System.Reflection;

namespace CIS3285_Chap7_TraderStart;

class Program
{
    static void Main(string[] args)
    {
        using var tradeStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("CIS3285_Chap7_TraderStart.trades.txt");
        if (tradeStream is null)
        {
            Console.Error.WriteLine("Could not find the embedded trades.txt file.");
            return;
        }

        var tradeProcessor = new TradeProcessor();
        tradeProcessor.ProcessTrades(tradeStream);

    }
}
