using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UserInterfaceTest.Helpers
{
    public static class WebClientHelper
    {
        public static async Task<byte[]> DownloadDataWithProgressAsync(this WebClient webClient, string uri, IProgress<double> progress, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (progress is null)
                throw new ArgumentNullException(nameof(progress));

            cancellationToken.Register(() => webClient.CancelAsync());
            webClient.DownloadProgressChanged += (s, e) => progress.Report(e.ProgressPercentage);
            return await webClient.DownloadDataTaskAsync(uri);
        }
    }
}
