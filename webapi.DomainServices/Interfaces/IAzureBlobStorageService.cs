using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.DomainServices.Interfaces
{
    public interface IAzureBlobStorageService
    {
        List<string> _uploadedFiles { get; }
        Task<Dictionary<string, string>> UploadFilesAsync(string[] files, string storageFolderName = "");
        Task<Dictionary<string, Stream>> DownloadFilesAsync(Dictionary<string, string> fileNameUrlMaps);
        Task<Stream> DownloadFileAsync(string fileName);
        Task DownloadFileAsync(string fileName, string filePath);
        Task<string> UploadFileAsync(string fileName, Stream stream, string contentType = null, bool absoluteUrl = false, bool closeStream = true);
        Task UploadLocalFileAsync(string localFilePath, string fileName);
        Task DeleteFilesAsync(List<string> fileUrls);
        void SetContainer(string containerName, string connectionString = "");
    }
}
