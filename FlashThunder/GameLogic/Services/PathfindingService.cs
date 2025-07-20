using FlashThunder.GameLogic.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Services;
internal interface IPathNode
{
    public IPathNode Parent { get; set; }
    public Point Position { get; set; }
}
file sealed class PathNodeDijkstras : IPathNode
{
    public IPathNode Parent { get; set; }
    public Point Position { get; set; }
    public float Weight { get; set; }

    public PathNodeDijkstras(IPathNode parent, Point position)
    {
        Parent = parent;
        Position = position;
    }
}
file sealed class PathNodeAStar : IPathNode
{
    public float G { get; set; }
    public float H { get; set; }
    public IPathNode Parent { get; set; }
    public Point Position { get; set; }
    public float F
        => G + H;

    public PathNodeAStar(PathNodeAStar parent, Point position, float g, float h)
    {
        Parent = parent;
        Position = position;
        G = g;
        H = h;
    }
}

/// <summary>
/// A WIP pathfinding service that I ripped from an old project. Some modifications still need to
/// be made, so this is unorganized right now.
/// </summary>
internal class PathfindingService
{
    private MapResource _map;
    private int _height;
    private int _width;
    public MapResource Map { get
        {
            return _map;
        }
        set
        {
            _map = value;
            _height = value.Tiles.Length;
            _width = value.Tiles[0].Length;
        }
    }
    // this function is used to determine whether a tile is passable or not (and thus whether it's
    // a valid neighbor)
    public Func<Point, string[], int, bool> IsPassable { get; set; }
    // this heuristic is for A*, which needs the distance from the current node to the goal node
    public Func<Point, Point, float> PathfindingHeuristic { get; set; }
    // this heuristic is for Dijkstras, which just needs the weight of the tile
    public Func<Point, float> GetNodeWeight { get; set; }
    public PathfindingService(
        MapResource map,
        Func<Point, string[], int, bool> isPassable,
        Func<Point, Point, float> pathfindingHeuristic,
        Func<Point, float> getNodeWeight
        )
    {
        Map = map;
        IsPassable = isPassable;
        PathfindingHeuristic = pathfindingHeuristic;
        GetNodeWeight = getNodeWeight;
    }

    private bool IsOutOfBounds(Point p)
        => p.X < 0 || p.Y < 0 || p.Y >= _height || p.X >= _width;

    /// <summary>
    /// from a completed pathfind, convert to a more usable normal list
    /// </summary>
    /// <param name="waypoint"></param>
    /// <returns></returns>
    private static List<Point> ReconstructPath(IPathNode waypoint)
    {
        List<Point> path = [];
        while (waypoint != null)
        {
            path.Add(waypoint.Position);
            waypoint = waypoint.Parent;
        }

        //the path is currently leading with the goal in front, so flip it
        path.Reverse();
        return path;
    }
    #region - - - [ clearance-based A* pathfinding ] - - -

