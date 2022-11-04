//Common testing requirement. If you are consuming an API in a sandbox/test region, uncomment this line of code ONLY for non production uses.
//System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

//Be sure to run "Install-Package Microsoft.Net.Http" from your nuget command line.
/*
using System;
using System.Net.Http;

var baseAddress = new Uri("https://api.openrouteservice.org/v2/directions/foot-walking?api_key=your-api-key&start=8.681495,49.41461&end=8.687872,49.420318");

using (var httpClient = new HttpClient{ BaseAddress = baseAddress })
{
  httpClient.DefaultRequestHeaders.Clear();
  httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json, application/geo+json, application/gpx+xml, img/png; charset=utf-8");

  using(var response = await httpClient.getAsync("directions"))
  {
    string responseData = await response.Content.ReadAsStringAsync();
    var data = JsonConvert.DeserializeObject(responseData);
  }
}
*/