// Setup
// 1) Go to https://www.microsoft.com/cognitive-services/en-us/computer-vision-api 
//    Sign up for computer vision api
// 2) Go to Function app settings -> App Service settings -> Settings -> Application settings
//    create a new app setting Vision_API_Subscription_Key and use Computer vision key as value
#r "Microsoft.WindowsAzure.Storage"
#r "Newtonsoft.Json"
#r "System.Drawing"

using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.IO;

public static async Task Run(Stream image, string name, IAsyncCollector<FaceRectangle> outTable, Stream outputBlob, TraceWriter log)
{
    var memory = new System.IO.MemoryStream();
    await image.CopyToAsync(memory);
    memory.Seek(0, SeekOrigin.Begin);
    image.Seek(0, SeekOrigin.Begin);

    string result = await CallVisionAPI(image);
    log.Info(result);

    if (String.IsNullOrEmpty(result))
    {
        return;
    }

    ImageData imageData = JsonConvert.DeserializeObject<ImageData>(result);
    foreach (Face face in imageData.Faces)
    {
        var faceRectangle = face.FaceRectangle;
        faceRectangle.RowKey = Guid.NewGuid().ToString();
        faceRectangle.PartitionKey = "Functions";
        faceRectangle.ImageFile = name;
        await outTable.AddAsync(faceRectangle);
    }
    var resultImage = GetImage(memory, imageData.Faces);
    resultImage.Save(outputBlob, System.Drawing.Imaging.ImageFormat.Jpeg);
    await outputBlob.FlushAsync();
    outputBlob.Close();
}

static async Task<string> CallVisionAPI(Stream image)
{
    using (var client = new HttpClient())
    {
        var content = new StreamContent(image);
        var url = "https://api.projectoxford.ai/vision/v1.0/analyze?visualFeatures=Faces";
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("Vision_API_Subscription_Key"));
        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        var httpResponse = await client.PostAsync(url, content);

        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
    return null;
}

static System.Drawing.Image GetImage(Stream imageStream, List<Face> rec)
{
    imageStream.Seek(0, SeekOrigin.Begin);
    var image = System.Drawing.Bitmap.FromStream(imageStream);

    var rotation = RotateFlipType.RotateNoneFlipNone;
    foreach (var item in image.PropertyItems)
    {
        if (item.Id != 0x0112)
            continue;
        switch (item.Value[0])
        {
            case 3:
                rotation = RotateFlipType.Rotate180FlipNone;
                break;
            case 6:
                rotation = RotateFlipType.Rotate90FlipNone;
                break;
            case 8:
                rotation = RotateFlipType.Rotate270FlipNone;
                break;
        }
    }
    image.RotateFlip(rotation);

    using (var g = System.Drawing.Graphics.FromImage(image))
    {
        System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.Azure, 3);
        foreach (var fc in rec)
        {
            var f = fc.FaceRectangle;
            var rect = new System.Drawing.Rectangle(f.Left, f.Top, f.Width, f.Height);
            g.DrawRectangle(p, rect);
            var font = System.Drawing.SystemFonts.DefaultFont;
            font = new System.Drawing.Font(font.FontFamily, 16);
            g.DrawString($"{fc.Age.ToString()} - {fc.Gender}", font, System.Drawing.Brushes.Azure, f.Left, f.Top);
        }
        g.Flush();
        g.Save();
    }

    return image;

}


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