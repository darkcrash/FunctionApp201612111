# FunctionApp201612111

Serverless meetup sapporo demo application

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fgithub.com%2Fdarkcrash%2FFunctionApp201612111%2Fraw%2Fmaster%2FAzureResourceGroup20161211%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fgithub.com%2Fdarkcrash%2FFunctionApp201612111%2Fraw%2Fmaster%2FAzureResourceGroup20161211%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


## Azure Functions

* AddImageFile
 * Trigger - HttpReqest
 * Input - HttpRequest
 * Output - Azure Storage Blob

* FaceLocatorCSharp
 * Trigger - Azure Storage Blob
 * Input - Azure Storage Blob
 * Output - Azure Storage Blob
 * Output - Azure Storage Table

## Web Application

* Controllers
 * Home Controller
  * Index Action - ~/
  * AddImageFile Action - ~/AddImageFile
  * ImageList Action - ~/ImageList

## 注意
現在、azure-functions-cli で実行するローカルホストは、最大要求サイズが65500バイトに制限されているため
そのサイズを超えるイメージをアップロードすることができません。