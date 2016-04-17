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

        private MainData mainData = new MainData();

        private FolderList photoFolders = new FolderList();                                                    // ToDo: Move into MainData
        private  ObservableCollection<SourceFolder> sourceFolders = new ObservableCollection<SourceFolder>();  // ToDo: Move into MainData

        protected PhotosController photosController;
        protected ImportController importController;

        public MainWindow()
        {
            InitializeComponent();

            this.Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag); 
            this.DataContext = photoFolders;

            photosController = new PhotosController(photoFolders);
            importController = new ImportController(photoFolders);

            LoadSourceFolders(sourceFolders);
            lstImport.DataContext = sourceFolders;

            ShowIsBusy(false);
            pnlDuplicates.Visibility = System.Windows.Visibility.Collapsed;
            pnlDuplicates.DataContext = mainData.duplicates;
        }

        private void LoadSourceFolders(ObservableCollection<SourceFolder> sources)
        {
            sources.Clear();

            // Set up Final Target
            SourceFolder final = new SourceFolder("Final PhotoBox", @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\FinalPhotoBox"); 
            //sources.Insert(0, final);

            // Setup Buffer
            SourceFolder buffer = new SourceFolder("Buffer", @"C:\Users\Vegard\Documents\Dropbox\Temp\iPhotoMerge\Buffer", final);

            // Setup Sources
            foreach (var path in myImagePaths)
            {
                string name = Path.GetFileName(path);
                SourceFolder source = new SourceFolder(name, path, buffer);
                sources.Add(source);
            }

            sources.Add(buffer);


        }

        // -- Debug - Kladd
        private void onReadFolder(object sender, RoutedEventArgs e)
        {
            ShowIsBusy(true);
            photoFolders.Clear();

            photosController.LoadFolder(sourceFolders[2]);
            photosController.AutoSplitFolders();
            UpdateView();
            photosController.LoadFolder(sourceFolders[3]);
            photosController.AutoSplitFolders();
            UpdateView();
            /*
            controller.LoadFolder(sourceFolders[0]);
            controller.AutoSplitFolders();
            UpdateView();
            // */
            photosController.LoadFolder(sourceFolders[1]);
            photosController.AutoSplitFolders();
            UpdateView();

            ShowIsBusy(false);
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
                    mainData.duplicates.Clear();
                    PhotoList originals = mainData.duplicates.OrgList;
                    PhotoList duplicates = mainData.duplicates.DupList;
                    

                    ShowIsBusy(true);
                    photoFolders.Clear();
                    photosController.LoadFolder(sourceFolder);
                    importController.RegisterNewPhotos(photoFolders, ref originals, ref duplicates);

                    // Show Duplicates
                    if (duplicates.Count > 0)
                    {
                        pnlDuplicates.Visibility = System.Windows.Visibility.Visible;
                        Debug.WriteLine("Found duplicates:");
                        for (int i = 0; i < duplicates.Count; i++)
                        {
                            string msg = " - " + duplicates[i].Photoname;
                            if (i < originals.Count)
                                msg += " is duplicate of " + originals[i].Photoname;
                            Debug.WriteLine(msg);
                        }
                    }
                    else
                    {
                        photosController.AutoSplitFolders();
                    }
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
                    string path = Path.GetDirectoryName(folder.PhotosDisk[0].FullPath);
                    Process.Start(path);
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

        public void UpdateView()
        {
            Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null); 
        }

    }
}
