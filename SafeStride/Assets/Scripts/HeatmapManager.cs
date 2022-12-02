using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using Mapbox.Unity.Map;

public class HeatmapManager
{
    Button button;
    Tilemap tilemap;
    TileBase whiteHex;
    GameObject offenderDot;
    List<GameObject> spawnedNodes;
    AbstractMap map;
    public bool init;

    // Start is called before the first frame update
    /*void Start()
    {
        DisplayInArea(29.182873f, -81.052624f, 0.15f);
        *//*map.SetTile(new Vector3Int(0, 0, 0), whiteHex);
        map.SetColor(new Vector3Int(0, 0, 0), new Color(1.0f, 0.5f, 0.0f, 0.35f));
        map.SetTileFlags(new Vector3Int(0, 0, 0), TileFlags.None);
        for (int i = 0; i < 70; i++)
        {
            for (int ii = 0; ii < 35; ii++)
            {
                map.SetTile(new Vector3Int(i, ii, 0), whiteHex);
                map.SetTileFlags(new Vector3Int(i, ii, 0), TileFlags.None);
                map.SetColor(new Vector3Int(i, ii, 0), new Color(1.0f, i * ii /2450.0f, 0.0f, 0.35f));
                if (i + ii == 0) { continue; }
                map.SetTile(new Vector3Int(-i, ii, 0), whiteHex);
                map.SetTileFlags(new Vector3Int(-i, ii, 0), TileFlags.None);
                map.SetColor(new Vector3Int(-i, ii, 0), new Color(1.0f, i * ii / 2450.0f, 0.0f, 0.35f));
                map.SetTile(new Vector3Int(i, -ii, 0), whiteHex);
                map.SetTileFlags(new Vector3Int(i, -ii, 0), TileFlags.None);
                map.SetColor(new Vector3Int(i, -ii, 0), new Color(1.0f, i * ii / 2450.0f, 0.0f, 0.35f));
                map.SetTile(new Vector3Int(-i, -ii, 0), whiteHex);
                map.SetTileFlags(new Vector3Int(-i, -ii, 0), TileFlags.None);
                map.SetColor(new Vector3Int(-i, -ii, 0), new Color(1.0f, i * ii / 2450.0f, 0.0f, 0.35f));
            }
        }
    }*/
    public HeatmapManager(Tilemap map, TileBase whiteHex, GameObject offenderDot)
    {
        this.tilemap = map;
        this.whiteHex = whiteHex;
        this.offenderDot = offenderDot;
        this.spawnedNodes = new List<GameObject>();
        this.init = false;
    }

    public IEnumerator DisplayInArea(AbstractMap _map, float latcenter, float loncenter, float zoom)
    {
        UnityEngine.Debug.Log("Initialized");
        init = true;
        float width = (1079.03f * Mathf.Exp(-.692919f * zoom) - 0.0000149912f)/1.6f;
        //var horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        //var vertExtent = Camera.main.orthographicSize;
        //float w2h = Screen.height / Screen.width;

        //float horzExtent = 150f;
        //float vertExtent = 174f;
        float w2h = 2f;
        //UnityEngine.Debug.Log(horzExtent);
        //UnityEngine.Debug.Log(vertExtent);
        //UnityEngine.Debug.Log(w2h);
        //Vector3 centroid = Camera.main.transform.position;
        if (spawnedNodes.Count > 0)
        {
            foreach (GameObject node in spawnedNodes)
            {
                node.Destroy();
            }
            spawnedNodes.Clear();
        }
        DataDownloader downMan = new DataDownloader();
        Task.Factory.StartNew(() => Task.Factory.StartNew(() => downMan.GetOffendersWithin(latcenter - (width * w2h / 2), loncenter - (width / 2), latcenter + (width * w2h / 2), loncenter + (width / 2), 0)));
        while (downMan.GOWrequest == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        foreach (OffenderNode spawnNode in downMan.GOWrequest)
        {
            GameObject obj = GameObject.Instantiate(offenderDot, _map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(spawnNode.lat, spawnNode.lon)) + new Vector3(0, 0.01f, 0), Quaternion.Euler(90, 0, 0));
            obj.SetActive(true);
            spawnedNodes.Add(obj);
        }

        yield return new WaitForSeconds(0.001f);
        int tileWidth = 95;
        int next = -1;
        for (int ii = 0; ii < tileWidth;)
        {
            for (int i = -1 * (int)((w2h) * (tileWidth / 3)); i < (int)((w2h) * tileWidth); i++)
            {
                float risk = 0;
                Vector3 pos = tilemap.GetCellCenterWorld(new Vector3Int(i, ii, 0));
                //Vector3 pos = tilemap.CellToLocal(new Vector3Int(i, ii, 0));
                //Mapbox.Utils.Vector2d conv = _map.WorldToGeoPosition(pos);
                //UnityEngine.Debug.Log(conv);
                //UnityEngine.Debug.Log(_map.CenterLatitudeLongitude);
                //float hexlon = (float) conv.y;
                //float hexlat = (float) conv.x;
                foreach (GameObject spawnNode in spawnedNodes)
                {
                    risk += Mathf.Pow((spawnNode.transform.position - pos).magnitude/3, -1.5f);
                }
                //foreach (OffenderNode analysis in downMan.GOWrequest)
                //{
                //    risk += 1 / System.Math.Pow(analysis.DegreeDistanceFrom(hexlat, hexlon)*1000,1.5);
                //}
                //UnityEngine.Debug.Log(hexlat + " " + hexlon + " " + frisk);
                tilemap.SetTile(new Vector3Int(i, ii, 0), whiteHex);
                tilemap.SetTileFlags(new Vector3Int(i, ii, 0), TileFlags.None);
                tilemap.SetColor(new Vector3Int(i, ii, 0), new Color(1.0f, 1.0f - risk/6, 0.0f, System.Math.Min(0.35f, System.Math.Max((risk-.4f)/6, 0.0f))));
            }
            if (next < 0)
            {
                ii = next;
                next = 0 - next;
            } 
            else
            {
                ii = next;
                next++;
                next = 0 - next;
            }
            if (next > 0) yield return new WaitForSeconds(0.25f);
        }
    }

    public void toggleHeatmap()
    {
        tilemap.gameObject.SetActive(!(tilemap.gameObject.activeSelf));
        foreach (GameObject node in spawnedNodes)
        {
            node.SetActive(!(node.activeSelf));
        }
    }
}
