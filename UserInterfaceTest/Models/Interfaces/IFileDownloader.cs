using System;
using System.Threading.Tasks;

namespace UserInterfaceTest.Models.Interfaces
{
    public interface IFileDownloader
    {
        void CancelDownload();
        Task<byte[]> Download(string Url, Action<double> progress);
    }
}
