using VezonCore;

namespace VezonLoader
{
    internal class Program : IVezonLoader
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.OnBoot(args);
        }

        void OnBoot(string[] args)
        {
            _G.VezonInstance.VezonLoader = this;
            _G.VezonInstance.Main();
        }
    }
}