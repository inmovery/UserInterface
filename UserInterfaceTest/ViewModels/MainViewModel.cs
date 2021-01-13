using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using UserInterfaceTest.Commands;
using UserInterfaceTest.Enums;
using UserInterfaceTest.Helpers;
using UserInterfaceTest.Models;
using UserInterfaceTest.Views;

namespace UserInterfaceTest.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Private Members

        private TotalDownloadingProgress totalDownloadingProgress;
        private int CountImageDownloaders = 3;
        
        private ICommand downloadAllCommand;
        
        /// <summary>
        /// The Window this view model controls
        /// </summary>
        private Window windowInstance;
        
        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private int outerMarginSize = 3;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int windowRadius = 0;

        #endregion Private Members

        #region Public Members [INPC]

        public ObservableCollection<ImageDownloaderViewModel> ImageDownloaders { get; set; }

        public ICollectionView ImageDownloadersView { get; set; }

        public TotalDownloadingProgress TotalDownloadingProgress
        {
            get
            {
                return totalDownloadingProgress;
            }
            set
            {
                totalDownloadingProgress = value;
                RaisePropertyChanged();
            }
        }

        public long MaximumProgressBar
        {
            get
            {
                return (long)ImageDownloaders.Sum(x => x?.TotalBytesToReceive);
            }
        }

        private long GetTotalBytesReceive(string url)
        {
            try
            {
                long result = 0;

                WebRequest req = WebRequest.Create(url);
                req.Method = "HEAD";
                using (WebResponse resp = req.GetResponse())
                {
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out long contentLength))
                    {
                        result = contentLength;
                    }
                }

                return result;
            }
            catch
            {
                return 0;
            }
        }

        #endregion Public Members [INPC]

        #region Window Toolbar

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder { get; set; } = 6;

        /// <summary>
        /// The size of the resize border, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness
        {
            get
            {
                return new Thickness(ResizeBorder + OuterMarginSize);
            }
        }

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding
        {
            get
            {
                return new Thickness(ResizeBorder);
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public int OuterMarginSize
        {
            get
            {
                return windowInstance.WindowState == WindowState.Maximized ? 0 : outerMarginSize;
            }
            set
            {
                outerMarginSize = value;
            }
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        public Thickness OuterMarginSizeThickness
        {
            get
            {
                return new Thickness(OuterMarginSize);
            }
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public int WindowRadius
        {
            get
            {
                return windowInstance.WindowState == WindowState.Maximized ? 0 : windowRadius;
            }
            set
            {
                windowRadius = value;
            }
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius
        {
            get
            {
                return new CornerRadius(WindowRadius);
            }
        }

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public int TitleHeight { get; set; } = 35;

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength
        {
            get
            {
                return new GridLength(TitleHeight + ResizeBorder);
            }
        }

        #endregion

        #region Commands for Controls

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        #endregion

        #region Commands

        public ICommand DownloadAllCommand
        {
            get
            {
                if (downloadAllCommand == null)
                {
                    bool check = false;
                    foreach(var vm in ImageDownloaders)
                        check = check || string.IsNullOrEmpty(vm.Url);
                    downloadAllCommand = new AsyncRelayCommand(DownloadAllAsync, () => check);
                }
                return downloadAllCommand;
            }
        }

        #endregion Commands

        #region Methods

        public async Task DownloadAllAsync()
        {
            ImageDownloaders.Where(x => x.TotalBytesToReceive == 0).AsParallel().ForAll(x => x.TotalBytesToReceive = GetTotalBytesReceive(x?.Url));
            ImageDownloaders.AsParallel().ForAll(x => x.DownloadingState = DownloadingState.Downloading);

            List<Task> tasks = new List<Task>();
            foreach (var vm in ImageDownloaders)
            {
                tasks.Add(vm.StartDownloadAsync("All"));
            }
            await Task.WhenAll(tasks);

            ImageDownloaders.AsParallel().ForAll(x => x.DownloadingState = DownloadingState.Completed);
        }

        /// <summary>
        /// Handle closing window and saving data
        /// </summary>
        private void CloseWindow()
        {
            // TODO: implement this like smth else
            windowInstance.Close();
        }

        #endregion

        #region Constructors

        public MainViewModel(Window window)
        {
            windowInstance = window;

            // Listen out for the window resizing
            windowInstance.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            // Create commands for controls
            MinimizeCommand = new RelayCommand(() => windowInstance.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => windowInstance.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(CloseWindow);

            List<ImageDownloaderViewModel> cotrols = new List<ImageDownloaderViewModel>();
            for (int i = 0; i < CountImageDownloaders; i++)
            {
                ImageDownloaderViewModel vm = new ImageDownloaderViewModel();
                vm.PropertyChanged += ImageDownloaders_PropertyChanged;
                cotrols.Add(vm);
            }

            ImageDownloaders = new ObservableCollection<ImageDownloaderViewModel>(cotrols);
            BindingOperations.EnableCollectionSynchronization(ImageDownloaders, new object());
            ImageDownloadersView = CollectionViewSource.GetDefaultView(ImageDownloaders);

            TotalDownloadingProgress = new TotalDownloadingProgress();
            TotalDownloadingProgress.PropertyChanged += TotalDownloadingProgress_PropertyChanged;
        }

        public MainViewModel(params ImageDownloaderViewModel[] models)
        {
            for(int i = 0; i < models.Length; i++)
            {
                ImageDownloaders[i] = models[i];
            }

            TotalDownloadingProgress = new TotalDownloadingProgress();
        }

        #endregion Constructors

        #region PropertyChange Methods

        private void ImageDownloaders_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DownloadingProgress":
                    long scale = MaximumProgressBar == 0 ? 1 : (MaximumProgressBar / 100);
                    TotalDownloadingProgress.TotalDownloadingProgressValue = ImageDownloaders.Sum(x => x.CurrentBytesReceived) / scale;
                    RaisePropertyChanged(nameof(TotalDownloadingProgress));
                    break;
            }
        }

        private async void TotalDownloadingProgress_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalDownloadingProgressValue")
            {
                if (((TotalDownloadingProgress)sender).TotalDownloadingProgressValue == MaximumProgressBar)
                {
                    TotalDownloadingProgress.TotalDownloadingProgressValue = -1;
                }
            }
        }

        #endregion PropertyChange Methods

    }
}
