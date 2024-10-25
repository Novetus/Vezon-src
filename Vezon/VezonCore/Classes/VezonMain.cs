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
            if (VezonLoader != null)
            {
                PluginLoader = new VezonPluginLoader();

                while (!exit)
                {
                    foreach (var loader in PluginLoader.LoadedExtensionLoaders)
                    {
                        // This assumes the implementation of IPlugin has a parameterless constructor
                        IVezonExtension? plugin = PluginLoader.GetPluginForLoader(loader);
                        if (plugin != null)
                        {
                            plugin.OnThink();
                        }
                    }
                }

                foreach (var loader in PluginLoader.LoadedExtensionLoaders)
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    IVezonExtension? plugin = PluginLoader.GetPluginForLoader(loader);
                    if (plugin != null)
                    {
                        plugin.OnShutdown();
                    }
                }

                PluginLoader.UnloadPlugins();
            }
        }

        public void Kill()
        {
            Global.WriteLine($"Exiting...");
            exit = true;
        }
    }
}
