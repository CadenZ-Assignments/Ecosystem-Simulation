using Simulation_CSharp.World.Tiles;

namespace Simulation_CSharp.PathFinding;

public class AStarPathFinder<T> where T : Node
{
    private Dictionary<TileCell, T>? _map;
    private List<Node>? _openSet;
    private List<Node>? _closedSet;
    private Node? _start;
    private Node? _end;
    private int _boundX1;
    private int _boundY1;
    private int _boundX2;
    private int _boundY2;

    public void Init(Node start, Node end, Dictionary<TileCell, T> map)
    {
        _map = map;
        _start = start;
        _end = end;
        _openSet = new List<Node>();
        _closedSet = new List<Node>();

        // no need to evaluate the entire map. only evaluate nodes within a box that will be used by path finding but if no valid path is found we search in a bigger grid.
        var startPos = _start.Position;
        var endPos = _end.Position;

        // the 2 corners
        var x1 = Math.Min(startPos.X, endPos.X);
        var y1 = Math.Min(startPos.Y, endPos.Y);
        var x2 = Math.Max(startPos.X, endPos.X);
        var y2 = Math.Max(startPos.Y, endPos.Y);

        InitializeGrid(x1, y1, x2, y2);
    }

    public List<TileCell> FindPath()
    {
        if (_openSet is null || _closedSet is null || _start is null || _end is null)
        {
            throw new Exception("Init must be called before Find Path is called");
        }
        
        var output = new List<TileCell>();

        var current = _start;
        _openSet.Add(current);

        while (_openSet.Any() && current != _end)
        {
            foreach (var node in GetNeighbors(current).Where(node => !_openSet.Contains(node)))
            {
                _openSet.Add(node);
            }

            _openSet.Remove(current);
            _closedSet.Add(current);
                
            foreach (var openNode in _openSet.Where(openNodes => openNodes.Initialized && !_closedSet.Contains(openNodes)))
            {
                if (openNode.FCost < current.FCost)
                {
                    current = openNode;
                }
            }
        }

        AddParent(current, output);
        
        foreach (var closedNode in _closedSet)
        {
            closedNode.UnInitializeCosts();
        }
        
        output.Reverse();
        return output;
    }

    private static void AddParent(Node baseNode, ICollection<TileCell> parents)
    {
        if (baseNode.Parent is null)
        {
            parents.Add(baseNode.Position);
            return;
        }
        
        parents.Add(baseNode.Parent.Position);
        AddParent(baseNode.Parent, parents);
    }

    private List<Node> GetNeighbors(Node node)
    {
        if (_map is null)
        {
            throw new Exception("Tried to initialize grid without initializing path finder");
        }
        
        var neighbors = new List<Node>();

        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                // the base node itself
                if (i == 0 && j == 0)
                {
                    continue;
                }
                
                var x = node.Position.X + i;
                var y = node.Position.Y + j;
                var pos = new TileCell(x, y);

                if (!ExistInRange(x, y)) continue;

                _map[pos].Parent = node;
                neighbors.Add(_map[pos]);
            }
        }

        return neighbors;
    }

    private bool ExistInRange(int x, int y)
    {
        return x > 0 && y > 0 && x >= _boundX1 && y >= _boundY1 && x <= _boundX2 && y <= _boundY2;
    }

    private void InitializeGrid(int x1, int y1, int x2, int y2)
    {
        if (_map is null || _start is null || _end is null)
        {
            throw new Exception("Tried to initialize grid without initializing map");
        }

        _boundX1 = x1;
        _boundY1 = y1;
        _boundX2 = x2;
        _boundY2 = y2;

        for (var i = x1; i <= x2; i++)
        {
            for (var j = y1; j <= y2; j++)
            {
                var pos = new TileCell(i, j);
                if (_map.ContainsKey(pos))
                {
                    // initialize costs for all required nodes.
                    _map[pos].InitializeCosts(_start.Position.Distance(pos), _end.Position.Distance(pos));
                }
            }
        }
    }
}