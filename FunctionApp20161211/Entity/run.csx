// Setup
// 1) Go to https://www.microsoft.com/cognitive-services/en-us/computer-vision-api 
//    Sign up for computer vision api
// 2) Go to Function app settings -> App Service settings -> Settings -> Application settings
//    create a new app setting Vision_API_Subscription_Key and use Computer vision key as value
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;


public class ImageData
{
    public List<Face> Faces { get; set; }

    public string storeGuid;
}

public class Face
{
    public int Age { get; set; }

    public string Gender { get; set; }

    public FaceRectangle FaceRectangle { get; set; }
}

public class FaceRectangle : TableEntity
{
    public string ImageFile { get; set; }

    public int Left { get; set; }

    public int Top { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
}