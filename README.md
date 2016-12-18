# FunctionApp201612111

Serverless meetup sapporo demo application

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