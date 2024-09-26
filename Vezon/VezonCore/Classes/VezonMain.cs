using McMaster.NETCore.Plugins;
using System.Reflection;

namespace VezonCore
{
    public class Vezon
    {
        VezonPluginLoader PluginLoader { get; set; }

        public void Main()
        {
            Console.WriteLine($"Project Vezon Version " + Assembly.GetEntryAssembly().GetName().Version);
            PluginLoader = new VezonPluginLoader();
        }
    }
}
