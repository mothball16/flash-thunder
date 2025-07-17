using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace LettersInTheBoxGame.Systems;


public class PathNode
{
    public float G { get; set; }
    public float H { get; set; }
    public PathNode Parent { get; set; }
    public Point Position { get; set; }
    public float F
        => G + H;

    public PathNode(PathNode parent, Point position, float g, float h)
    {
        Parent = parent;
        Position = position;
        G = g;
        H = h;
    }
}


/// <summary>
/// true clearance pathfinding thingamajig for tile-based maps
/// tried to loosely couple this as much as i can since im going to re-use this for diff projects
/// (at the cost of readability and complexity -- i have to constantly pull info from TileManager, 
/// which kinda sucks. might reconsider this later)
///
/// (whiteboard for proj.)
/// a new pathfinder should be made to handle groups with distinct solid/heuristic checks
/// it should not be within any entities
/// we could use events to do a RequestPath? i dont want to make an event bus tho
/// but usually one should be fine?
/// https://harablog.wordpress.com/2009/01/29/clearance-based-pathfinding/
/// </summary>
public class Pathfinder<T>
{

    /// <summary>
    /// this determines whether a point on the map is impassible
    /// passing as a point after geting the node just to convert back to th node is a bit fucky
    /// but i need the coords for clearance map checks..
    /// </summary>
    public Func<Point, string[], int, bool> CheckSolid;

    /// <summary>
    /// this determines the value to add onto the node's weight
    /// gives from and to, should be enough to implement the two common heuristics +
    /// heatmap/preferred routes?
    /// REMINDER!!!!!!!!!!!!!! THIS IS NOT PASSING BY [row, col]!!!!! THIS IS X, Y!!!!!
    /// </summary>
    public Func<Point, Point, float> Heuristic;
    /// <summary>
    /// generate a clearance map given the map, and the parts that count as solid
    /// instead of generating collision types
    /// </summary>
    /// <param name="map">the 2D array representing the tilemap</param>
    /// <param name="collidesWith">what the entity can collide with</param>
    /// <returns></returns>
    public int[,] GenClearanceMap(
        T[,] map,
        string[] collidesWith
        )
    {
        //set defaults
        collidesWith = collidesWith ?? ["obstacle", "all"];

        //bottom-up DP approach
        //annotated A* only checks the bounding box down - right - downright
        Dictionary<T, bool> solidCache = new();
        int height = map.GetLength(0);
        int width = map.GetLength(1);
        int[,] memo = new int[height, width];

        //begin from the bottom right
        for (int row = height - 1; row >= 0; row--)
        {
            for (int col = width - 1; col >= 0; col--)
            {
                T rep = map[row, col];
                //check whether we've already cached a checkSolid resul
                if (!solidCache.TryGetValue(rep, out bool solid))
                    //if we haven't found a result, then go figure it out (evil syntax warning..)
                    //passes in X, Y format because i am stupid and decided to use points
                    // [ Will probably convert back to int row, int col at some point ]
                    solidCache[rep] = solid = CheckSolid(new Point(col, row), collidesWith, -1);

                //if solid, we can cut off the check because it has a clearance value of 0
                if (solid)
                {
                    memo[row, col] = 0;
                    continue;
                }

                //because the immediate bounding box of cell at i,j is the three tiles below,
                //we can assert that its clearance would be one more than the lowest tile value
                int clearance = Math.Min(Math.Min(
                    row + 1 >= height ? 0 : memo[row + 1, col],
                    col + 1 >= width ? 0 : memo[row, col + 1]),
                    row + 1 >= height || col + 1 >= width ? 0 : memo[row + 1, col + 1]);

                //store the result
                memo[row, col] = clearance + 1;
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
                    Console.Write($"{memo[i, j]},   "[..3]);
                }
            }
        }*/


        return memo;
    }

    /// <summary>
    /// from a point, determines a path to the other point/whether there is a possible path to begin with
    /// </summary>
    /// <param name="map">the 2D array of character representatives</param>
    /// <param name="collidesWith">the collisiongroups </param>
    /// <param name="from">the origin, pass this as (X, Y)</param>
    /// <param name="to">the goal, pass this as (X,Y)</param>
    /// <returns>the list of points starting from the origin to the goal (or none, if no path was found)</returns>
    public List<Point> Pathfind(
        T[,] map,
        string[] collidesWith,
        Point from,
        Point to,
        int size = 1
        )
    {
        //init. data structures
        PriorityQueue<PathNode, float> open = new();
        HashSet<Point> closed = [];
        Dictionary<Point, float> gValues = [];
        //shortcut the map dimensions
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        //init. and enqueue the first node
        PathNode start = new(null, from, 0, Heuristic(from, to));
        open.Enqueue(start, start.F);

        //this will end when all nodes have been iterated throguh
        //(this should probably be calculating in regions instead, but monogame should be able
        //to handle this...?)
        //(other solution could be only checking within 2x the visible region of the map, any
        //errors in pathfinding wouldn't be obvious to the player)
        while (open.Count > 0)
        {
            PathNode node = open.Dequeue();
            int xDiff = to.X - node.Position.X;
            int yDiff = to.Y - node.Position.Y;
            //if we reach goal, return the reconstructed path
            if (node.Position == to ||
                //naive fix to enemies not pathing to corners of clearance zones/pathing too far
                //this is not a good solution optimization wise because it has to brute force the
                //solution when the path is occluded, but im leaving the elegant solutions to
                //the Cs majors (Losers !!!)
                (size > 1 && xDiff < size && xDiff >= 0 && yDiff < size && yDiff >= 0))

                return ReconstructPath(node);

            closed.Add(node.Position);

            //look thru all neighbors within 1 block distance
            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    Point newPos = new Point(node.Position.X + col, node.Position.Y + row);

                    float distSq = row != 0 && col != 0
                        ? 1.4f //approximately sqrt(2) for a diagonal movement
                        : 1; //the move is either horizontal or vertical
                    float gVal = node.g + distSq;

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
                    if (newPos.X < 0 || newPos.Y < 0 || newPos.Y >= height || newPos.X >= width
                        || closed.Contains(newPos) || newPos.X == 0 && newPos.Y == 0)
                        continue;

                    //if it's solid, we can abort here after adding it to the closed list
                    if (CheckSolid(newPos, collidesWith, size))
                    {
                        closed.Add(newPos);
                        continue;
                    }
                    //the node is actually valid: do the thingy

                    PathNode nNode = new PathNode(node, newPos, gVal, Heuristic(newPos, to));
                    open.Enqueue(nNode, nNode.F);
                }
            }
        }

        //there was no path found, send an empty list back
        return new List<Point>();
    }

    /// <summary>
    /// from a completed pathfind, convert to a more usable normal list
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private static List<Point> ReconstructPath(PathNode n)
    {
        List<Point> path = [];
        //convert the knockoff linked list to a normal list (but inverted)
        while (n != null)
        {
            path.Add(n.Position);
            n = n.Parent;
        }

        //the path is currently leading with the goal in front, so flip it
        path.Reverse();
        return path;
    }
}
