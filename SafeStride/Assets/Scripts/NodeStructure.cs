using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;

public class NodeStructure : MonoBehaviour
{
    void Start()
    {
        DataDownloader downloadMan = new DataDownloader();
        UnityEngine.Debug.Log(downloadMan.ListDatabases());
        return;
        
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
    }
}
