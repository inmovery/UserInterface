using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UserInterfaceTest.Helpers;
using UserInterfaceTest.Models.Interfaces;
using UserInterfaceTest.ViewModels;

namespace UserInterfaceTest.Models
{
    public class FileDownloader : BaseViewModel, IFileDownloader
    {
        private CancellationTokenSource _cancelationTokenSource;

        public async Task DownloadImage(WebClient webClient, string Url)
        {
            _cancelationTokenSource = new CancellationTokenSource();
            var imageBytes = new byte[0];

            var cancellationToken = _cancelationTokenSource.Token;
            cancellationToken.Register(() => webClient.CancelAsync());
            webClient.DownloadDataAsync(new Uri(Url));
        }

        public void CancelDownload()
        {
            if (_cancelationTokenSource is null)
                throw new InvalidOperationException("Загрузка не была запущена");
            _cancelationTokenSource.Cancel();
        }

    }
}
