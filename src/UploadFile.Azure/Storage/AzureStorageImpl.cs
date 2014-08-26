using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using UploadFile.Azure.Interfaces;

namespace UploadFile.Azure
{
    public class AzureStorageImpl
    {
        CloudStorageAccount _account;
        CloudBlobClient _blobClient;

        public AzureStorageImpl()
        {         
            _account = CloudStorageAccount.Parse(FileSettings.StorageConnectionString);
            _blobClient = _account.CreateCloudBlobClient();
        }

        public AzureStorageImpl(ICloudStorageAccount account)
        {            
            _blobClient = account.CreateCloudBlobClient();
        }

        public ResultUploadFile Upload(FileStream fileStream)
        {
            ResultUploadFile result = new ResultUploadFile();

            if (null == fileStream)
            {
                result.AddError("fileStream invalid");
                return result;
            }            

            try
            {                   
                CloudBlobContainer container = GetContainer(FileSettings.Path, true);

                result.BlobName = Path.GetFileName(fileStream.Name);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(result.BlobName);

                fileStream.Seek(0, SeekOrigin.Begin);

                blockBlob.UploadFromStream(fileStream);                
            }
            catch (Exception ex)
            {
                //TODO: log error  
                result.AddError(ex.Message);                
            }

            return result;
        }

        public ResultDeleteFile Delete(string fileName)
        {
            ResultDeleteFile result = new ResultDeleteFile();            

            if (String.IsNullOrWhiteSpace(fileName))
            {
                result.AddError("fileName invalid");

                return result;
            }

            try
            {
                CloudBlobContainer container = GetContainer(FileSettings.Path);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                blockBlob.Delete();                
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);                  
            }

            return result;
        }

        CloudBlobContainer GetContainer(string containerName, bool createIfNotExists = false)
        {
            if (null == _blobClient)
            {
                throw new ArgumentNullException("blobClient invalid", new Exception());
            }

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);

            if (createIfNotExists)
            {
                // Create the container if it doesn't already exist and set public.
                container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
            }

            return container;
        }
    }
}