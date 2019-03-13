using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfTreeView
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder or path
    /// Attribute has to be given so that xaml file finds it
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //get the full path
            var path = (string)value;

            //if the path is null, ignore
            if (path == null)
                return null;

            //get the name of the file/folder
            var name = DirectoryStructure.GetFileFolderName(path);

            //by default we presume an image
            var image = "Images/C.jpg";

            //if the name is blank, we presume it's a drive (we cannot have a blank file/image name)
            if (string.IsNullOrEmpty(name))
                image = "Images/A.jpg";
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
                image = "Images/B.jpg";

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
