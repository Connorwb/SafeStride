using System.Collections.Generic;

public enum NodeType
{
    Uninit,
    Waypoint
}
public class Node
{
    public double lat, lon;
    public NodeType nodeType;
}

public class WaypointNode : Node
{
    public List<WaypointNode> connections;
    public List<int> ways;
    public WaypointNode(double inlat, double inlon)
    {
        lat = inlat;
        lon = inlon;
        nodeType = NodeType.Waypoint;
        connections = new List<WaypointNode>();
        ways = new List<int>();
    }
}
