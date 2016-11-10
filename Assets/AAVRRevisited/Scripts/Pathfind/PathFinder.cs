using System.Collections.Generic;
using UnityEngine;
using Pathfind;

public enum PathParameter {
    AnyTile,
    Walkable,
    WalkableAndVisible
}
 
public static class PathFinder
{
    //distance f-ion should return distance between two adjacent nodes
    //estimate should return distance between any node and destination node
    public static Path FindPath( Tile start, Tile destination, PathParameter parameter = PathParameter.Walkable) {
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
            
            List<Tile> neighbours = new List<Tile>();
            closed.Add(path.LastStep);
            if(parameter == PathParameter.AnyTile) {
                neighbours = path.LastStep.GetNeighbourTiles();
            } else if (parameter == PathParameter.Walkable){
                neighbours = path.LastStep.GetWalkableNeighbours();
            } else if(parameter == PathParameter.WalkableAndVisible) {
                neighbours = path.LastStep.GetWalkableAndVisibleNeighbours();
            }
            if(neighbours.Count == 0) {
                Debug.Log("Could not find Neighbours for "+path.LastStep.gameObject.name);
            }            
 
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