using Raylib_cs;

namespace Simulation_CSharp.Serialization;

// TODO
public class SaveLoad
{
    private const string Path = "Saves/";
    private const string FilePath = "Saves/Test.json";
        
    public static void Load(string file)
    {
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Loading save file");
    }

    public static void Save()
    {
        Raylib.TraceLog(TraceLogLevel.LOG_INFO, "Saving to save file");
        if (!File.Exists(Path)) Directory.CreateDirectory(Path);
        using var fileStream = new FileStream(FilePath, FileMode.Create);
    }
}