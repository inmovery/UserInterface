using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UserInterfaceTest.Commands;
using UserInterfaceTest.Enums;
using UserInterfaceTest.Models;
using UserInterfaceTest.Helpers;

namespace UserInterfaceTest.ViewModels
{
    public class ImageDownloaderViewModel : BaseViewModel
    {

        #region Private Members

        private FileDownloader fileDownloader;
        private BitmapImage sourceImage;
        private double downloadingProgress;
        private string url;
        private DownloadingState downloadingState;
        private ICommand startDownloadCommand;
        private ICommand stopDownloadCommand;
        private double currentBytes;
        private WebClient webClient;
        private long totalBytesToReceive;
        private long currentBytesReceived;

        private bool isDownloading;

        #endregion Private Members

        #region Public Members [INPC]

        public BitmapImage SourceImage
        {
            get
            {
                return sourceImage;
            }
            set
            {
                sourceImage = value;
                RaisePropertyChanged();
            }
        }

        public double DownloadingProgress
        {
            get
            {
                return downloadingProgress;
            }
            set
            {
                downloadingProgress = value;
                RaisePropertyChanged();
            }
        }

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
                RaisePropertyChanged();
            }
        }

        public DownloadingState DownloadingState
        {
            get
            {
                return downloadingState;
            }
            set
            {
                downloadingState = value;
                RaisePropertyChanged();
            }
        }

        public bool IsDownloading
        {
            get
            {
                return isDownloading;
            }
            set
            {
                isDownloading = value;
                RaisePropertyChanged();
            }
        }

        public double CurrentBytes
        {
            get
            {
                return currentBytes;
            }
            set
            {
                currentBytes = value;
                RaisePropertyChanged();
            }
        }

        public long TotalBytesToReceive
        {
            get
            {
                return totalBytesToReceive;
            }
            set
            {
                totalBytesToReceive = value;
                RaisePropertyChanged();
            }
        }

        public long CurrentBytesReceived
        {
            get
            {
                return currentBytesReceived;
            }
            set
            {
                currentBytesReceived = value;
                RaisePropertyChanged();
            }
        }

        #endregion Public Members [INPC]

        #region Commands

        public ICommand StartDownloadCommand
        {
            get
            {
                if (startDownloadCommand == null)
                    startDownloadCommand = new AsyncRelayCommand(() => StartDownloadAsync("Default"), () => DownloadingState != DownloadingState.Downloading && !string.IsNullOrEmpty(Url));
                return startDownloadCommand;
            }
        }

        public ICommand StopDownloadCommand
        {
            get
            {
                if (stopDownloadCommand == null)
                    stopDownloadCommand = new RelayCommand(StopDownload, () => DownloadingState == DownloadingState.Downloading);
                return stopDownloadCommand;
            }
        }

        #endregion Commands

        #region Methods

        public async Task StartDownloadAsync(string setting)
        {
            try
            {
                if (setting != "All")
                    DownloadingState = DownloadingState.Downloading;
                SourceImage = null;
                await fileDownloader.DownloadImage(webClient, Url);
            }
            catch(WebException ex)
            {
                MessageBox.Show(ex.Message);
                // TODO: create exception dialog
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
                // TODO: create exception dialog with words relates to "Enter Url"
            }
            finally
            {
                if (setting != "All")
                    DownloadingState = DownloadingState.Completed;
            }
        }



        public void StopDownload()
        {
            fileDownloader.CancelDownload();
            DownloadingState = DownloadingState.Completed;
            DownloadingProgress = 0.0;
        }

        public void ClearState()
        {
            DownloadingState = DownloadingState.Idle;
            DownloadingProgress = 0.0;
        }

        #endregion Methods

        #region Constructor

        public ImageDownloaderViewModel()
        {
            CurrentBytesReceived = 0;
            TotalBytesToReceive = 0;
            SourceImage = null;
            DownloadingState = DownloadingState.Idle;
            fileDownloader = new FileDownloader();
            webClient = new WebClient();

            webClient.DownloadDataCompleted += (sender, completedEvent) =>
            {
                if (!completedEvent.Cancelled)
                {
                    SourceImage = Converter.FromBytesToImage(completedEvent.Result);
                    DownloadingState = DownloadingState.Completed;
                    //CurrentBytesReceived = 0;
                    //TotalBytesToReceive = 0;
                }
            };

            webClient.DownloadProgressChanged += (sender, changedEvent) =>
            {
                DownloadingState = DownloadingState.Downloading;
                DownloadingProgress = changedEvent.ProgressPercentage;
                TotalBytesToReceive = changedEvent.TotalBytesToReceive;
                CurrentBytesReceived = changedEvent.BytesReceived;
            };

        }

        #endregion Constructor

    }
}
