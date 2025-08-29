using FlashThunder.GameLogic.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlashThunder.GameLogic.Movement.Services;
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

    #region - - - [ scuffed lazy dijkstra's algorithm for pathmap ] - - -
    /// <summary>
    /// Finds all valid paths in range of a tile.
    /// </summary>
    /// <remarks>
    /// I'm actually not sure if this is the best way to go about implementing this, but Manhattan
    /// distance as the range constraint didn't work with weighted tiles and i can't think of another
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
