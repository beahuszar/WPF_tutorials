using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace WpfTreeView
{
    /// <summary>
    /// A viewModel for each directory item
    /// </summary>
    public class DirectoryItemViewModel : BaseViewModel
    {
        #region Public properties

        public DirectoryItemType Type { get; set; }
        public string FullPath { get; set; }
        public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath); } }


        /// <summary>
        /// In ViewModel we tend to use Observable Collection instead of Lists
        /// it is just a list, which has another INotifyCollectionChanged event 
        /// so that any ViewModel listening out to this list whenever a new item is added or any item is removed
        /// </summary>
        /// A list of all children contained inside this item
        public ObservableCollection<DirectoryItemViewModel> Children { get; set; }

        //indicates if this item can be extended
        //this is a UI specific property
        public bool CanExpand { get { return this.Type != DirectoryItemType.File; } }

        public bool IsExpanded
        {
            get
            {
                //find a children and make sure we got some -> '?' here
                //get the count of the children list, but we want children that are not equal to null (because that's going to be the dummy item)
                //i.e.: if we have any children which is not null, then we are expanded
                return this.Children?.Count(f => f != null) > 0;
            }
            set
            {
                //if we are not expanded, but the UI tells us the we want to be expanded
                //value == true -> UI saying that we are expanded
                if (value == true)
                    //find all children
                    Expand();
                else
                    this.ClearChildren();
            }
        }
        #endregion

        #region Public commands
        //if a button is clicked, or any action is needed on the UI
        //all the commands should have no inputs, as the model should have self aware properties

        /// <summary>
        /// the command that expands this items
        /// </summary>
        public ICommand ExpandCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="fullPath">full path of this item</param>
        /// <param name="type">type of this item</param>
        public DirectoryItemViewModel(string fullPath, DirectoryItemType type)
        {
            //create command
            this.ExpandCommand = new RelayCommand(Expand);
            //set properties
            this.FullPath = fullPath;
            this.Type = type;

            //setup the children as needed
            this.ClearChildren();
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// removes all children from the list & adds a dummy item to show the expand icon if required
        /// </summary>
        private void ClearChildren()
        {
            //clear items
            this.Children = new ObservableCollection<DirectoryItemViewModel>();

            //show the expand arrow if we are not a file
            if (this.Type != DirectoryItemType.File)
                this.Children.Add(null);
        }
        #endregion

        /// <summary>
        /// Expands this directory and finds all children
        /// </summary>
        private void Expand()
        {
            if (this.Type == DirectoryItemType.File)
                return;

            //find all children
            this.Children = new ObservableCollection<DirectoryItemViewModel>(
                DirectoryStructure.GetDirectoryContents(this.FullPath).Select(content => new DirectoryItemViewModel(content.FullPath, content.Type)));
            
        }

        
    }
}
