using McMaster.NETCore.Plugins;
using System.Diagnostics;
using System.Reflection;

namespace VezonCore
{
    public class Vezon
    {
        bool exit = false;
        public IVezonLoader? VezonLoader { get; set; } = null;
        public VezonPluginLoader? PluginLoader { get; set; } = null;

        public void Main()
        {
            _G.WriteLine($"Project Vezon Version {Assembly.GetEntryAssembly()!.GetName().Version}");

            if (VezonLoader != null)
            {
                PluginLoader = new VezonPluginLoader();

                while (!exit)
                {
                    if (PluginLoader!.LoadedExtensions.Any())
                    {
                        PluginLoader!.LoadedExtensions.ForEach(extension =>
                        {
                            extension.OnThink();
                        });
                    }
                }

                if (PluginLoader!.LoadedExtensions.Any())
                {
                    PluginLoader!.LoadedExtensions.ForEach(extension =>
                    {
                        extension.OnShutdown();
                    });
                }
            }
        }

        public void Kill()
        {
            _G.WriteLine($"Exiting...");
            exit = true;
        }
    }
}
