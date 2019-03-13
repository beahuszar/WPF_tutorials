using System.Collections.ObjectModel;
using System.Linq;

namespace WpfTreeView
{
    /// <summary>
    /// A view model for the application main directory structure
    /// </summary>
    public class DirectoryStructureViewModel : BaseViewModel
    {
        #region Public properties

        //a list of all directories on the machine
        public ObservableCollection<DirectoryItemViewModel> Items { get; set; }

        #endregion

        #region Constructors

        public DirectoryStructureViewModel()
        {
            //Get the logical drives
            var children = DirectoryStructure.GetLogicalDrives();

            //Create the view models from the data
            this.Items = new ObservableCollection<DirectoryItemViewModel>(
                children.Select(drive => new DirectoryItemViewModel(drive.FullPath, DirectoryItemType.Drive)));
        }

        #endregion
    }
}
