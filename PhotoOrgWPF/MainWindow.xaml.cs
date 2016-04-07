using PhotoOrgWPF.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PhotoOrgWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int useIndex = 2;
        string[] myImagePaths = new string[] {@"C:\Users\Vegard\Pictures\Skal importeres\115_PANA", 
                               @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato",
                               @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato\Email",
                               @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato\Subfolder 2"};

        string imagePath = null;

        
        ObservableCollection<PhotoFolder> photoFolders = new ObservableCollection<PhotoFolder>();
        Photo myPhoto = null;

        public MainWindow()
        {
            InitializeComponent();

            imagePath = myImagePaths[useIndex];
            myPhoto = new Photo(@"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato\Email\2 - Sommerfuglpark.JPG");
            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag); 
            this.DataContext = photoFolders;
            //this.DataContext = myPhoto;
        }

        private void onReadFolder(object sender, RoutedEventArgs e)
        {
            
            //lstImages.DataContext = photoFolder;
            //txtPathBound.DataContext = photoFolder;
            photoFolders.Clear();

            photoFolders.Add(new PhotoFolder(myImagePaths[2]));
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            photoFolders.Add(new PhotoFolder(myImagePaths[3]));
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null); 
            //photoFolders.Add(new PhotoFolder(myImagePaths[0]));
            //Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
            photoFolders.Add(new PhotoFolder(myImagePaths[1]));
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);

            txtPathBound_Copy.DataContext = photoFolders[0].Photos;


            txtPath.Content = "(" + photoFolders[0].Photos.Count + ") " + imagePath;

            txtPathBound_Copy.GetBindingExpression(Label.ContentProperty).UpdateTarget();
        }

        private void btnOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(imagePath);
        }

        private void onViewFullFolderClick(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                PhotoFolder folder = (PhotoFolder) ((Button)sender).DataContext;
                folder.ViewAllPhotos();
            }

            

        }

    }
}
