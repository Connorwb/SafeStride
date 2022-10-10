﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

public class WaypointNode : Node {
    public List<WaypointNode> connections;
    public WaypointNode(double inlat, double inlon)
    {
        lat = inlat;
        lon = inlon;
        nodeType = NodeType.Waypoint;
        connections = new List<WaypointNode>();
    }
}

public class NodeStructure : MonoBehaviour
{
    void Start()
    {
        List<WaypointNode> AStarNodes = new List<WaypointNode>();
        Dictionary<int, List<WaypointNode>> connections = new Dictionary<int, List<WaypointNode>>();
        string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string newPath = Path.GetFullPath(Path.Combine(execPath, @"..\..\Assets\Data\Intersections.txt"));
        Debug.Log(newPath);
        IEnumerable<string> lines = File.ReadLines(newPath);
        foreach (string line in lines)
        {
            string[] nodeInfo = line.Split(',');
            WaypointNode newNode = new WaypointNode(System.Convert.ToDouble(nodeInfo[0]), System.Convert.ToDouble(nodeInfo[1]));
            AStarNodes.Add(newNode);
            for (int i = 2; i < nodeInfo.Length; i++)
            {
                int linker = AStarNodes.Count - 1;
                int wayKey = System.Convert.ToInt32(nodeInfo[i]);
                if (connections.ContainsKey(wayKey))
                {
                    connections[wayKey].Add(AStarNodes[linker]);
                } 
                else
                {
                    List<WaypointNode> newList = new List<WaypointNode>();
                    newList.Add(AStarNodes[linker]);
                    connections.Add(wayKey, newList);
                }
            }
        }
        foreach (List<WaypointNode> way in connections.Values)
        {
            foreach (WaypointNode node in way)
            {
                foreach (WaypointNode checknode in way)
                {
                    if (node == checknode) continue;
                    if (node.connections.Contains(checknode)) continue;
                    node.connections.Add(checknode);
                    Debug.Log("Linked (" + node.lat + "," + node.lon + ") to (" + checknode.lat + "," + checknode.lon + ")");
                }
            }
        }
    }
}
public class Astar
{
    static void main(string[] args)
    {
        var startCoordX = 0.0;
        var startCoordY = 0.0;
        var endCoordX = 0.0;
        var endCoordY = 0.0;
        var startID = 0;
        var endID = 0;
        
        //this should all be taking in user input via gui, but...
        Console.WriteLine("Enter Start X(longitude) coordinate");
        try {
            startCoordX = Console.ReadLine();
        }catch{
            Console.WriteLine("Please enter a valid Coordinate");
        }
        Console.WriteLine("Enter Start Y(latitude) coordinate");
        try {
            startCoordY = Console.ReadLine();
        }catch{
            Console.WriteLine("Please enter a valid Coordinate");
        }
        Console.WriteLine("Enter End X(longitude) coordinate");
        try {
            endCoordX = Console.ReadLine();
        }catch{
            Console.WriteLine("Please enter a valid Coordinate");
        }
        Console.WriteLine("Enter End Y(latitude) coordinate");
        try {
            endCoordY = Console.ReadLine();
        }catch{
            Console.WriteLine("Please enter a valid Coordinate");
        }
        Console.WriteLine("Enter Start ID");
        try {
            startCoordX = Console.ReadLine();
        }catch{
            Console.WriteLine("Please enter a valid ID");
        }
        Console.WriteLine("Enter End ID");
        try {
            startCoordX = Console.ReadLine();
        }catch{
            Console.WriteLine("Please enter a valid ID");
        }
        

        //searching for the starting coordinates
        var start = new locations();
        start.latY = AStarNodes.FindIndex(x => x.Contains(startCoordY));
        start.lonX = AStarNodes[start.latY].Find(startCoordX);

        //end coordinates now
        var finish = new locations();
        finish.latY = AStarNodes.FindIndex(x => x.Contains(endCoordY));
        finish.lonX = AStarNodes[finish.latY].Find(endCoordX);

        //finding distance between
        start.SetDistance(finish.lonX, finish.latY);

        //making active and visited lists, populating the active list
        var active = new List<locations>();
        active.Add(start);
        var visited = new List<locations>();

        while (active.any()) //run while there are still active locations
        {

            var checkTile = active.OrderBy(x => x.costDist).First();
            
            if (checkTile.X == finish.X && checkTile.Y == finish.Y)
            {
                //found shortest distance thanks to OrderBy
                var tile = checkTile;
                while(true)
                {
                    //drawing the path would go here, but idk yet

                    tile = tile.Parent;
                    if (tile == null)
                    {
                        Console.WriteLine("Done");
                    }
                    
                }
            }
            visited.Add(checkTile);
            active.Remove(checkTile);
        }
    }
    private static List<locations> GetWalkable(List<WaypointNode> AStarNodes, locations currentLoc, locations targetLoc)
    {
        var possibleLoc = new List<locations>()
        {
            //new locations {lonX,latY,Parent,Cost}
            new locations {ID = startID},

        };

        //possibleLoc.ForEach(locations => locations.SetDistance(targetLoc.lonX, targetLoc.latY));
        return possibleLoc 
                .ToList();

    }
}
class locations
{
    public double lonX { get; set; } = WaypointNode.lon;
    public double latY { get; set; } = WaypointNode.lat;
    public double Cost { get; set; }
    public double Distance { get; set; }
    public double costDist => Cost + Distance;
    public int ID { get; set; } = WaypointNode.connections;
    public locations Parent { get; set; }

    public void SetDistance(int targetX, int targetY)
    {
        this.Distance = Math.Abs(targetX - lonX) + Math.Abs(targetY - latY);
    }
}