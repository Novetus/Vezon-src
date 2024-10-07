using McMaster.NETCore.Plugins;
using VezonCore;

namespace VezonTestPlugin
{
    public class VezonTestPlugin : IVezonExtension
    {
        public virtual string Name() 
        { 
            return "Test"; 
        }
        public virtual string Version() 
        { 
            return "1.0.0"; 
        }
        public virtual string Author() 
        { 
            return "Bitl"; 
        }
        public virtual void OnLoad() 
        {
            _G.WriteLine("test");
        }
        public virtual void OnShutdown() 
        {
            _G.WriteLine("shutdown");
        }
        public virtual void OnThink() 
        {
            _G.WriteLine("gay");

            foreach (var i in _G.VezonInstance.PluginLoader!.LoadedExtensionLoaders)
            {
                IVezonExtension? plugin = _G.VezonInstance.PluginLoader!.GetPluginForLoader(i);
                if (plugin != null)
                {
                    _G.WriteLine($"List has {plugin.Name()}");
                }
            }
        }
    }
}