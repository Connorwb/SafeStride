namespace Mapbox.Unity.Map
{
	using System.Collections;
	using Mapbox.Unity.Location;
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.Tilemaps;

	public class InitializeMapWithLocationProvider : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		ILocationProvider _locationProvider;

		[SerializeField]
		private Tilemap map;

		[SerializeField]
		private TileBase whiteHex;

		[SerializeField]
		public GameObject offenderDot;

		[SerializeField]
		Button button;

		private HeatmapManager heatManager;

		private void Awake()
		{
			// Prevent double initialization of the map. 
			_map.InitializeOnStart = false;
			heatManager = new HeatmapManager(map, whiteHex, offenderDot, button);
		}

		protected virtual IEnumerator Start()
		{
			yield return null;
			_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
			_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated; ;
		}

		void LocationProvider_OnLocationUpdated(Unity.Location.Location location)
		{
			_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
			_map.Initialize(location.LatitudeLongitude, _map.AbsoluteZoom);
			heatManager.DisplayInArea((float) _locationProvider.CurrentLocation.LatitudeLongitude.x, (float)_locationProvider.CurrentLocation.LatitudeLongitude.y, _map.Zoom);
		}
	}
}
