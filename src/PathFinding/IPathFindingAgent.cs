using System.Numerics;
using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.PathFinding;

public interface IPathFindingAgent<T> where T : Node
{ 
    void Init(Node start, Node end, Dictionary<TileCell, T> map);

    Task<List<TileCell>> FindPath();
}