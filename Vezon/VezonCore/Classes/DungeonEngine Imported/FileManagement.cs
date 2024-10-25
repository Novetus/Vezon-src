using System.Reflection;
using System.Text.Json;

public class FileManagement
{
    public class JsonFileList
    {
        public List<string> FileNames = new List<string>();
        public List<string> CleanedFileNames = new List<string>();

        public JsonFileList(string folderPath)
        {
            foreach (string fileName in Directory.GetFiles(folderPath, "*.json"))
            {
                FileNames.Add(fileName);
            }

            foreach (string fileName in FileNames)
            {
                CleanedFileNames.Add(Path.GetFileNameWithoutExtension(fileName));
            }
        }
    }

    public static string ExtensionPath = Path.Combine(AppContext.BaseDirectory, "extensions");
    public static string ResourcePath = Path.Combine(AppContext.BaseDirectory, "resource");

    public static readonly string LangPrefix = $"{_G.ProjectShortName}_"; 

    public static string GenerateJSONFilePath(string fileName)
    {
        return fileName + ".json";
    }

    public static JsonElement LoadElementProperty(JsonElement element, string name)
    {
        JsonElement result;
        element.TryGetProperty(name, out result);
        return result;
    }

    public static string? LoadTranslatedElementString(JsonElement element, string name)
    {
        JsonElement untranslated = LoadElementProperty(element, name);
        return _G.CurLanguage?.LoadTranslatedString(untranslated.ToString());
    }
}
