using System.ComponentModel;

namespace WpfTreeView
{
    /// <summary>
    /// a base viewModel that fires the propertyChanged event when needed
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// the event that is fired when any child property changes its value
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => {};
    }
}
