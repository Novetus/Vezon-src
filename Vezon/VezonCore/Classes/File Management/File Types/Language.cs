using System.Text.Json;

namespace VezonCore
{
    public class Language : VezonJSONLoaderMultiElement
    {
        public string Name { get; set; }
        public string ShortName { get; set; }

        public static readonly string LangPrefix = $"{Global.ProjectShortName}_";

        public Language(string languageName) : base(Path.Combine(Global.Locations.Resource, $"{LangPrefix}{languageName}"))
        {
            Name = LoadElementProperty(Root, "Name").ToString();
            ShortName = LoadElementProperty(Root, "ShortName").ToString();
        }
        
        //outputs the classic source empty language value.
        public override string LoadValueString(string valueToSearch)
        {
            JsonElement jsonResult = LoadElementProperty(Values, valueToSearch);
            string result = jsonResult.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            else
            {
                return $"#{valueToSearch}";
            }
        }
    }
}


