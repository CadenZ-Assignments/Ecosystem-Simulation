using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.PathFinding;

public class AStarPathFinder<T> : IPathFindingAgent<T> where T : Node
{
    private const bool IgnoreGCost = false;
    
    private Dictionary<TileCell, T>? _grid;
    private Dictionary<Node, Node?>? _parents;
    private Dictionary<Node, float>? _gCosts;
    private Dictionary<Node, float>? _hCosts;
    private Dictionary<Node, float>? _fCosts;
    private List<Node>? _openSet;
    private List<Node>? _closedSet;
    private Node? _start;
    private Node? _end;
    private int _boundX1;
    private int _boundY1;
    private int _boundX2;
    private int _boundY2;

    public void Init(Node start, Node end, Dictionary<TileCell, T> grid)
    {
        _grid = grid;
        _parents = new Dictionary<Node, Node?>();
        _gCosts = new Dictionary<Node, float>();
        _hCosts = new Dictionary<Node, float>();
        _fCosts = new Dictionary<Node, float>();
        _start = start;
        _end = end;
        _openSet = new List<Node>();
        _closedSet = new List<Node>();
        _openSet.Add(_start);

        var startPos = _start.Position;
        var endPos = _end.Position;

        // no need to evaluate the entire map. only evaluate nodes within a box that will be used by path finding but if no valid path is found we search in a bigger grid.
        // the 2 corners
        _boundX1 = Math.Min(startPos.X, endPos.X);
        _boundY1 = Math.Min(startPos.Y, endPos.Y);
        _boundX2 = Math.Max(startPos.X, endPos.X);
        _boundY2 = Math.Max(startPos.Y, endPos.Y);

        for (var i = _boundX1 - 1; i <= _boundX2 + 1; i++)
        {
            for (var j = _boundY1 - 1; j < _boundY2 + 1; j++)
            {
                _parents.Add(grid[new TileCell(i, j)], null);
                _gCosts.Add(grid[new TileCell(i, j)], 0);
                _hCosts.Add(grid[new TileCell(i, j)], 0);
                _fCosts.Add(grid[new TileCell(i, j)], 0);
            }
        }
    }

    public List<TileCell> FindPath()
    {
        if (_grid is null || _fCosts is null || _gCosts is null || _hCosts is null || _parents is null || _openSet is null || _closedSet is null || _start is null || _end is null)
        {
            throw new Exception("Tried to path find without initializing required variables");
        }

        var path = new List<TileCell>();

        // still have cells left to evaluate
        while (_openSet.Count > 0)
        {
            // find the cell with the lowest F value
            var winningIndex = 0;

            for (var i = 0; i < _openSet.Count; i++)
            {
                if (_fCosts[_openSet[i]] < _fCosts[_openSet[winningIndex]])
                {
                    winningIndex = i;
                }
            }

            var winningCell = _openSet[winningIndex];

            // needs to reset path before calling retrace path because retrace path does not clear the path array. It only adds path to it.
            path.Clear();
            RetracePath(winningCell, path);

            if (winningCell == _end)
            {
                // clear out the open set as we are done.
                _openSet = null;
                _closedSet = null;
                
                path.Clear();
                RetracePath(winningCell, path);
                path.Reverse();
                
                // for whatever reason the last element gets added twice so we remove it
                path.RemoveAt(0);
                
                // add the end cell to path as well because retracePath does not add the base position to the array
                path.Add(winningCell.Position);

                return path;
            }

            // basically removes the winning cell from the open set
            _openSet.Remove(winningCell);
            _closedSet.Add(winningCell);

            var neighbors = GetNeighbors(winningCell);
            
            foreach (var neighbor in neighbors.Where(neighbor => !neighbor.IsObstructed).Where(neighbor => !_closedSet.Contains(neighbor)))
            {
                var tempG = _gCosts[winningCell] + neighbor.Position.Distance(winningCell.Position);
                var newG = false;
                
                // if _openSet contains
                if (_openSet.Contains(neighbor))
                {
                    if (tempG < _gCosts[neighbor])
                    {
                        _gCosts[neighbor] = tempG;
                        newG = true;
                    }
                }
                else
                {
                    _gCosts[neighbor] = tempG;
                    newG = true;
                    _openSet.Add(neighbor);
                }

                if (!newG) continue;
                _hCosts[neighbor] = neighbor.Position.Distance(_end.Position);
                _fCosts[neighbor] = IgnoreGCost ? _hCosts[neighbor] : _gCosts[neighbor] + _hCosts[neighbor];
                _parents[neighbor] = winningCell;
            }
        }

        return new List<TileCell>();
    }

    private void RetracePath(Node baseNode, ICollection<TileCell> parents)
    {
        if (_parents is null)
        {
            throw new Exception("Tried to path find without initializing required variables");
        }

        var parent = _parents[baseNode];
        
        if (parent is null)
        {
            parents.Add(baseNode.Position);
            return;
        }
        
        parents.Add(parent.Position);
        RetracePath(parent, parents);
    }

    private IEnumerable<Node> GetNeighbors(Node node)
    {
        if (_grid is null)
        {
            throw new Exception("Tried to initialize grid without initializing path finder");
        }

        var neighbors = new List<Node>();

        for (var i = -1; i <= 1; i++) {
            for (var j = -1; j <= 1; j++) {
                // the base node itself
                if (i == 0 && j == 0) {
                    continue;
                }

                // if (!corners && Math.abs(i) + Math.abs(j) > 1) {
                    // continue;
                // }

                var x = node.Position.X + i;
                var y = node.Position.Y + j;
                var pos = new TileCell(x, y);

                if (!ExistInRange(x, y)) continue;

                neighbors.Add(_grid[pos]);
            }
        }

        return neighbors;
    }

    private bool ExistInRange(int x, int y)
    {
        return x > 0 && y > 0 && x >= _boundX1 && y >= _boundY1 && x <= _boundX2 && y <= _boundY2;
    }
}