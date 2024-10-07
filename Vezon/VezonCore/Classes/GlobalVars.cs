using System.Diagnostics;
using VezonCore;

public static class _G
{
    public static readonly string ProjectName = "Inertia Engine";
    public static readonly Vezon VezonInstance = new Vezon();

    public static void WriteLine(string? message)
    { 
        Console.WriteLine(message);
        Debug.WriteLine(message);
    }
}

