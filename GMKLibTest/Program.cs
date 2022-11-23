using GameMaker.GM8Project;
using System.Diagnostics;

namespace GMKLibTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            var reader = new ProjectReader(@"D:\Games\I Wanna\I Wanna Kill the Kamilia 3 EZ\I Wanna Kill the Kamilia 3 EZ_online.gm81");
            var project = reader.ReadProject();
            Console.WriteLine(sw.ElapsedMilliseconds);

        }
    }
}