using GameMaker.GM8Project;
using GameMaker.GML;
using System.Diagnostics;

namespace GMKLibTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var reader = new ProjectReader(@"D:\Games\I Wanna\I Wanna Kill the Kamilia 3 EZ\I Wanna Kill the Kamilia 3 EZ_online.gm81");
            var project = reader.ReadProject();

            foreach (var scr in project.Scripts)
            {
                var ast = Parser.Parse(project, scr.Name, scr.Code);
                Console.WriteLine(ast);
                Console.WriteLine();
            }
        }
    }
}