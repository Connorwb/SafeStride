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
        
        UnityEngine.Debug.Log(newPath);
        IEnumerable<string> lines = File.ReadLines(newPath);
        foreach (string line in lines)
        {
            string[] nodeInfo = line.Split(',');
            WaypointNode newNode = new WaypointNode(System.Convert.ToDouble(nodeInfo[0]), System.Convert.ToDouble(nodeInfo[1]), 0);
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
                WaypointNode startConnect = new WaypointNode();
                startConnect = node;
                startConnect.lat = way.FindIndex(x => x.Contains(startCoordY));
                startConnect.lon = way[startConnect.lat].Find(startCoordX);
                //end coordinates now
                WaypointNode finishConnect = new WaypointNode();
                finishConnect = node;
                finishConnect.lat = way.FindIndex(x => x.Contains(endCoordY));
                finishConnect.lon = way[startConnect.lat].Find(endCoordX);
            
                startConnect.distance = Math.Abs(endCoordX - node.lon) + Math.Abs(endCoordY - node.lat);
                foreach (WaypointNode checknode in way)
                {
                    if (node == checknode) continue;
                    if (node.connections.Contains(checknode)) continue;
                    node.connections.Add(checknode);

                    UnityEngine.Debug.Log("Linked (" + node.lat + "," + node.lon + ") to (" + checknode.lat + "," + checknode.lon + ")");
                }
            }
            /*
            //searching for the starting coordinates
            WaypointNode startConnect = new WaypointNode();
            startConnect = node;
            startConnect.lat = way.FindIndex(x => x.Contains(startCoordY));
            startConnect.lon = way[startConnect.lat].Find(startCoordX);
            //end coordinates now
            WaypointNode finishConnect = new WaypointNode();
            finishConnect = node;
            finishConnect.lat = way.FindIndex(x => x.Contains(endCoordY));
            finishConnect.lon = way[startConnect.lat].Find(endCoordX);
            
            startConnect.distance = Math.Abs(endCoordX - node.lon) + Math.Abs(endCoordY - node.lat); 
            */
        }
       
        //making active and visited lists, populating the active list
        var active = new List<WaypointNode>();
        active.Add(startConnect);
        var visited = new List<WaypointNode>();
        
        while (active.any()) //run while there are still active locations
        {
            
            var checkTile = active.OrderBy(x => x.costDist).First();
            
            if (checkTile.lon == finishConnect.lon && checkTile.lat == finishConnect.lat)
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
}

