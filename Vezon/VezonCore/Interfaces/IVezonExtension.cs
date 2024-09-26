namespace VezonCore
{
    public interface IVezonExtension
    {
        public virtual string Name() { return "Unnamed Object"; }
        public virtual string Version() { return "1.0.0"; }
        public virtual string Author() { return "John Doe"; }
        public virtual string FullInfoString() { return (Name() + " v" + Version() + " by " + Author()); }
        public virtual void OnExtensionLoad() { }
        public virtual void OnExtensionUnload() { }
    }
}
