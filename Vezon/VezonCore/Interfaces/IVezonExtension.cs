namespace VezonCore
{
    public class IVezonExtension
    {
        public virtual VezonPluginAddonManager Manager { get; set; } = new VezonPluginAddonManager();

        public virtual string Name() { return "Unnamed Object"; }
        public virtual string ShortName() { return "object"; }
        public virtual string Version() { return "1.0.0"; }
        public virtual string Author() { return "John Doe"; }
        public virtual string FullInfoString() { return (Name() + " v" + Version() + " by " + Author()); }
        public virtual void OnLoad() 
        {
            Manager.LoadExtensions($"{Path.Combine(Global.Locations.ExtensionAddons, ShortName())}");
        }
        public virtual void OnShutdown() 
        {
            Manager.UnloadExtensions();
            Manager.GetExtensionList().Clear();
        }
        public virtual void OnThink() 
        {
            Manager.UpdateExtensions();
        }
    }
}
