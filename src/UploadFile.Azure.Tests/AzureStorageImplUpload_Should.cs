using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Linq;
using UploadFile.Azure.Interfaces;

namespace UploadFile.Azure.Tests
{
    /// <summary>
    /// Need to run windows azure emulator
    /// </summary>
    [TestClass]
    public class AzureStorageImplUpload_Should
    {
        [TestMethod]
        public void Result_error_when_file_stream_invalid()
        {
            //arrange
            AzureStorageImpl azure = new AzureStorageImpl();

            //act
            Result result = azure.Upload(null);

            //assert
            Assert.AreEqual("fileStream invalid", result.Errors.FirstOrDefault());
        }

        [TestMethod, DeploymentItem("files\\test-file.txt")]
        public void Result_error_when_blob_client_invalid()
        {
            //arrange
            AzureStorageImpl azure = new AzureStorageImpl(MockCloudStorageAccountBlobClientNull());

            //act
            using (var fileStream = File.OpenRead("test-file.txt"))
            {
                Result result = azure.Upload(fileStream);

                //assert
                Assert.AreEqual("blobClient invalid", result.Errors.FirstOrDefault());
            }

        }

        [TestMethod, DeploymentItem("files\\test-file.txt")]
        public void Result_successfully_upload_file_blob_client()
        {
            //arrange
            AzureStorageImpl azure = new AzureStorageImpl();

            //act
            using (var fileStream = File.OpenRead("test-file.txt"))
            {
                ResultUploadFile result = azure.Upload(fileStream);

                //assert
                Assert.AreEqual("http://127.0.0.1:10000/devstoreaccount1/files/test-file.txt", result.Url);
            }            
        }

        ICloudStorageAccount MockCloudStorageAccountBlobClientNull()
        {
            Mock<ICloudStorageAccount> mock = new Mock<ICloudStorageAccount>();

            mock.Setup(x => x.CreateCloudBlobClient()).Returns(() => { return null; });

            return mock.Object;
        }
    }
}
