using System.Text.Json;

namespace VezonCore
{
    public class VezonJSONLoaderMultiElement : VezonJSONLoaderBase
    {
        public JsonElement Values { get; set; }

        public VezonJSONLoaderMultiElement(string fileName) : base(fileName)
        {
            Values = LoadElementProperty(Root, "Values");
        }

        public virtual string LoadValueString(string valueToSearch)
        {
            JsonElement jsonResult = LoadElementProperty(Values, valueToSearch);
            string result = jsonResult.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            else
            {
                return valueToSearch;
            }
        }
    }
}


