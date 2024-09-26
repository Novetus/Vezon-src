using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezonCore
{
    public class VezonPluginLoader
    {
        public List<IVezonExtension> LoadedExtensions = new List<IVezonExtension>();

        public VezonPluginLoader()
        {
            Console.WriteLine($"Loading plugins...");
            LoadedExtensions = new List<IVezonExtension>();
            LoadPlugins();
            Console.WriteLine($"Plugins loaded.");
        }

        public void LoadPlugins()
        {
            var loaders = new List<PluginLoader>();

            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new[] { typeof(IVezonExtension) });
                    loaders.Add(loader);
                }
            }

            // Create an instance of plugin types
            foreach (var loader in loaders)
            {
                foreach (var pluginType in loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IVezonExtension).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    IVezonExtension plugin = (IVezonExtension)Activator.CreateInstance(pluginType);
                    LoadedExtensions.Add(plugin);
                    Console.WriteLine($"Loaded '{LoadedExtensions.IndexOf(plugin)}'.");
                }
            }
        }
    }
}
