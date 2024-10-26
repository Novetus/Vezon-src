#region Usings
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
#endregion

// based on https://stackoverflow.com/questions/137933/what-is-the-best-scripting-language-to-embed-in-a-c-sharp-desktop-application
namespace VezonCore
{
    public class ExtensionAddonScript
    {
        public static object LoadScriptFromContent(string scriptPath)
        {
            try
            {
                using (var stream = File.OpenRead(scriptPath))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        string script = reader.ReadToEnd();
                        Assembly compiled = CompileScript(script, scriptPath);
                        object code = ExecuteScript(compiled, scriptPath);
                        return code;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(scriptPath + ": " + ex.ToString());
            }

            return null;
        }

        private static object ExecuteScript(Assembly assemblyScript, string filePath)
        {
            if (assemblyScript == null)
            {
                goto error;
            }

            foreach (Type type in assemblyScript.GetExportedTypes())
            {
                if (type.IsInterface || type.IsAbstract)
                    continue;

                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);

                if (constructor != null && constructor.IsPublic)
                {
                    return constructor.Invoke(null);
                }
                else
                {
                    ErrorHandler(filePath + ": Constructor does not exist or it is not public.");
                    return null;
                }
            }

        error:
            return null;
        }

        private static Assembly CompileScript(string code, string filePath)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerParameters perams = new CompilerParameters();
            perams.GenerateExecutable = false;
            perams.GenerateInMemory = true;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).Select(a => a.Location);
            perams.ReferencedAssemblies.AddRange(assemblies.ToArray());

            CompilerResults result = provider.CompileAssemblyFromSource(perams, code);

            foreach (CompilerError error in result.Errors)
            {
                if (error.IsWarning)
                    continue;

                ErrorHandler(error, filePath);
            }

            if (result.Errors.HasErrors)
            {
                return null;
            }

            return result.CompiledAssembly;
        }

        private static void ErrorHandler(string error)
        {
            Global.WriteLine($"[SCRIPT ERROR] - {error}");
        }

        private static void ErrorHandler(CompilerError error, string fileName)
        {
            Global.WriteLine($"[SCRIPT ERROR] - {fileName} ({error.Line},{error.Column}): {error.ErrorText}");
        }
    }

    public class VezonPluginAddonManager
    {
        private List<IVezonExtensionAddon> ExtensionList = new List<IVezonExtensionAddon>();
        private string directory = "";

        public VezonPluginAddonManager()
        {
        }

        public virtual List<IVezonExtensionAddon> GetExtensionList()
        {
            return ExtensionList;
        }

        public virtual void LoadExtensions(string dirPath)
        {
            string nothingFoundError = "No Extension Addons found.";

            if (!Directory.Exists(dirPath))
            {
                Global.WriteLine(nothingFoundError);
                return;
            }
            else
            {
                directory = dirPath;
            }

            // load up all .cs files.
            string[] filePaths = Directory.GetFiles(dirPath, "*.cs", SearchOption.TopDirectoryOnly);

            if (filePaths.Count() == 0)
            {
                Global.WriteLine(nothingFoundError);
                return;
            }

            foreach (string file in filePaths)
            {
                int index = 0;

                try
                {
                    IVezonExtensionAddon newExt = (IVezonExtensionAddon)ExtensionAddonScript.LoadScriptFromContent(file);
                    ExtensionList.Add(newExt);
                    index = ExtensionList.IndexOf(newExt);
                    Global.WriteLine("Loaded Extension Addons " + newExt.FullInfoString() + " from " + Path.GetFileName(file));
                    newExt.OnAddonLoad();
                }
                catch (Exception)
                {
                    Global.WriteLine("Failed to load script " + Path.GetFileName(file));
                    ExtensionList.RemoveAt(index);
                    continue;
                }
            }
        }

        public virtual void ReloadExtensions()
        {
            string nothingFoundError = "No Extension Addons found. There is nothing to reload.";

            if (!ExtensionList.Any())
            {
                Global.WriteLine(nothingFoundError);
                return;
            }

            Global.WriteLine("Reloading Extension Addons...");

            UnloadExtensions();
            LoadExtensions(directory);
        }

        public virtual void UnloadExtensions()
        {
            string nothingFoundError = "No Extension Addons found. There is nothing to unload.";

            if (!ExtensionList.Any())
            {
                Global.WriteLine(nothingFoundError);
                return;
            }

            Global.WriteLine("Unloading all Extension Addons...");

            foreach (IVezonExtensionAddon extension in ExtensionList.ToArray())
            {
                try
                {
                    extension.OnAddonUnload();
                }
                catch (Exception)
                {
                }
            }

            ExtensionList.Clear();
        }

        public virtual void UpdateExtensions()
        {
            foreach (IVezonExtensionAddon extension in ExtensionList.ToArray())
            {
                try
                {
                    extension.OnAddonUpdate();
                }
                catch (Exception)
                {
                }
            }
        }

        public virtual string GenerateExtensionList()
        {
            string nothingFoundError = "No Extension Addons found.";

            if (!ExtensionList.Any())
            {
                return nothingFoundError;
            }

            string result = "";

            foreach (IVezonExtensionAddon extension in ExtensionList.ToArray())
            {
                try
                {
                    result += $"- {extension.FullInfoString()}\n";
                }
                catch (Exception)
                {
                }
            }

            result.Trim();
            return result;
        }
    }
}
