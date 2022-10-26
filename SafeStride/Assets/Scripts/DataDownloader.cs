using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using UnityEngine;

public class DataDownloader
{
    void Download()
    {
        
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

    public List<WaypointNode> GetWaypointsWithin(double latmin, double lonmin, double latmax, double lonmax, double buffer)
    {
        List<WaypointNode> waypoints = new List<WaypointNode>();
        MongoClient dbClient = new MongoClient("mongodb+srv://bramhalc:qAGBTrJ6U4McuEDl@safestride.pum3uy6.mongodb.net/test");
        var database = dbClient.GetDatabase("WaypointData");
        var collection = database.GetCollection<BsonDocument>("Intersections");
        UnityEngine.Debug.Log(collection.EstimatedDocumentCount());
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
        }
        return waypoints;
    }
}
