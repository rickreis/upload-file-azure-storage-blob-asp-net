using Microsoft.WindowsAzure.Storage.Blob;

namespace UploadFile.Azure.Interfaces
{
    public interface ICloudStorageAccount
    {
        CloudBlobClient CreateCloudBlobClient();
    }
}
