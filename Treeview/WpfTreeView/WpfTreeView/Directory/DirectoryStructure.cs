using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfTreeView
{
    /// <summary>
    /// A helper class to query information about directories
    /// </summary>
    public static class DirectoryStructure
    {
        public static List<DirectoryItem> GetLogicalDrives()
        {
            //.GetLogicalDrivers() -- a helper in the framework, which retrieves the names of the logical drivers of this computer ( e.g.: Driver C, Driver D etc)
            return Directory.GetLogicalDrives().Select(drive => new DirectoryItem { FullPath = drive, Type = DirectoryItemType.Drive }).ToList();
        }

        /// <summary>
        /// Gets the directory top-level content
        /// </summary>
        /// <param name="fullPath">the full path to the directory</param>
        /// <returns></returns>
        public static List<DirectoryItem> GetDirectoryContents(string fullPath)
        {
            var items = new List<DirectoryItem>();
            #region Get Folders
            
            //find every folder and put them in a list
            //try-catch - if user does not have access to a folder, that can throw exception - BAD PRACTICE, try catch is
            //used here only for demonstration purposes, all exceptions are ignored here
            try
            {
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    items.AddRange(dirs.Select(dir => new DirectoryItem { FullPath = dir, Type = DirectoryItemType.Folder}));
            }
            catch { }
            #endregion

            #region Get Files
            //create a blank list of files
            try
            {
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    items.AddRange(fs.Select(file => new DirectoryItem { FullPath = file, Type = DirectoryItemType.File} ));
            }
            catch { }
            #endregion

            return items;
        }
       
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
