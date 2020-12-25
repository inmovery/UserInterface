using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UserInterfaceTest.ViewModels
{
    /// <summary>
    /// A base view model that fires Property Changed events as needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// The event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Call this to fire a <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Call this to a fire a PropertyChanged event by typical method
        /// </summary>
        /// <param name="prop"></param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
