using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VezonCore
{
    public class IVezonExtensionAddon
    {
        public virtual string Name() { return "Unnamed Object"; }
        public virtual string Version() { return "1.0.0"; }
        public virtual string Author() { return "John Doe"; }
        public virtual string FullInfoString() { return (Name() + " v" + Version() + " by " + Author()); }
        public virtual void OnAddonLoad() { }
        public virtual void OnAddonUnload() { }
        public virtual void OnAddonUpdate() { }
    }
}
