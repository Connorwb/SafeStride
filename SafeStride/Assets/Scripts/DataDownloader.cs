using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataDownloader
{
    public List<WaypointNode> GWWrequest;
    public List<OffenderNode> GOWrequest;
    
    public DataDownloader()
    {
        GWWrequest = null;
        GOWrequest = null;
    }
    public string ListDatabases()
    {
        string retString = "";
        MongoClient dbClient = new MongoClient("mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/test");
        var dbList = dbClient.ListDatabases().ToList();

        retString += "The list of databases on this server is: ";
        foreach (var db in dbList)
        {
            retString += db + ",";
        }
        return retString;
    }

    public void GetWaypointsWithin(double latmin, double lonmin, double latmax, double lonmax, double buffer)
    {
        List<WaypointNode> waypoints = new List<WaypointNode>();
        MongoClient dbClient = new MongoClient("mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/test");
        var database = dbClient.GetDatabase("WaypointData");
        var collection = database.GetCollection<BsonDocument>("Intersections");
        var filterBuilder = Builders<BsonDocument>.Filter;
        var filter = filterBuilder.Gt("latitude", latmin) & filterBuilder.Lt("latitude", latmax) 
            & filterBuilder.Gt("longitude", lonmin) & filterBuilder.Lt("longitude", lonmax);
        var queryResults = collection.Find(filter);
        var cursor = collection.Find(filter).ToCursor();
        foreach (var document in cursor.ToEnumerable())
        {
            WaypointNode newnode = new WaypointNode(document.GetValue("latitude").AsDouble, document.GetValue("longitude").AsDouble);
            BsonArray waysBson = document.GetValue("connections").AsBsonArray;
            foreach (BsonValue way in waysBson)
            {
                newnode.ways.Add(way.AsInt32);
            }
            waypoints.Add(newnode);
        }
        GWWrequest = waypoints;
    }

    public void GetOffendersWithin(double latmin, double lonmin, double latmax, double lonmax, double buffer)
    {
        List<OffenderNode> waypoints = new List<OffenderNode>();
        MongoClient dbClient = new MongoClient("mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/test");
        var database = dbClient.GetDatabase("OffenderData");
        var collection = database.GetCollection<BsonDocument>("initialExcel");
        var filterBuilder = Builders<BsonDocument>.Filter;
        var filter = filterBuilder.Gt("latitude", latmin) & filterBuilder.Lt("latitude", latmax)
            & filterBuilder.Gt("longitude", lonmin) & filterBuilder.Lt("longitude", lonmax);
        var queryResults = collection.Find(filter);
        var cursor = collection.Find(filter).ToCursor();
        foreach (var document in cursor.ToEnumerable())
        {
            OffenderNode newnode = new OffenderNode(document.GetValue("latitude").AsDouble, document.GetValue("longitude").AsDouble, (document.GetValue("TRANSIENT").AsString == "TRUE"));
            waypoints.Add(newnode);
        }
        GOWrequest = waypoints;
    }
}
