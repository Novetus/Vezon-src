using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VezonCore.Vezon;

namespace VezonCore
{
    public class VezonPluginLoader
    {
        public List<PluginLoader> LoadedExtensionLoaders;

        public VezonPluginLoader()
        {
            Global.WriteLine($"Loading extensions...");
            LoadedExtensionLoaders = new List<PluginLoader>();
            LoadPlugins();
            Global.WriteLine($"Extensions loaded.");
        }

        public void LoadPlugins()
        {
            if (!LoadedExtensionLoaders.Any())
            {
                // create plugin loaders
                var pluginsDir = Global.Locations.Extension;

                if (Directory.Exists(pluginsDir))
                {
                    foreach (var dir in Directory.GetFiles(pluginsDir, "*.dll"))
                    {
                        var loader = PluginLoader.CreateFromAssemblyFile(
                                assemblyFile: dir,
                                isUnloadable: true,
                                sharedTypes: new[] { typeof(IVezonExtension) });

                        LoadedExtensionLoaders.Add(loader);
                    }

                    // Create an instance of plugin types
                    foreach (var loader in LoadedExtensionLoaders)
                    {
                        IVezonExtension? plugin = GetPluginForLoader(loader);
                        if (plugin != null)
                        {
                            Global.WriteLine($"Loaded '{plugin.Name()}' by '{plugin.Author()}'.");
                            plugin.OnLoad();
                        }
                    }
                }
                else
                {
                    Global.WriteLine($"Cannot load extensions: The {pluginsDir} folder is missing. The folder will be created for future loads.");
                    Directory.CreateDirectory(pluginsDir);
                }
            }
        }

        public void UnloadPlugins()
        {
            if (LoadedExtensionLoaders.Any())
            {
                LoadedExtensionLoaders.ForEach(loader => {
                    UnloadPlugin(loader);
                });
            }
        }

        public void UnloadPlugin(PluginLoader loader)
        {
            if (loader.IsUnloadable)
            {
                loader.Dispose();
            }
        }

        public IVezonExtension? GetPluginForLoader(PluginLoader loader)
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
                    return plugin;
                }
            }

            return null;
        }
    }
}
