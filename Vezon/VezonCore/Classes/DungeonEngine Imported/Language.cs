using System.Text.Json;

public class Language
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public JsonElement Values { get; set; }
    public JsonElement Root { get; set; }

    public Language()
    {
        string file = File.ReadAllText(FileManagement.GenerateJSONFilePath(Path.Combine(FileManagement.ResourcePath, $"{FileManagement.LangPrefix}english")));
        JsonDocument doc = JsonDocument.Parse(file);
        Root = doc.RootElement;
        Name = FileManagement.LoadElementProperty(Root, "Name").ToString();
        ShortName = FileManagement.LoadElementProperty(Root, "ShortName").ToString();
        Values = FileManagement.LoadElementProperty(Root, "Values");
    }

    public Language(string fileName)
    {
        string file = File.ReadAllText(FileManagement.GenerateJSONFilePath(Path.Combine(FileManagement.ResourcePath, $"{FileManagement.LangPrefix}{fileName}")));
        JsonDocument doc = JsonDocument.Parse(file);
        Root = doc.RootElement;
        Name = FileManagement.LoadElementProperty(Root, "Name").ToString();
        ShortName = FileManagement.LoadElementProperty(Root, "ShortName").ToString();
        Values = FileManagement.LoadElementProperty(Root, "Values");
    }

    public virtual string LoadTranslatedString(string untranslated)
    {
        JsonElement jsonResult = FileManagement.LoadElementProperty(Values, untranslated);
        string result = jsonResult.ToString();
        if (!string.IsNullOrEmpty(result))
        {
            return result;
        }
        else
        {
            return untranslated;
        }
    }
}


