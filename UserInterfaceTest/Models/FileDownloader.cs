using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UserInterfaceTest.Helpers;
using UserInterfaceTest.Models.Interfaces;

namespace UserInterfaceTest.Models
{
    public class FileDownloader : IFileDownloader
    {
        private CancellationTokenSource _cancelationTokenSource;

        public async Task<byte[]> Download(string Url, Action<double> progress)
        {
            try
            {
                _cancelationTokenSource = new CancellationTokenSource();
                var bytes = new byte[0];
                using (var webClient = new WebClient())
                {
                    bytes = await webClient.DownloadDataWithProgressAsync(Url, new Progress<double>(progress), _cancelationTokenSource.Token);
                }
                return bytes;
            }
            catch (WebException e)
            {
                if (!_cancelationTokenSource.IsCancellationRequested)
                    throw;
            }
            return new byte[0];
        }

        public void CancelDownload()
        {
            if (_cancelationTokenSource is null)
                throw new InvalidOperationException("Загрузка не была запущена");
            _cancelationTokenSource.Cancel();
        }

    }
}
