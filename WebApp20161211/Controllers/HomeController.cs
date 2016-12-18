using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp20161211.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImageUpload()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.ApiHostName = System.Configuration.ConfigurationManager.AppSettings.Get("ApiHostName");
            return View();
        }

        public ActionResult AddImageFile()
        {
            //var file = this.Request.Form["file"];

            return View("ImageUpload");
        }

        public ActionResult ImageList()
        {
            var list = new List<string>();
            var storageKey = System.Configuration.ConfigurationManager.AppSettings.Get("StorageKey");

            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageKey);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("faceimages");
            container.CreateIfNotExists();
            var perm = new BlobContainerPermissions();
            perm.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(perm);

            var query = container.ListBlobs(null, false).Where(_ => _.GetType() == typeof(CloudBlockBlob)).Select(_ => (CloudBlockBlob)_);
            query = query.OrderByDescending(_ => _.SnapshotTime);

            // Loop over items within the container and output the length and URI.
            foreach (CloudBlockBlob blob in query)
            {
                Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                list.Add(blob.Uri.ToString());

            }
            ViewBag.BlobList = list;

            return View();
        }


    }
}