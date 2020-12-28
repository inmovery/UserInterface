﻿using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UserInterfaceTest.Commands;
using UserInterfaceTest.Enums;
using UserInterfaceTest.Models;

namespace UserInterfaceTest.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        #region Private Members

        private ImageDownloaderViewModel firstImageDownloaderViewModel;
        private ImageDownloaderViewModel secondImageDownloaderViewModel;
        private ImageDownloaderViewModel thirdImageDownloaderViewModel;

        private TotalDownloadingProgress totalDownloadingProgress;
        private int countActiveDownloading;

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

        public ImageDownloaderViewModel FirstImageDownloaderViewModel
        {
            get
            {
                return firstImageDownloaderViewModel;
            }
            set
            {
                firstImageDownloaderViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ImageDownloaderViewModel SecondImageDownloaderViewModel
        {
            get
            {
                return secondImageDownloaderViewModel;
            }
            set
            {
                secondImageDownloaderViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ImageDownloaderViewModel ThirdImageDownloaderViewModel
        {
            get
            {
                return thirdImageDownloaderViewModel;
            }
            set
            {
                thirdImageDownloaderViewModel = value;
                RaisePropertyChanged();
            }
        }

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

        public int CountActiveDownloading
        {
            get
            {
                return countActiveDownloading;
            }
            set
            {
                countActiveDownloading = value;
                RaisePropertyChanged();
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
                    downloadAllCommand = new AsyncRelayCommand(DownloadAllAsync,
                        () => !(string.IsNullOrEmpty(FirstImageDownloaderViewModel.Url) ||
                                string.IsNullOrEmpty(SecondImageDownloaderViewModel.Url) ||
                                string.IsNullOrEmpty(ThirdImageDownloaderViewModel.Url)));
                return downloadAllCommand;
            }
        }

        #endregion Commands

        #region Methods

        public async Task DownloadAllAsync()
        {
            CountActiveDownloading = 0;

            FirstImageDownloaderViewModel.DownloadingState = DownloadingState.Downloading;
            SecondImageDownloaderViewModel.DownloadingState = DownloadingState.Downloading;
            ThirdImageDownloaderViewModel.DownloadingState = DownloadingState.Downloading;

            await Task.WhenAll(
                FirstImageDownloaderViewModel.StartDownloadAsync("All"),
                SecondImageDownloaderViewModel.StartDownloadAsync("All"),
                ThirdImageDownloaderViewModel.StartDownloadAsync("All")
            );

            FirstImageDownloaderViewModel.DownloadingState = DownloadingState.Completed;
            SecondImageDownloaderViewModel.DownloadingState = DownloadingState.Completed;
            ThirdImageDownloaderViewModel.DownloadingState = DownloadingState.Completed;

            CountActiveDownloading = 0;
        }

        /// <summary>
        /// Clearing ProgressBar values
        /// </summary>
        public async void ApplyCompletedState()
        {
            CountActiveDownloading = 0;
            FirstImageDownloaderViewModel.ClearState();
            SecondImageDownloaderViewModel.ClearState();
            ThirdImageDownloaderViewModel.ClearState();
            TotalDownloadingProgress.TotalDownloadingProgressValue = -1;
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

            FirstImageDownloaderViewModel = new ImageDownloaderViewModel();
            SecondImageDownloaderViewModel = new ImageDownloaderViewModel();
            ThirdImageDownloaderViewModel = new ImageDownloaderViewModel();

            FirstImageDownloaderViewModel.PropertyChanged += FirstImageDownloader_PropertyChanged;
            SecondImageDownloaderViewModel.PropertyChanged += SecondImageDownloader_PropertyChanged;
            ThirdImageDownloaderViewModel.PropertyChanged += ThirdImageDownloader_PropertyChanged;

            CountActiveDownloading = 0;
            TotalDownloadingProgress = new TotalDownloadingProgress();
            TotalDownloadingProgress.PropertyChanged += TotalDownloadingProgress_PropertyChanged;
        }

        public MainViewModel(ImageDownloaderViewModel first, ImageDownloaderViewModel second, ImageDownloaderViewModel third)
        {
            FirstImageDownloaderViewModel = first;
            SecondImageDownloaderViewModel = second;
            ThirdImageDownloaderViewModel = third;

            CountActiveDownloading = 0;
            TotalDownloadingProgress = new TotalDownloadingProgress();
        }

        #endregion Constructors

        #region PropertyChange Methods

        private void FirstImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DownloadingProgress":
                    TotalDownloadingProgress.TotalDownloadingProgressValue = (FirstImageDownloaderViewModel.DownloadingProgress + SecondImageDownloaderViewModel.DownloadingProgress
                            + ThirdImageDownloaderViewModel.DownloadingProgress) / (CountActiveDownloading != 0 ? CountActiveDownloading : 1);
                    RaisePropertyChanged(nameof(TotalDownloadingProgress));
                    break;
                case "DownloadingState":
                    if(((ImageDownloaderViewModel)sender).DownloadingState == DownloadingState.Downloading)
                    {
                        CountActiveDownloading++;
                    }
                    else if(((ImageDownloaderViewModel)sender).DownloadingState == DownloadingState.Completed)
                    {
                        CountActiveDownloading--;
                    }
                    RaisePropertyChanged(nameof(CountActiveDownloading));
                    break;
            }
        }

        private void SecondImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DownloadingProgress":
                    TotalDownloadingProgress.TotalDownloadingProgressValue = (FirstImageDownloaderViewModel.DownloadingProgress + SecondImageDownloaderViewModel.DownloadingProgress
                            + ThirdImageDownloaderViewModel.DownloadingProgress) / (CountActiveDownloading != 0 ? CountActiveDownloading : 1);
                    RaisePropertyChanged(nameof(TotalDownloadingProgress));
                    break;
                case "DownloadingState":
                    if (((ImageDownloaderViewModel)sender).DownloadingState == DownloadingState.Downloading)
                    {
                        CountActiveDownloading++;
                    }
                    else if (((ImageDownloaderViewModel)sender).DownloadingState == DownloadingState.Completed)
                    {
                        CountActiveDownloading--;
                    }
                    RaisePropertyChanged(nameof(CountActiveDownloading));
                    break;
            }
        }

        private void ThirdImageDownloader_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "DownloadingProgress":
                    TotalDownloadingProgress.TotalDownloadingProgressValue = (FirstImageDownloaderViewModel.DownloadingProgress + SecondImageDownloaderViewModel.DownloadingProgress
                            + ThirdImageDownloaderViewModel.DownloadingProgress) / (CountActiveDownloading != 0 ? CountActiveDownloading : 1);
                    RaisePropertyChanged(nameof(TotalDownloadingProgress));
                    break;
                case "DownloadingState":
                    if (((ImageDownloaderViewModel)sender).DownloadingState == DownloadingState.Downloading)
                    {
                        CountActiveDownloading++;
                    }
                    else if (((ImageDownloaderViewModel)sender).DownloadingState == DownloadingState.Completed)
                    {
                        CountActiveDownloading--;
                    }
                    RaisePropertyChanged(nameof(CountActiveDownloading));
                    break;
            }
        }

        private async void TotalDownloadingProgress_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TotalDownloadingProgressValue")
            {
                if (Math.Abs(((TotalDownloadingProgress)sender).TotalDownloadingProgressValue - 100.0) < 0.1)
                {
                    await Task.Run(ApplyCompletedState);
                }
            }
        }

        #endregion PropertyChange Methods

    }
}
