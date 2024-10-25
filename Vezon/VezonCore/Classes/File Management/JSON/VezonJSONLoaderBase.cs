using System;
using System.Text.Json;

namespace VezonCore
{
    public class VezonJSONLoaderBase
    {
        public JsonElement Root { get; set; }

        public VezonJSONLoaderBase(string FilePath)
        {
            string file = File.ReadAllText($"{FilePath}.json");
            JsonDocument doc = JsonDocument.Parse(file);
            Root = doc.RootElement;
        }

        public static JsonElement LoadElementProperty(JsonElement element, string name)
        {
            JsonElement result;
            element.TryGetProperty(name, out result);
            return result;
        }
    }
}