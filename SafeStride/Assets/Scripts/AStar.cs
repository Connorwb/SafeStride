//Common testing requirement. If you are consuming an API in a sandbox/test region, uncomment this line of code ONLY for non production uses.
//System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

//Be sure to run "Install-Package Microsoft.Net.Http" from your nuget command line.
using System;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

var baseAddress = new Uri("https://api.openrouteservice.org");

using (var httpClient = new HttpClient{ BaseAddress = baseAddress })
{
  httpClient.DefaultRequestHeaders.Clear();
  httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");
  httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
  httpClient.DefaultRequestHeaders.Authorization =
      new AuthenticationHeaderValue("Bearer", "5b3ce3597851110001cf624875fb6393d1e24539ab9da6760164e9d3");
    
    // quotes might have to be escaped
    using (var content = new StringContent("{:[[8.681495,49.41461],[8.686507,49.41943],[8.687872,49.420318]]}"))
    {
      using (var response = await httpClient.PostAsync("/v2/directions/foot-walking/json", content))
      {
        string responseData = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject(responseData);
      }
  }
}

public class Astar : MonoBehavior
{
  private const string URL = "https://api.openrouteservice.org/v2/directions/foot-walking/json";
  private const string HOST = "https://api.openrouteservice.org";
    private const string API_KEY = "5b3ce3597851110001cf624875fb6393d1e24539ab9da6760164e9d3";
    public void GenerateRequest()
    {
        StartCoroutine(ProcessRequest(URL));
    }
    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("", HOST);
            request.SetRequestHeader("", API_KEY);

            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Received: " + request.downloadHandler.text);
            }
        }
    }
}

//https://api.openrouteservice.org/v2/directions/foot-walking?api_key=your-api-key&start=8.681495,49.41461&end=8.687872,49.420318


/*

// UnityWebRequest.Get example

// Access a website and use UnityWebRequest.Get to download a page.
// Also try to download a non-existing page. Display the error.

public class Example : MonoBehaviour
{
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://www.example.com"));

        // A non-existing page.
        StartCoroutine(GetRequest("https://error.html"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
*/