    public List<Point> Pathfind(
        string[] canTraverse,
        Point from,
        Point to,
        int size = 1
        )
    {
        //init. data structures
        PriorityQueue<PathNodeAStar, float> open = new();
        HashSet<Point> closed = [];
        Dictionary<Point, float> gValues = [];

        //init. and enqueue the first node
        var start = new PathNodeAStar(null, from, 0, PathfindingHeuristic(from, to));
        open.Enqueue(start, start.F);

        while (open.Count > 0)
        {
            var node = open.Dequeue();
            int xDiff = to.X - node.Position.X;
            int yDiff = to.Y - node.Position.Y;
            // EXIT POINT : if we reached the goal, return the (reconstructed) path
            if (node.Position == to ||
                // This should be converted to AABB for proper accuracy. I am just lazy right now.
                (size > 1 && xDiff < size && xDiff >= 0 && yDiff < size && yDiff >= 0))
            {
                return ReconstructPath(node);
            }

            closed.Add(node.Position);

            //look thru all neighbors within 1 block distance
            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    var newPos = new Point(node.Position.X + col, node.Position.Y + row);

                    float distSq = row != 0 && col != 0
                        ? 1.4f //approximately sqrt(2) for a diagonal movement
                        : 1; //the move is either horizontal or vertical
                    float gVal = node.G + distSq;

                    //if there's already a cheaper path that reaches that node, this search is redundant
                    //so terminate here
                    if (gValues.ContainsKey(newPos))
                    {
                        if (gValues[newPos] <= gVal)
                            continue;
                    }
                    else
                    {
                        gValues[newPos] = gVal;
                    }

                    //if it's:
                    // out of bounds
                    // already checked (in closed list)
                    // the same node as the one being checked
                    // continue the loop, no action needed
                    if (
                        IsOutOfBounds(newPos)
                        || closed.Contains(newPos)
                        || (newPos.X == 0 && newPos.Y == 0))
                    {
                        continue;
                    }

                    //if it's solid, we can abort here after adding it to the closed list
                    if (!IsPassable(newPos, canTraverse, size))
                    {
                        closed.Add(newPos);
                        continue;
                    }

                    var waypoint = new PathNodeAStar(node, newPos, gVal, PathfindingHeuristic(newPos, to));
                    open.Enqueue(waypoint, waypoint.F);
                }
            }
        }

        // if we reach here, then no path was found
        return [];
    }

    /// <summary>
    /// generate a clearance map given the map, and the parts that count as solid
    /// instead of generating collision types
    /// </summary>
    /// <param name="collidesWith">what the entity can collide with</param>
    /// <returns></returns>
    public int[][] GenClearanceMapDEPRECATED(
        string[] collidesWith
        )
    {
        //set defaults
        collidesWith = collidesWith ?? ["obstacle", "all"];

        //bottom-up DP approach
        //annotated A* only checks the bounding box down - right - downright
        var solidCache = new Dictionary<char, bool>();

        var memo = new int[_height][];
        for (int i = 0; i < _height; i++)
            memo[i] = new int[_width];

        //begin from the bottom right
        for (int row = _height - 1; row >= 0; row--)
        {
            for (int col = _width - 1; col >= 0; col--)
            {
                char rep = Map.Tiles[row][col];
                //check whether we've already cached a checkSolid resul
                if (!solidCache.TryGetValue(rep, out bool solid))
                    //if we haven't found a result, then go figure it out (evil syntax warning..)
                    //passes in X, Y format because i am stupid and decided to use points
                    // [ Will probably convert back to int row, int col at some point ]
                    solidCache[rep] = solid = IsPassable(new Point(col, row), collidesWith, -1);

                //if solid, we can cut off the check because it has a clearance value of 0
                if (solid)
                {
                    memo[row][col] = 0;
                    continue;
                }

                //because the immediate bounding box of cell at i,j is the three tiles below,
                //we can assert that its clearance would be one more than the lowest tile value
                int clearance = Math.Min(Math.Min(
                    row + 1 >= _height ? 0 : memo[row + 1][col],
                    col + 1 >= _width ? 0 : memo[row][col + 1]),
                    row + 1 >= _height || col + 1 >= _width ? 0 : memo[row + 1][col + 1]);

                //store the result
                memo[row][col] = clearance + 1;
            }
        }

        /* (this works now, so no need to debug unless this is revisited)
        {
            //visualize the clearance map in the console
            for (int i = 0; i < height; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < width; j++)
                {
                    Console.Write($"{memo[i][j]},   "[..3]);
                }
            }
        }*/

        return memo;
    }
    #endregion

    #region - - - [ scuffed lazy dijkstra's algorithm for pathmap ] - - -
    /// <summary>
    /// Finds all valid paths in range of a tile.
    /// </summary>
    /// <remarks>
    /// I'm actually not sure if this is the best way to go about implementing this, but Manhattan
    /// distance + range constraint didn't work with weighted tiles and i can't think of another
    /// way to get all reachable tiles within a range before iterating through the queue (like
    /// normal Dijkstra's).
    /// </remarks>

    public Dictionary<Point, List<Point>> GetPathMap(Point from, int range, string[] canTraverse)
    {
        var startingNode = new PathNodeDijkstras(null, from);
        var closedNodes = new HashSet<Point>();

        var nodesToSearch = new PriorityQueue<PathNodeDijkstras, float>();
        var bestPaths = new Dictionary<Point, PathNodeDijkstras>();

        // this checks a neighbor tile, and updates the values if a better path was found
        void CheckNeighbor(PathNodeDijkstras from, Point neighbor)
        {
            if (IsOutOfBounds(neighbor) || !IsPassable(neighbor, canTraverse, 1))
                return;
            var neighborWeight = from.Weight + GetNodeWeight(neighbor);
            if (neighborWeight > range)
                return;

            // we only want to update the path if:
            // a) it's better than the old one
            // b) it's the first time we found a path to this neighbor
            if (!bestPaths.TryGetValue(neighbor, out var oldNode) || oldNode.Weight > neighborWeight)
            {
                // set the best path to this neighbor as the path we just made, and then
                // enqueue it for further searching
                var node = new PathNodeDijkstras(from, neighbor) { Weight = neighborWeight };
                bestPaths[neighbor] = node;
                nodesToSearch.Enqueue(node, node.Weight);
            }
        }
        // initialize the search
        bestPaths[from] = startingNode;
        nodesToSearch.Enqueue(startingNode, 0);

        while (nodesToSearch.Count > 0)
        {
            // dequeue and set node as visited, so it isn't re-visited
            var current = nodesToSearch.Dequeue();
            if (closedNodes.Contains(current.Position))
                continue;
            closedNodes.Add(current.Position);

            // check the paths to L/R/T/D neighbors. neighbor will be queued if the path is found
            // to be the shortest path to it
            CheckNeighbor(current, new Point(current.Position.X - 1, current.Position.Y));
            CheckNeighbor(current, new Point(current.Position.X + 1, current.Position.Y));
            CheckNeighbor(current, new Point(current.Position.X, current.Position.Y - 1));
            CheckNeighbor(current, new Point(current.Position.X, current.Position.Y + 1));
        }

        var pathMap = new Dictionary<Point, List<Point>>();
        foreach (var kvp in bestPaths)
            pathMap[kvp.Key] = ReconstructPath(kvp.Value);

        return pathMap;
    }

    #endregion
}
