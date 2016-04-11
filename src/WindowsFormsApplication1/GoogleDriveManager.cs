using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Google.GData.Client;
//using Google.GData.Spreadsheets;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
//using Google.Apis.Requests;
using Google.Apis.Services;
//using Google.Apis.Util.Store;

namespace WindowsFormsApplication1
{
    public sealed class GoogleDriveManager
    {
        //for singleton
        private static GoogleDriveManager _sigleInstance = new GoogleDriveManager();
        public static GoogleDriveManager Instance
        {
            get
            {
                return _sigleInstance;
            }
        }
        private GoogleDriveManager()
        {
            m_service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleAuthorizationManager.userCredential,
                ApplicationName = "SolidEdgePartsManager",
            });
        }

        private DriveService m_service;

        public bool uploadFile(File body, System.IO.MemoryStream stream)
        {
            try
            {
                FilesResource.InsertMediaUpload request = m_service.Files.Insert(body, stream, "text/plain");
                request.Upload();

                File file = request.ResponseBody;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CSV file upload error.\n" + ex.Message);
                return false;
            }
        }

        public IList<Google.Apis.Drive.v2.Data.File> getChildFolder(String parentId = "root")
        {
            Console.WriteLine("GoogleDriveManager.getChildFolder(" + parentId + ")");
            FilesResource.ListRequest request = m_service.Files.List();
            request.Q = "mimeType = 'application/vnd.google-apps.folder' and '" + parentId + "' in parents and trashed = false"
                + "or mimeType = 'application/vnd.google-apps.spreadsheet' and '" + parentId + "' in parents and trashed = false";
            IList<Google.Apis.Drive.v2.Data.File> files = request.Execute().Items;
            return files;
        }
    }
}
