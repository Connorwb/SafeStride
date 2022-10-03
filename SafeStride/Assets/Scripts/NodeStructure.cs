using System.Collections;
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
