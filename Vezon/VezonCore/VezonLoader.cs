using System.Reflection;

namespace VezonCore
{
    public class VezonLoader
    {
        public void OnBoot(string[] args)
        {
            Global.WriteLine($"{Global.ProjectName} {Global.CurLanguage?.LoadValueString("Inertia_Version")} {Assembly.GetEntryAssembly()!.GetName().Version}");
            Global.WriteLine("Main Class loading....");
            Global.Instance.VezonLoader = this;
            Global.Instance.Main();
        }
    }
}
