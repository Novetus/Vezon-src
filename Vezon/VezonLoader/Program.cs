using System.Reflection;
using VezonCore;

namespace VezonLoader
{
    internal class Program : VezonCore.VezonLoader
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            program.OnBoot(args);
        }
    }
}