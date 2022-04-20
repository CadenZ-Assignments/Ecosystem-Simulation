using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Simulation_CSharp.World.Tiles.PathFinding;

public class PathFinding<T> where T : Node
{
    private Dictionary<TileCell, T>? _map;
    private List<T>? _openSet;
    private T? _start;
    private T? _end;

    public void Init([DisallowNull] T start, [DisallowNull] T end, Dictionary<TileCell, T> map)
    {
        _map = map;
        _start = start;
        _end = end;
        _openSet = new List<T> {_start};

        // no need to evaluate the entire map. only evaluate nodes within a box that will be used by path finding
        var startPos = _start.Position;
        var endPos = _end.Position;

        // the 2 corners
        var x1 = Math.Min(startPos.X, endPos.X);
        var y1 = Math.Min(startPos.Y, endPos.Y);
        var x2 = Math.Max(startPos.X, endPos.X);
        var y2 = Math.Max(startPos.Y, endPos.Y);

        for (var i = x1; i < x2; i++)
        {
            for (var j = y1; j < y2; j++)
            {
                var pos = new TileCell(i, j);
                if (_map.ContainsKey(pos))
                {
                    // initialize costs for all required nodes.
                    _map[pos].InitializeCosts(startPos.Distance(pos), endPos.Distance(pos));
                }
            }
        }
    }

    public async Task<List<Vector2>> FindPath()
    {
        var output = new List<Vector2>();

        await new Task(() =>
        {
            if (_openSet is null || _start is null || _end is null)
            {
                throw new Exception("Init must be called before Eval is called");
            }

            var current = _start;

            // keeps going until it runs out of nodes to evaluate
            while (_openSet.Any())
            {
                foreach (var node in _openSet.Where(node => node.Initialized && node.FCost < current.FCost))
                {
                    current = node;
                    node.UnInitializeCosts();
                    // TODO: is this correct?
                }

                if (current == _end)
                {
                    // output = ReconstructPath();
                    break;
                }

                _openSet.Remove(current);


            }
        });

        return output;
    }

    private List<T> GetNeighbors(TileCell tileCell)
    {
        if ()
    }

    private List<Vector2> ReconstructPath(List<TileCell> cameFrom, Node current)
    {
        var totalPath = new List<Node>{current};
        return new List<Vector2>();
    }
}