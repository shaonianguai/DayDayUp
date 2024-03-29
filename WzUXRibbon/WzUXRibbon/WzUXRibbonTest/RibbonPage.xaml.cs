﻿using System;
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
using WzUXRibbonTest.Data;

namespace WzUXRibbonTest
{
    /// <summary>
    /// Interaction logic for RibbonPage.xaml
    /// </summary>
    public partial class RibbonPage : Page
    {
        private RibbonStateData _ribbonStateData;

        public RibbonPage()
        {
            InitializeComponent();
        }

        public void InitDatatext()
        {

        }

        private void Ribbon_Loaded(object sender, RoutedEventArgs e)
        {
            // stored data
            _ribbonStateData = new RibbonStateData(RibbonControl);
            _ribbonStateData.Load();

            RibbonControl.IsMinimized = false;
        }

        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Avatar_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpandOrMinmizedRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            RibbonControl.IsMinimized = RibbonControl.IsMinimized ? false : true;
        }
    }
}
