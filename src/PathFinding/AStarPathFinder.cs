using Simulation_CSharp.Entities;
using Simulation_CSharp.Tiles;

namespace Simulation_CSharp.PathFinding;

public class AStarPathFinder : IPathFindingAgent<Tile>
{
    private const bool IgnoreGCost = false;
    
    private Dictionary<TileCell, Tile>? _grid;
    private Dictionary<Tile, Tile?>? _parents;
    private Dictionary<Tile, float>? _gCosts;
    private Dictionary<Tile, float>? _hCosts;
    private Dictionary<Tile, float>? _fCosts;
    private List<Tile>? _openSet;
    private List<Tile>? _closedSet;
    private Tile? _start;
    private Tile? _end;
    private int _boundX1;
    private int _boundY1;
    private int _boundX2;
    private int _boundY2;

    private readonly Entity _entity;
    
    public AStarPathFinder(Entity entity)
    {
        _entity = entity;
    }

    public void Init(Tile start, Tile end, Dictionary<TileCell, Tile> grid)
    {
        _grid = grid;
        _parents = new Dictionary<Tile, Tile?>();
        _gCosts = new Dictionary<Tile, float>();
        _hCosts = new Dictionary<Tile, float>();
        _fCosts = new Dictionary<Tile, float>();
        _start = start;
        _end = end;
        _openSet = new List<Tile>();
        _closedSet = new List<Tile>();
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
            
            foreach (var neighbor in neighbors.Where(neighbor => neighbor.WalkableForEntity(_entity) || neighbor == _end).Where(neighbor => !_closedSet.Contains(neighbor)))
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

    private void RetracePath(Tile baseTile, ICollection<TileCell> parents)
    {
        if (_parents is null)
        {
            throw new Exception("Tried to path find without initializing required variables");
        }

        var parent = _parents[baseTile];
        
        if (parent is null)
        {
            parents.Add(baseTile.Position);
            return;
        }
        
        parents.Add(parent.Position);
        RetracePath(parent, parents);
    }

    private IEnumerable<Tile> GetNeighbors(Tile node)
    {
        if (_grid is null)
        {
            throw new Exception("Tried to initialize grid without initializing path finder");
        }

        var neighbors = new List<Tile>();

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