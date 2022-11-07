using System.Collections.Generic;

public enum NodeType
{
    Uninit,
    Waypoint,
    Offender
}
public class Node
{
    public float lat, lon;
    public NodeType nodeType;

    public double DegreeDistanceFrom(float latcomp, float loncomp)
    {
        return System.Math.Sqrt(System.Math.Pow(latcomp - lat, 2) + System.Math.Pow(loncomp - lon, 2));
    }
}

public class WaypointNode : Node
{
    public List<WaypointNode> connections;
    public List<int> ways;
    public WaypointNode(double inlat, double inlon)
    {
        lat = (float) inlat;
        lon = (float) inlon;
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
        lat = (float) inlat;
        lon = (float) inlon;
    }

    public int getThreatLevel()
    {
        return (predator & (!supervised)) ? 2 : 1;
    }
}