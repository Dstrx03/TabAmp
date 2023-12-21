using TabAmp.Engine.Core.Score;
using TabAmp.Engine.GuitarProFileFormat;

namespace TabAmp.Cli.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var score = new Gp5Score();
            var reader = new SomeFileReader();
            System.Console.WriteLine("Hello, World!\n");
            System.Console.WriteLine($"The Application agent\t{typeof(Program).FullName}");
            System.Console.WriteLine($"The Score class\t\t{score.GetType().FullName}");
            System.Console.WriteLine($"The Reader class\t{reader.GetType().FullName}");
        }
    }
}
