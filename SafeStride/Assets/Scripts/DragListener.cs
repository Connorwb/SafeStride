using System.Collections;
using UnityEngine;
using Mapbox.Unity.Map;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragListener : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private TileBase whiteHex;

    [SerializeField]
    public GameObject offenderDot;

    [SerializeField]
    Button button;

    [SerializeField]
    GameObject hidebutton1;

    [SerializeField]
    GameObject hidebutton2;

    [SerializeField]
    GameObject hidebutton3;

    [SerializeField]
    GameObject loc1;

    [SerializeField]
    GameObject loc2;

    private Mapbox.Utils.Vector2d bruh;
    private Mapbox.Utils.Vector2d bruh2;

    private HeatmapManager heatManager;

    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    private Vector3 cameraOrigin;
    private Coroutine routine;

    private void Start()
    {
        cameraOrigin = Camera.main.transform.position;
        button.onClick.AddListener(toggleHeatmap);
        heatManager = new HeatmapManager(tilemap, whiteHex, offenderDot);
        tilemap.gameObject.SetActive(false);
        Vector3 firsts = loc1.transform.position;
        firsts.y = 0;
        bruh = _map.WorldToGeoPosition(firsts);
        Vector3 seconds = loc2.transform.position;
        seconds.y = 0;
        bruh2 = _map.WorldToGeoPosition(seconds);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0)) StartCoroutine(Replace());
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        Camera.main.transform.Translate(move, Space.World);
    }

    IEnumerator Replace()
    {
        if (IsPointerOverUIObject()) yield break;
        var v3 = Input.mousePosition;
        v3.y = 0.0f;
        Vector3 dragNew = Camera.main.transform.position;
        dragNew.y = 0;
        Vector3 dragOld = cameraOrigin;
        dragOld.y = 0;
        Mapbox.Utils.Vector2d translateVec =  _map.WorldToGeoPosition(dragNew) - _map.WorldToGeoPosition(dragOld);
        _map.UpdateMap(translateVec + _map.CenterLatitudeLongitude);
        Camera.main.transform.SetPositionAndRotation(cameraOrigin, Camera.main.transform.rotation);
        if (!(float.IsNaN(_map.GeoToWorldPosition(bruh).x))) {
            loc1.transform.SetPositionAndRotation(_map.GeoToWorldPosition(bruh) + new Vector3(0,0.2f,0), loc1.transform.rotation);
        } else {
            Vector3 firsts = loc1.transform.position;
            firsts.y = 0;
            bruh = _map.WorldToGeoPosition(firsts);
        }
        if (!(float.IsNaN(_map.GeoToWorldPosition(bruh2).x))) {
            loc2.transform.SetPositionAndRotation(_map.GeoToWorldPosition(bruh2) + new Vector3(0,0.2f,0), loc2.transform.rotation);
        } else {
            Vector3 seconds = loc2.transform.position;
            seconds.y = 0;
            bruh2 = _map.WorldToGeoPosition(seconds);
        }
        if (tilemap.gameObject.activeSelf)
        {
            if (routine != null) StopCoroutine(routine);
            yield return routine = StartCoroutine(heatManager.DisplayInArea(_map, (float)_map.WorldToGeoPosition(dragNew).x, (float)_map.WorldToGeoPosition(dragNew).y, _map.Zoom));
        } 
        else
        {
            heatManager.init = false;
        }
        yield return new WaitForSeconds(0.2f);
    }

    public void toggleHeatmap()
    {
        if (!(heatManager.init))
        {
            Vector3 dragOld = cameraOrigin;
            dragOld.y = 0;
            //StopCoroutine(routine);
            routine = StartCoroutine(heatManager.DisplayInArea(_map, (float)_map.WorldToGeoPosition(dragOld).x, (float)_map.WorldToGeoPosition(dragOld).y, _map.Zoom));
        }
        heatManager.toggleHeatmap();
        hidebutton1.SetActive(!(hidebutton1.activeSelf));
        hidebutton2.SetActive(!(hidebutton2.activeSelf));
        hidebutton3.SetActive(!(hidebutton3.activeSelf));
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
