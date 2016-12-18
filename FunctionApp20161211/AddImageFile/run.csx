#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, Stream outputBlob, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    // parse query parameter
    string filename = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "filename", true) == 0)
        .Value;

    dynamic data = req.Content.ReadAsStringAsync();
    var imgData = JsonConvert.DeserializeObject<filec>(data.Result);

    // Set name to query string or body data
    filename = filename ?? imgData?.name;

    await Write(imgData.file.image as string, outputBlob);

    return req.CreateResponse(HttpStatusCode.OK, "request upload " + filename);
}

static async Task Write(string mimeData, Stream outputBlob)
{
    if (mimeData == null) return;
    var data = System.Convert.FromBase64String(mimeData);
    await outputBlob.WriteAsync(data, 0, data.Length);
}



public class img
{
    public string image { get; set; }
}

public class filec
{
    public img file { get; set; }
    public string name { get; set; }
}