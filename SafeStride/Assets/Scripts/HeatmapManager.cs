using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class HeatmapManager
{
    Button button;
    Tilemap map;
    TileBase whiteHex;
    GameObject offenderDot;
    List<GameObject> spawnedNodes;

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
    public HeatmapManager(Tilemap map, TileBase whiteHex, GameObject offenderDot, Button button)
    {
        this.map = map;
        this.whiteHex = whiteHex;
        this.offenderDot = offenderDot;
        this.spawnedNodes = new List<GameObject>();
        button.onClick.AddListener(toggleHeatmap);
    }

    public void DisplayInArea(float latcenter, float loncenter, float zoom)
    {
        float width = 1079.03f * Mathf.Exp(-.692919f * zoom) - 0.0000149912f;
        //var horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
        //var vertExtent = Camera.main.orthographicSize;
        //float w2h = Screen.height / Screen.width;

        float horzExtent = 150f;
        float vertExtent = 174f;
        float w2h = 2f;
        //UnityEngine.Debug.Log(horzExtent);
        //UnityEngine.Debug.Log(vertExtent);
        //UnityEngine.Debug.Log(w2h);
        //Vector3 centroid = Camera.main.transform.position;
        DataDownloader downMan = new DataDownloader();
        List<OffenderNode> offenders = downMan.GetOffendersWithin(latcenter - (width * w2h / 2), loncenter - (width/2), latcenter + (width * w2h / 2), loncenter + (width / 2), 0);
        spawnedNodes.Clear();
        foreach (OffenderNode spawnNode in offenders)
        {
            spawnedNodes.Add(GameObject.Instantiate(offenderDot, new Vector3((spawnNode.lon - loncenter) * (horzExtent * 2 / width), 0.50f, (spawnNode.lat - latcenter) * (vertExtent * 2 / (width * w2h))), Quaternion.Euler(90, 0, 0)));
        }

        int tileWidth = 95;
        for (int i = -1 * (int)((w2h) * (tileWidth/3)) ; i < (int)((w2h) * tileWidth); i++)
        {
            for (int ii = -1 * tileWidth; ii < tileWidth; ii++)
            {
                double risk = 0;
                Vector3 pos = map.CellToLocal(new Vector3Int(i, ii, 0));
                float hexlon = 1.25f * ((pos.x / horzExtent) * (width)) + loncenter;
                float hexlat = 1.25f * ((pos.y / vertExtent) * (width * w2h)) + latcenter;
                //foreach (GameObject spawnNode in spawnedNodes)
                //{
                //    risk += 1/ System.Math.Pow(Vector3.Distance(pos, spawnNode.transform.localPosition), 2);
                //}
                foreach (OffenderNode analysis in offenders)
                {
                    risk += 1 / System.Math.Pow(analysis.DegreeDistanceFrom(hexlat, hexlon)*1000,1.5);
                }
                float frisk = (float)risk;
                //UnityEngine.Debug.Log(hexlat + " " + hexlon + " " + frisk);
                map.SetTile(new Vector3Int(i, ii, 0), whiteHex);
                map.SetTileFlags(new Vector3Int(i, ii, 0), TileFlags.None);
                map.SetColor(new Vector3Int(i, ii, 0), new Color(1.0f, 1.0f - frisk/6, 0.0f, System.Math.Min(0.35f, System.Math.Max((frisk-.4f)/6, 0.0f))));
            }
        }
    }

    public void toggleHeatmap()
    {
        map.gameObject.SetActive(!(map.gameObject.activeSelf));
        foreach (GameObject node in spawnedNodes)
        {
            node.SetActive(!(node.activeSelf));
        }
    }
}
