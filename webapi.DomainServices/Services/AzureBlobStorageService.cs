using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using webapi.DomainServices.Interfaces;

namespace webapi.DomainServices.Services
{
    public class AzureBlobStorageService : IAzureBlobStorageService
    {
        private BlobContainerClient _blobContainerClient;
        private BlobServiceClient _blobServiceClient;
        public string _connectionString { get; internal set; }
        public List<string> _uploadedFiles { get; internal set; } = new List<string>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connectionString">Storage account connection string</param>
        /// <exception cref="ArgumentException"></exception>
        public AzureBlobStorageService(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) return;
            _connectionString = connectionString;
            try
            {
                _blobServiceClient = new BlobServiceClient(connectionString);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Change the current container before performing any other operations
        /// </summary>
        /// <param name="containerName">Container name</param>
        /// <param name="connectionString">[Optional] Storage account connection string</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetContainer(string containerName, string connectionString = "")
        {
            if (!string.IsNullOrEmpty(connectionString)) _connectionString = connectionString;
            try
            {
                _blobServiceClient = new BlobServiceClient(_connectionString);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }

            var existingContainer = _blobServiceClient.GetBlobContainers(BlobContainerTraits.Metadata, prefix: containerName).FirstOrDefault();
            if (existingContainer == null)
            {
                _blobServiceClient.CreateBlobContainer(containerName, publicAccessType: PublicAccessType.BlobContainer);
            }

            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        }

        /// <summary>
        /// Uploads files on disk to storage blob with a unique GUID name and same extension as the file
        /// </summary>
        /// <param name="files">path of files to upload</param>
        /// <returns>Dictionary of {file-name, file-url}</returns>
        public async Task<Dictionary<string, string>> UploadFilesAsync(string[] files, string storageFolderName = "")
        {
            var fileUrls = new Dictionary<string, string>();
            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                if (!string.IsNullOrEmpty(storageFolderName))
                {
                    fileName = Path.Combine(storageFolderName, fileName);
                }

                using FileStream uploadFileStream = File.OpenRead(file);
                var uploadedFileName = await UploadFileAsync(fileName, uploadFileStream);

                Console.WriteLine($"Successfully uploaded file: {file}");

                fileUrls.Add(Path.GetFileName(file), uploadedFileName);
                this._uploadedFiles.Add(uploadedFileName);
            }

            return fileUrls;
        }

        /// <summary>
        ///  Uploads file on disk to storage blob with a unique GUID name and same extension as the file
        /// </summary>
        /// <param name="localFilePath">absolute file path</param>
        /// <param name="blobName">Blob name</param>
        /// <returns></returns>
        public async Task UploadLocalFileAsync(string localFilePath, string blobName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(localFilePath, overwrite: true);
        }

        /// <summary>
        /// Uploads files on disk to storage blob with a unique GUID name and same extension as the file
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="stream">Contant to upload</param>
        /// <param name="contentType">MIME type for current blob</param>
        /// <param name="absoluteUrl">Return blob absolute URI</param>
        /// <param name="closeStream">Close the current stream before exiting</param>
        /// <returns>Blob's name(uri after uploading it to current container</returns>
        public async Task<string> UploadFileAsync(string fileName, Stream stream, string contentType = null, bool absoluteUrl = false, bool closeStream = true)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            var blob = await blobClient.UploadAsync(stream, overwrite: true);

            if (!string.IsNullOrEmpty(contentType))
            {
                blobClient.SetHttpHeaders(new BlobHttpHeaders { ContentType = contentType });
            }

            if (closeStream)
            {
                stream.Dispose();
            }

            if (absoluteUrl)
            {
                return blobClient.Uri.AbsoluteUri;
            }

            return blobClient.Name;
        }

        /// <summary>
        /// Downloads files from storage blob and returns a mapping of file name and download stream
        /// </summary>
        /// <param name="fileNameUrlMaps">Dictionary of {file-name, file-url}</param>
        /// <returns>Dictionary of {file-name, download-stream}</returns>
        public async Task<Dictionary<string, Stream>> DownloadFilesAsync(Dictionary<string, string> fileNameUrlMaps)
        {
            var fileDownloadMap = new Dictionary<string, Stream>();
            foreach (var fileMap in fileNameUrlMaps)
            {
                fileDownloadMap.Add(fileMap.Key, await DownloadFileAsync(fileMap.Value));
            }

            return fileDownloadMap;
        }

        /// <summary>
        /// Downloads a blob (file) from current container
        /// </summary>
        /// <param name="fileName">blob name</param>
        /// <returns>Blob content</returns>
        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            BlobDownloadInfo fileDownload = await blobClient.DownloadAsync();
            return fileDownload.Content;
        }

        /// <summary>
        /// Downloads a complete Blob including metadata information and writes it to disk path
        /// </summary>
        /// <param name="blobName">blob name</param>
        ///  <param name="blobName">local path to store the blob</param>
        /// <returns>Blob content</returns>
        public async Task DownloadFileAsync(string fileName, string filePath)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DownloadToAsync(filePath);
        }

        /// <summary>
        /// Marks the specified files within blob for deletion
        /// </summary>
        /// <param name="fileUrls">List with the file names with extension to delete from current container</param>
        public async Task DeleteFilesAsync(List<string> fileNames)
        {
            foreach (var fileUrl in fileNames)
            {
                var blobClient = _blobContainerClient.GetBlobClient(fileUrl);
                await blobClient.DeleteAsync();
            }
        }
    }
}
