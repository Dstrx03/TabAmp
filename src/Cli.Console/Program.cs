using System.Runtime.CompilerServices;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Cli.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var score = new Gp5Score();
            System.Console.WriteLine("Hello, World!\n");
            System.Console.WriteLine($"The Application agent\t{typeof(Program).FullName}");
            System.Console.WriteLine($"The Score class\t\t{score.GetType().FullName}");
        }
    }
}
