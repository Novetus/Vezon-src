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
            _G.WriteLine($"Loading extensions...");
            LoadedExtensions = new List<IVezonExtension>();
            LoadPlugins();
            _G.WriteLine($"Extensions loaded.");
        }

        public void LoadPlugins()
        {
            var loaders = new List<PluginLoader>();

            // create plugin loaders
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "extensions");

            if (Directory.Exists(pluginsDir))
            {
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
                        IVezonExtension? plugin = (IVezonExtension?)Activator.CreateInstance(pluginType) ?? null;
                        if (plugin != null)
                        {
                            LoadedExtensions.Add(plugin);
                            _G.WriteLine($"Loaded '{LoadedExtensions.IndexOf(plugin)}'.");
                        }
                    }
                }
            }
            else
            {
                _G.WriteLine($"Cannot load extensions: The {pluginsDir} folder is missing. The folder will be created for future loads.");
                Directory.CreateDirectory(pluginsDir);
            }
        }
    }
}
