using System.Diagnostics;
using System.Net.Http.Headers;

namespace VezonCore
{
    public static class Global
    {
        public class Locations
        {
            public static string DataDir = Path.Combine(AppContext.BaseDirectory, "data");
            public static string Extension = Path.Combine(DataDir, "extensions");
            public static string ExtensionAddons = Path.Combine(Extension, "extension_addons");
            public static string Resource = Path.Combine(DataDir, "resource");
            public static string Config = Path.Combine(DataDir, "cfg");
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

