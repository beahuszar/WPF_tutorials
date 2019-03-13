namespace WpfTreeView
{
    /// <summary>
    /// Information about a directory item such as a drive, file or a folder
    /// </summary>
    public class DirectoryItem
    {
        public DirectoryItemType Type { get; set; }
        //the absolute path to this item
        public string FullPath { get; set; }

        //the name of this directory item
        public string Name { get { return this.Type == DirectoryItemType.Drive ? this.FullPath : DirectoryStructure.GetFileFolderName(this.FullPath); } }
    }
}
