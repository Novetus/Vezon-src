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
            Global.WriteLine($"{Global.ProjectName} {Global.CurLanguage?.LoadValueString("Inertia_Version")} {Assembly.GetEntryAssembly()!.GetName().Version}");
            Global.WriteLine("Main Class loading....");
            Global.Instance.VezonLoader = this;
            Global.Instance.Main();
        }
    }
}