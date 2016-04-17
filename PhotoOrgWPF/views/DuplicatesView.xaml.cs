﻿using PhotoOrgWPF.controller;
using PhotoOrgWPF.models;
using System;
using System.Collections;
using System.Collections.Generic;
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

namespace PhotoOrgWPF.views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DuplicatesView : UserControl
    {
        public ImportController Controller { get; set; }

        public DuplicatesView()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnExclude_Click(object sender, RoutedEventArgs e)
        {
            IList items = (IList)lstIncluded.SelectedItems;
            PhotoList included = ((Duplicates)DataContext).OrgList;
            PhotoList excluded = ((Duplicates)DataContext).DupList;
            Controller.ExcludeIncludedPhotos(items, included, excluded);
        }

        private void btnInclude_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList items = (System.Collections.IList)lstExcluded.SelectedItems;
            PhotoList included = ((Duplicates)DataContext).OrgList;
            PhotoList excluded = ((Duplicates)DataContext).DupList;
            Controller.IncludeExcludedPhotos(items, included, excluded);
        }
    }
}
