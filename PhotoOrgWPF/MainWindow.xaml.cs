using PhotoOrgWPF.controller;
using PhotoOrgWPF.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Threading;

namespace PhotoOrgWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] myImagePaths = new string[] {@"C:\Users\Vegard\Pictures\Skal importeres\115_PANA", 
                               @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato",
                               @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato\Email",
                               @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Cato\Subfolder 2"};

        string imagePath = null;


        private  ObservableCollection<PhotoFolder> photoFolders = new ObservableCollection<PhotoFolder>();
        private  ObservableCollection<SourceFolder> sources = new ObservableCollection<SourceFolder>();

        protected PhotosController controller;

        public MainWindow()
        {
            InitializeComponent();

            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag); 
            this.DataContext = photoFolders;
            
             controller = new PhotosController(photoFolders);

            LoadSourceFolders(sources);
            lstImport.DataContext = sources;

            ShowIsBusy(false);
        }

        private void LoadSourceFolders(ObservableCollection<SourceFolder> sources)
        {
            sources.Clear();

            // Set up Final Target
            SourceFolder final = new SourceFolder("Final PhotoBox", @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\FinalPhotoBox"); 
            //sources.Insert(0, final);

            // Setup Buffer
            SourceFolder buffer = new SourceFolder("Buffer", @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Buffer", final);
            sources.Insert(0, buffer);

            // Setup Sources
            foreach (var path in myImagePaths)
            {
                string name = Path.GetFileName(path);
                SourceFolder source = new SourceFolder(name, path, buffer);
                sources.Insert(0, source);
            }


        }

        // -- Debug - Kladd
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
            

            txtPathBound_Copy.DataContext = photoFolders[0].PhotosView;


            txtPath.Content = "(" + photoFolders[0].PhotosView.Count + ") " + imagePath;

            txtPathBound_Copy.GetBindingExpression(Label.ContentProperty).UpdateTarget();
        }
        //------

        private void onImportFolderClick(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            if (button!=null)
            {
                SourceFolder sourceFolder = (SourceFolder)button.DataContext;
                if (sourceFolder!=null)
                {
                    ShowIsBusy(true);
                    controller.LoadFolder(sourceFolder);
                    controller.AutoSplitFolders();
                    ShowIsBusy(false);
                }
            }
        }

        private void onOpenExplorerClick(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            if (button != null)
            {
                PhotoFolder folder = (PhotoFolder)button.DataContext;
                if (folder != null)
                {
                    Process.Start(folder.FolderPath);
                }
            }

            
        }

        private void onViewFullFolderClick(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                ShowIsBusy(true);
                PhotoFolder folder = (PhotoFolder) ((Button)sender).DataContext;
                folder.ViewAllPhotos();
                ShowIsBusy(false);
            }

            

        }

        protected void ShowIsBusy(bool isBusy)
        {
            if (isBusy)
                lblIsWorking.Visibility = Visibility.Visible;
            else
                lblIsWorking.Visibility = Visibility.Hidden;
            UpdateView();
        }

        protected void UpdateView()
        {
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null); 
        }

    }
}
