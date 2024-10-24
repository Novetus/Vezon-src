using System.Reflection;
using VezonCore;

namespace VezonLoader
{
    internal class Program : IVezonLoader
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.OnBoot(args);
        }

        void OnBoot(string[] args)
        {
            _G.WriteLine($"{_G.ProjectName} {_G.LoadTranslatedString("Version")} {Assembly.GetEntryAssembly()!.GetName().Version}");
            _G.WriteLine("Main Class loading....");
            _G.VezonInstance.VezonLoader = this;
            _G.VezonInstance.Main();
        }
    }
}