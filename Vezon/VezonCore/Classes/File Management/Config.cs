using System.Text.Json;
using System.Text.Json.Nodes;

namespace VezonCore
{
    public class Config
    {
        public static readonly string ConfigPath = Path.Combine(Global.Locations.Config, $"config.json");

        public JsonObject? Values { get; set; }

        public Config(string languageName)
        {
            if (!File.Exists(ConfigPath)) 
            {
                LoadDefaults();
                Save();
            }
            else
            {
                Load();
            }
        }

        public void LoadDefaults()
        {
            Values = new JsonObject
            {
                ["Values"] = new JsonObject
                {
                    ["Language"] = "English",
                }
            };
        }

        public virtual void WriteValue(string key, string value)
        {
            if (Values != null)
            {
                Values["Values"][key] = value;
                Save();
            }
        }

        public virtual string ReadValue(string key)
        {
            Load();

            if (Values != null)
            {
                return Values["Values"][key].ToString();
            }

            return "";
        }

        public void Load()
        {
            VezonJSONLoaderMultiElement ConfigLoader = new VezonJSONLoaderMultiElement(ConfigPath.Replace(".json", ""));
            JsonNodeOptions options = new JsonNodeOptions();
            options.PropertyNameCaseInsensitive = true;
            Values = JsonObject.Create(ConfigLoader.Root, options);
        }

        public void Save() 
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            File.WriteAllText(ConfigPath, Values?.ToJsonString(options));
        }
    }
}


