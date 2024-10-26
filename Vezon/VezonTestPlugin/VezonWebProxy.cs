using McMaster.NETCore.Plugins;
using System.Net;
using System.Reflection.Metadata;
using VezonCore;

namespace VezonWebProxy
{
    public class VezonWebProxy : IVezonExtension
    {
        WebProxy proxy { get; } = new WebProxy();

        public override string Name() 
        { 
            return "Web Proxy"; 
        }

        public override string ShortName() 
        { 
            return "proxy"; 
        }

        public override string Version() 
        { 
            return "1.0.0"; 
        }

        public override string Author() 
        { 
            return "Bitl"; 
        }

        public override void OnLoad() 
        {
            base.OnLoad();

            bool result = ConvertSafe.ToBooleanSafe(Global.Cfg.ReadValue("WebProxyEnabled"));
            string Testresult = Global.Cfg.ReadValue("WebProxyEnabled");

            if (result)
            {
                proxy.Parent = this;
                proxy.Start();
            }
        }

        public override void OnShutdown() 
        {
            bool result = ConvertSafe.ToBooleanSafe(Global.Cfg.ReadValue("WebProxyEnabled"));

            if (result)
            {
                proxy.Stop();
            }

            base.OnShutdown();
        }
    }
}