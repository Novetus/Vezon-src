using System.Text.Json;

public class Language
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public JsonElement Values { get; set; }
    public JsonElement Root { get; set; }

    public Language()
    {
        string file = File.ReadAllText(FileManagement.GenerateJSONFilePath(Path.Combine(FileManagement.LanguagePath, $"{FileManagement.LangPrefix}english.json")));
        JsonDocument doc = JsonDocument.Parse(file);
        Root = doc.RootElement;
        Name = FileManagement.LoadElementProperty(Root, "Name").ToString();
        ShortName = FileManagement.LoadElementProperty(Root, "ShortName").ToString();
        Values = FileManagement.LoadElementProperty(Root, "Values");
    }

    public Language(string fileName)
    {
        string file = File.ReadAllText(FileManagement.GenerateJSONFilePath(Path.Combine(FileManagement.LanguagePath, $"{FileManagement.LangPrefix}{fileName}.json")));
        JsonDocument doc = JsonDocument.Parse(file);
        Root = doc.RootElement;
        Name = FileManagement.LoadElementProperty(Root, "Name").ToString();
        ShortName = FileManagement.LoadElementProperty(Root, "ShortName").ToString();
        Values = FileManagement.LoadElementProperty(Root, "Values");
    }

    public virtual string TranslateString(string propertyName)
    {
        JsonElement jsonResult = FileManagement.LoadElementProperty(Values, propertyName);
        string result = jsonResult.ToString();
        return result;
    }
}


