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
            Global.Cfg.WriteValue("Test", "Test Works");
            Global.WriteLine(Global.Cfg.ReadValue("Test"));
        }
        public virtual void OnShutdown() 
        {
            Global.WriteLine("shutdown");
        }
        public virtual void OnThink() 
        {
            
        }
    }
}