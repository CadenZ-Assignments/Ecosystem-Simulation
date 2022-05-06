using Raylib_cs;

namespace Simulation_CSharp.Core;

public static class ResourceLoader
{
    private static readonly Dictionary<string, Texture2D> Textures = new();

    public static void LoadTextures()
    {
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Loading PNG textures from Resources/Textures/ directory");
        foreach (var fileName in Directory.GetFiles("resources/textures/", "*.png", SearchOption.AllDirectories))
        {
            var fn = fileName[19..];
            // remove directory by string splicing
            Textures.Add(fn, Raylib.LoadTexture(fileName));
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Loaded " + fn);
        }
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Finished loading textures");
    }

    public static Texture2D GetTexture(string path)
    {
        if (!Textures.ContainsKey(path))
        {
            throw new Exception("Tried to access texture " + path + " but it does not exist (or not loaded)");
        }

        return Textures[path];
    }
}