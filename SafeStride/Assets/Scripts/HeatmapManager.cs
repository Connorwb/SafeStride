using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeatmapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private TileBase whiteHex;

    [SerializeField]
    public GameObject offenderDot;

    // Start is called before the first frame update
    void Start()
    {
        DisplayInArea(29.182873f, -81.052624f, 0.15f);
        /*map.SetTile(new Vector3Int(0, 0, 0), whiteHex);
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
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayInArea(float latcenter, float loncenter, float width)
    {
        var horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        var vertExtent = Camera.main.orthographicSize;
        float w2h = Screen.height / Screen.width;
        UnityEngine.Debug.Log(horzExtent);
        UnityEngine.Debug.Log(vertExtent);
        UnityEngine.Debug.Log(w2h);
        Vector3 centroid = Camera.main.transform.position;
        DataDownloader downMan = new DataDownloader();
        List<OffenderNode> offenders = downMan.GetOffendersWithin(latcenter - (width * w2h / 2), loncenter - (width/2), latcenter + (width * w2h / 2), loncenter + (width / 2), 0);
        foreach (OffenderNode spawnNode in offenders)
        {
            Instantiate(offenderDot, new Vector3((spawnNode.lat - latcenter) * (horzExtent*2/(width*w2h)), (spawnNode.lon - loncenter) * (vertExtent*2 / width), 0), Quaternion.identity);
            //this doesn't seem right???
        }

        int tileWidth = 35;
        for (int ii = -1 * tileWidth; ii < tileWidth; ii++)
        {
            for (int i = -1 * ((int) w2h) * tileWidth; i < ((int)w2h) * tileWidth; i++)
            {
                double risk = 0;
                Vector3 pos = map.CellToLocal(new Vector3Int(i, ii, 0));
                float hexlon = ((pos.x / tileWidth) * width / 2) + loncenter;
                float hexlat = ((pos.y / (tileWidth*w2h)) * (width*w2h) / 2) + latcenter;
                foreach (OffenderNode analysis in offenders)
                {
                    risk += 1 / System.Math.Pow(analysis.DegreeDistanceFrom(hexlat, hexlon)*1000,1.2);
                }
                float frisk = (float)risk;
                UnityEngine.Debug.Log(hexlat + " " + hexlon + " " + frisk);
                map.SetTile(new Vector3Int(i, ii, 0), whiteHex);
                map.SetTileFlags(new Vector3Int(i, ii, 0), TileFlags.None);
                map.SetColor(new Vector3Int(i, ii, 0), new Color(1.0f, 1.0f - frisk/3, 0.0f, 0.35f));
            }
        }
    }
}
