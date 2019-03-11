using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// when the app is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //.GetLogicalDrivers() -- a helper in the framework, which retrieves the names of the logical drivers of this computer ( e.g.: Driver C, Driver D etc)
            //we iterate through all these logical drivers and create a new TreeView Item
            //then for that item we want to add it to our FolderViewItem
            foreach (var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem()
                {
                    //adding header through binding
                    Header = drive,
                    //.Tag - an object that can store any information that we want
                    //get the full path
                    Tag = drive
                };
                

                //add a dummy dropdown item
                item.Items.Add(null);

                //listen out for items being expanded
                item.Expanded += Folder_Expanded;

                FolderView.Items.Add(item);
            }
        }

        #region Folder Expanded

        /// <summary>
        /// When a folder is expanded, find the sub folders/files
        /// </summary>
        /// <param name="sender">it will be the TreeViewItem</param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region Initial checks
            var item = (TreeViewItem)sender;

            //If the item is not 1 ir the first item is not null, then do nothing
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            //otherwise, clear dummy data & get full path
            item.Items.Clear();
            var fullPath = (string)item.Tag;
            #endregion

            #region Get Folders
            //find every folder (recursively) and put them in a list of strings
            //try-catch - if user does not have access to a folder, that can throw exception - BAD PRACTICE, try catch is
            //used here only for demonstration purposes, all exceptions are ignored here
            var directories = new List<string>();
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

            //for each directory, set the details of the item
            directories.ForEach(directoryPath =>
            {
                var subItem = new TreeViewItem()
                {
                    //header is the name of the folder
                    Header = GetFileFolderName(directoryPath),
                    //tag is the entire path
                    Tag = directoryPath
                };

                //add dummy item so we can expand the folder
                subItem.Items.Add(null);

                //handle expanding out - recursively using the method above
                subItem.Expanded += Folder_Expanded;

                //add this item to the parent
                item.Items.Add(subItem);
            });
            #endregion

            #region Get Files
            //create a blank list of files
            var files = new List<string>();
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }

            //for each file, set the details of the item
            files.ForEach(filePath =>
            {
                var subItem = new TreeViewItem()
                {
                    //header is the name of the folder
                    Header = GetFileFolderName(filePath),
                    //tag is the entire path
                    Tag = filePath
                };

                //add this item to the parent
                item.Items.Add(subItem);
            });
            #endregion

        }
        #endregion

        /// <summary>
        /// Find the file or folder name from a full path
        /// eg.: C:\Something\a folder, C:\something\a file -- will return only the last part of the path
        /// </summary>
        /// <param name="directoryPath">the full path</param>
        public static string GetFileFolderName(string directoryPath)
        {
            //if there is no path, return empty
            if (string.IsNullOrEmpty(directoryPath))
                return string.Empty;

            //...else, make all slashes backslashes (double backslash for escaping caracter)
            var normalizedPath = directoryPath.Replace('/', '\\');

            //find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            //if we dont find a backslash, return the path itself as presumably it is going to be the filename itself
            if (lastIndex <= 0)
                return directoryPath;

            //return the name after the last backslash
            return directoryPath.Substring(lastIndex + 1);
        }
    }
}
