namespace Simulation_CSharp.Registry;

public struct RegistryName
{
    public readonly string Namespace;
    public readonly string Path;
    public readonly string Full;

    public RegistryName(string ns, string path)
    {
        Namespace = ns;
        Path = path;
        Full = Namespace + ":" + Path;
    }
}