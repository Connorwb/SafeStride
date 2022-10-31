using System.Collections.Generic;

public enum NodeType
{
    Uninit,
    Waypoint,
    Offender
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

public class OffenderNode : Node
{
    public bool transient;
    private bool predator;
    private bool supervised;

    public OffenderNode(double inlat, double inlon, bool transient)
    {
        this.transient = transient;
        lat = inlat;
        lon = inlon;
    }

    public int getThreatLevel()
    {
        return (predator & (!supervised)) ? 2 : 1;
    }
}