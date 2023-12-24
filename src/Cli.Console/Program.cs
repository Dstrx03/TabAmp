using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Cli.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var reader = new PocSerialAsynchronousFileReader("sample.gp5");
        var bytesCount = 4;

        Stopwatch stopWatch = new Stopwatch();

        System.Console.WriteLine("Press any key...");
        System.Console.ReadKey(true);
        System.Console.Clear();
        System.Console.WriteLine("Processing...");

        stopWatch.Start();
        while (reader.Position + bytesCount <= reader.Length)
        {
            var res = await reader.ReadBytesAsync(bytesCount);
        }

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);

        System.Console.WriteLine(elapsedTime);
    }
}
