using MongoDB.Driver;

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
}
