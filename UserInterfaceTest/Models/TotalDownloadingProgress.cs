using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInterfaceTest.ViewModels;

namespace UserInterfaceTest.Models
{
    public class TotalDownloadingProgress : BaseViewModel
    {
        private double totalDownloadingProgress;

        public double TotalDownloadingProgressValue
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

        public TotalDownloadingProgress() {
            TotalDownloadingProgressValue = 0;
        }

    }
}
