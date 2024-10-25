using System.Diagnostics;
using VezonCore;

public static class _G
{
    public static readonly string ProjectName = "Inertia Engine";
    public static readonly string ProjectShortName = "Inertia";
    public static readonly Vezon VezonInstance = new Vezon();
    public static Language? CurLanguage = new Language();

    public static void WriteLine(string? message)
    { 
        Console.WriteLine(message);
        Debug.WriteLine(message);
    }
}

