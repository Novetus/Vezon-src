using System.Diagnostics;
using VezonCore;

namespace VezonCore
{
    public static class Global
    {
        public class Locations
        {
            public static string Extension = Path.Combine(AppContext.BaseDirectory, "extensions");
            public static string Resource = Path.Combine(AppContext.BaseDirectory, "resource");
            public static string Config = Path.Combine(AppContext.BaseDirectory, "cfg");
        }

        public static VezonJSONLoaderMultiElement Info = new VezonJSONLoaderMultiElement(Path.Combine(Locations.Config, $"info"));
        public static Config Cfg = new Config(Path.Combine(Locations.Config, $"config"));
        public static readonly string ProjectName = Info.LoadValueString("ProjectName");
        public static readonly string ProjectShortName = Info.LoadValueString("ProjectShortName");
        public static Language? CurLanguage = new Language(Cfg.ReadValue("Language"));

        public static Vezon Instance = new Vezon();

        public static void WriteLine(string? message)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}

