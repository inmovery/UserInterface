using System;
using System.Net;
using System.Threading.Tasks;

namespace UserInterfaceTest.Models.Interfaces
{
    public interface IFileDownloader
    {
        void CancelDownload();
        Task DownloadImage(WebClient webClient, string Url);
    }
}
