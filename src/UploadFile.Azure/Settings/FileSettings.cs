using System;
using System.Configuration;

namespace UploadFile.Azure
{
    public static class FileSettings
    {
        public static string Path
        {
            get { return ConfigurationManager.AppSettings["FileSettings.Path"] ?? String.Empty; }
        }

        public static string UrlFile
        {
            get { return ConfigurationManager.AppSettings["FileSettings.UrlFile"] ?? String.Empty; }
        }

        public static string StorageConnectionString
        {
            get { return ConfigurationManager.AppSettings["FileSettings.StorageConnectionString"] ?? String.Empty; }
        }
    }
}
