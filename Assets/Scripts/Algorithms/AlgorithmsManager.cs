using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class AlgorithmsManager
{
    private static readonly Vector2Int[] Directions = {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };
    public static void GenerateAldousBroder(BoundsInt bounds, out HashSet<Vector2Int> floors, out HashSet<Vector2Int> walls)
    {
        floors = new HashSet<Vector2Int>();
        walls = new HashSet<Vector2Int>();

        foreach (var pos in bounds.allPositionsWithin)
        {
            walls.Add(new Vector2Int(pos.x, pos.y));
        }
        int totalNodes = 0;
        for (int i = bounds.xMin + 1; i < bounds.xMax - 1; i += 2)
        {
            for (int j = bounds.yMin + 1; j < bounds.yMax - 1; j += 2)
            {
                totalNodes++;
            }
        }
        Vector2Int current = new Vector2Int(
            bounds.xMin + 1 + (Random.Range(0, (bounds.size.x - 2) / 2) * 2),
            bounds.yMin + 1 + (Random.Range(0, (bounds.size.y - 2) / 2) * 2)
        );
        HashSet<Vector2Int> visitedNodes = new HashSet<Vector2Int>();
        visitedNodes.Add(current);
        floors.Add(current);
        walls.Remove(current);

        while (visitedNodes.Count < totalNodes)
        {
            Vector2Int dir = Directions[Random.Range(0, Directions.Length)];
            Vector2Int next = current + dir * 2;

            if (next.x > bounds.xMin && next.x < bounds.xMax - 1 &&
                next.y > bounds.yMin && next.y < bounds.yMax - 1)
            {
                if (!visitedNodes.Contains(next))
                {
                    Vector2Int wallToBreak = current + dir;
                    floors.Add(wallToBreak);
                    walls.Remove(wallToBreak);

                    floors.Add(next);
                    walls.Remove(next);
                    visitedNodes.Add(next);
                }
                current = next;
            }
        }
    }
    public static PathFindingResult GetBFSExploration(Vector2Int start, Vector2Int end, HashSet<Vector2Int> floorPositions)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();
        List<Vector2Int> explorationPath = new List<Vector2Int>();
        List<Vector2Int> finalPath = new List<Vector2Int>();

        bool found = false;

        queue.Enqueue(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            explorationPath.Add(current);

            if (current == end)
            {
                found = true;
                break;
            }
            foreach (var dir in Directions)
            {
                Vector2Int next = current + dir;
                if (floorPositions.Contains(next) && !visited.Contains(next))
                {
                    visited.Add(next);
                    parentMap[next] = current;
                    queue.Enqueue(next);
                }
            }
        }
        if (found)
        {
            Vector2Int temp = end;
            while (temp != start)
            {
                finalPath.Add(temp);
                temp = parentMap[temp];
            }
            finalPath.Add(start);
            finalPath.Reverse();
        }
        return new PathFindingResult
        {
            ExplorationPath = explorationPath,
            FinalPath = finalPath
        };
    }
    public static PathFindingResult GetDFSExploration(Vector2Int start, Vector2Int end, HashSet<Vector2Int> floorPositions)
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();
        List<Vector2Int> explorationPath = new List<Vector2Int>();
        List<Vector2Int> finalPath = new List<Vector2Int>();
        bool found = false;
        stack.Push(start);
        visited.Add(start);
        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();
            explorationPath.Add(current);
            if (current == end)
            {
                found = true;
                break;
            }
            foreach (var dir in Directions)
            {
                Vector2Int next = current + dir;
                if (floorPositions.Contains(next) && !visited.Contains(next))
                {
                    visited.Add(next);
                    parentMap[next] = current;
                    stack.Push(next);
                }
            }
        }
        if (found)
            {
                Vector2Int temp = end;
                while (temp != start)
                {
                    finalPath.Add(temp);
                    temp = parentMap[temp];
                }
                finalPath.Add(start);
                finalPath.Reverse();
        }
        return new PathFindingResult
        {
            ExplorationPath = explorationPath,
            FinalPath = finalPath
        };
    }
    public static PathFindingResult GetAStarExploration(Vector2Int start, Vector2Int end, HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> exploration = new List<Vector2Int>();
        List<Vector2Int> finalPath = new List<Vector2Int>();
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();

        var openSet = new SortedSet<(float f, Vector2Int pos)>(Comparer<(float f, Vector2Int pos)>.Create((a, b) =>
        {
            int compare = a.f.CompareTo(b.f);
            if (compare == 0) return a.pos.GetHashCode().CompareTo(b.pos.GetHashCode());
            return compare;
        }));
        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);
        openSet.Add((fScore[start], start));

        while(openSet.Count > 0)
        {
            var current = openSet.Min.pos;
            openSet.Remove(openSet.Min);
            exploration.Add(current);
            if (current == end)
            {
                finalPath = ReconstructPath(parentMap, current);
                break;
            }
            foreach (var dir in Directions)
            {
                Vector2Int next = current + dir;
                if (!floorPositions.Contains(next)) continue;
                float tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(next) || tentativeGScore < gScore[next])
                {
                    parentMap[next] = current;
                    gScore[next] = tentativeGScore;
                    fScore[next] = gScore[next] + Heuristic(next, end);
                    openSet.Add((fScore[next], next));
                }
            }
        }
        return new PathFindingResult
        {
            ExplorationPath = exploration,
            FinalPath = finalPath
        };
    }
    private static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    private static List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> parentMap, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (parentMap.ContainsKey(current))
        {
            current = parentMap[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}
public struct PathFindingResult
{
    public List<Vector2Int> ExplorationPath;
    public List<Vector2Int> FinalPath;
}

