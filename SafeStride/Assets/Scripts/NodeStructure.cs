using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System.Linq;

public class NodeStructure : MonoBehaviour
{
    void Start()
    {
        double startCoordX = 0.0;
        double startCoordY = 0.0;
        double endCoordX = 0.0;
        double endCoordY = 0.0;
        int startID = 0;
        int endID = 0;

        //take in user input from gui

        List<WaypointNode> AStarNodes = new List<WaypointNode>();
        Dictionary<int, List<WaypointNode>> connections = new Dictionary<int, List<WaypointNode>>();
        string execPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string newPath = Path.GetFullPath(Path.Combine(execPath, @"..\..\Assets\Data\Intersections.txt"));

        if (!(File.Exists(newPath)))
        {
            UnityEngine.Debug.Log("Downloading intersections data.");
            string m_Path = Application.dataPath;
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = Path.GetFullPath(Path.Combine(m_Path, @"..\..\MapData\dist\OpenStreetMapInfo\OpenStreetMapInfo.exe"));
            start.Arguments = "-u";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = false;
            using (Process process = Process.Start(start))
            {
            }
            UnityEngine.Debug.Log("Done downloading data.");
        }
        else
        {
            UnityEngine.Debug.Log("Found the intersections data!");
        }
        
        List<WaypointNode> AStarNodes = new List<WaypointNode>();
        Dictionary<int, List<WaypointNode>> connections = new Dictionary<int, List<WaypointNode>>();
        UnityEngine.Debug.Log(newPath);
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
                    UnityEngine.Debug.Log("Linked (" + node.lat + "," + node.lon + ") to (" + checknode.lat + "," + checknode.lon + ")");
                }
            }
        }
        //searching for the starting coordinates
        var start = new AStarNodes();
        start.latY = AStarNodes.FindIndex(x => x.Contains(startCoordY));
        start.lonX = AStarNodes[start.latY].Find(startCoordX);

        //end coordinates now
        var finish = new AStarNodes();
        finish.latY = AStarNodes.FindIndex(x => x.Contains(endCoordY));
        finish.lonX = AStarNodes[finish.latY].Find(endCoordX);

        //finding distance between
        start.SetDistance(finish.lonX, finish.latY);
        
        //making active and visited lists, populating the active list
        var active = new List<AstarNodes>();
        active.Add(start);
        var visited = new List<AStarNodes>();
        
        while (active.any()) //run while there are still active locations
        {

            var checkTile = active.OrderBy(x => x.costDist).First();
            
            if (checkTile.lonX == finish.lonX && checkTile.latY == finish.latY)
            {
                //found shortest distance thanks to OrderBy
                var tile = checkTile;
                while(true)
                {
                    //drawing the path would go here, but idk yet

                    tile = tile.Parent;
                    if (tile == null)
                    {
                        //Console.WriteLine("Done");
                    }
                    
                }
            }
            visited.Add(checkTile);
            active.Remove(checkTile);
        }
    }
}
/*
public class Astar
{
    private static List<locations> GetWalkable(List<WaypointNode> AStarNodes, locations currentLoc, locations targetLoc)
    {
        var possibleLoc = new List<locations>()
        {
            //new locations {lonX,latY,Parent,Cost}
            //new locations {ID = startID},

        };
        

        //possibleLoc.ForEach(locations => locations.SetDistance(targetLoc.lonX, targetLoc.latY));
        return possibleLoc 
                .ToList();

    }
}

class locations
{
    public double lonX { get; set; } 
    public double latY { get; set; } 
    public double cost { get; set; }
    public double distance { get; set; }
    public double costDist => cost + distance;
    public int ID { get; set; } 
    public locations Parent { get; set; }

    public void SetDistance(double targetX, double targetY)
    {
        this.distance = Math.Abs(targetX - lonX) + Math.Abs(targetY - latY);
    }
}
*/
