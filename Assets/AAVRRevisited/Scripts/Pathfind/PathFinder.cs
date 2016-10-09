using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfind;
 
public static class PathFinder
{
    //distance f-ion should return distance between two adjacent nodes
    //estimate should return distance between any node and destination node
    public static Path FindPath( Tile start, Tile destination) {
        //set of already checked nodes
        List<Tile> closed = new List<Tile>();
        //queued nodes in open set
        PriorityQueue<float, Path> queue = new PriorityQueue<float, Path>();
        queue.Enqueue(0f, new Path(start));
 
        while (!queue.IsEmpty)
        {
            var path = queue.Dequeue();
 
            if (closed.Contains(path.LastStep))
                continue;
            if (path.LastStep.Equals(destination))
                return path;
 
            closed.Add(path.LastStep);
            List<Tile> neighbours = path.LastStep.GetWalkableNeighbours();
 
            foreach (Tile t in neighbours)
            {
                float d = 1f;
                //new step added without modifying current path
                Path newPath = path.AddStep(t, d);
                queue.Enqueue((float) (newPath.TotalCost + (start.gameObject.transform.localPosition-start.gameObject.transform.localPosition).magnitude), newPath);
            }
        }
 
        return null;
    }
    public static bool IsReachable(Tile start, Tile end) {
        Path foundPath = FindPath(start, end);
        return !(foundPath ==null);
    }
}