using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Download;
using Google.Apis.Upload;

namespace FeedForward
{
    class GoogleDriveHandler
    {

        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Neural Network Trainer";

        static UserCredential credential;
        static DriveService service;

        public static void GoogleDriveLogin(String credentialsFile)
        {

            using (var stream = new FileStream(credentialsFile, FileMode.Open, FileAccess.Read))
            {
                String credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
                
                
            }

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public static async Task<Boolean> GoogleDriveLogout(String credentialsFile)
        {
            service = null;
            return await credential.RevokeTokenAsync(CancellationToken.None);
        }

        public static IList<Google.Apis.Drive.v3.Data.File> GetFileList()
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            return listRequest.Execute().Files;
        }

        public static String GetFileIdByName(String name)
        {
            IList<Google.Apis.Drive.v3.Data.File> files = GetFileList();

            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Name == name) return file.Id;
                }
            }

            return null;
        }

        public static Stream DownloadGoogleDocument(String fileId, String mime)
        {
            var stream = new MemoryStream();
            var request = service.Files.Export(fileId, mime);
            
            request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download Complete");
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download Failed");
                            break;
                        }
                }
            };

            request.Download(stream);
            return stream;
        }

        public static String DownloadGoogleDocument(String fileId, String mime, Encoding encoding)
        {
            Stream stream = DownloadGoogleDocument(fileId, mime);

            stream.Seek(0, SeekOrigin.Begin);

            using(var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static Stream DownloadDocument(String fileId)
        {
            var stream = new MemoryStream();
            var request = service.Files.Get(fileId);

            request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download Complete");
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download Failed");
                            break;
                        }
                }
            };

            request.Download(stream);
            return stream;
        }

        public static String DownloadDocument(String fileId, String mime, Encoding encoding)
        {
            Stream stream = DownloadGoogleDocument(fileId, mime);

            stream.Seek(0, SeekOrigin.Begin);

            using (var sr = new StreamReader(stream))
            {
                return sr.ReadToEnd();
            }
        }

        public static void UploadGoogleDocument(String content, String name, String uploadMime, String localMime)
        {
            var fileMetaData = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = uploadMime
            };

            FilesResource.CreateMediaUpload request;

            byte[] bytes = Encoding.UTF8.GetBytes(content);

            Stream stream = new MemoryStream(bytes);

            request = service.Files.Create(fileMetaData, stream, localMime);
            request.Fields = "id";

            request.ProgressChanged += (IUploadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case UploadStatus.Uploading:
                        {
                            Console.WriteLine(progress.BytesSent);
                            break;
                        }
                    case UploadStatus.Completed:
                        {
                            Console.WriteLine("Upload Complete");
                            break;
                        }
                    case UploadStatus.Failed:
                        {
                            Console.WriteLine("Upload Failed");

                            throw progress.Exception;
                        }
                }
            };

            request.Upload();

            var file = request.ResponseBody;

            Console.WriteLine("File ID: " + file.Id);
        }

        public static void UploadDocument(String content, String name, String localMime)
        {
            var fileMetaData = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
            };

            FilesResource.CreateMediaUpload request;

            byte[] bytes = Encoding.UTF8.GetBytes(content);

            Stream stream = new MemoryStream(bytes);

            request = service.Files.Create(fileMetaData, stream, localMime);
            request.Fields = "id";

            request.ProgressChanged += (IUploadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case UploadStatus.Uploading:
                        {
                            Console.WriteLine(progress.BytesSent);
                            break;
                        }
                    case UploadStatus.Completed:
                        {
                            Console.WriteLine("Upload Complete");
                            break;
                        }
                    case UploadStatus.Failed:
                        {
                            Console.WriteLine("Upload Failed");
                            break;
                        }
                }
            };

            request.Upload();

            var file = request.ResponseBody;

            Console.WriteLine("File ID: " + file.Id);
        }


    }
}
