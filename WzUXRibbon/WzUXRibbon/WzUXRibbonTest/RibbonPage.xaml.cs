using System;
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

namespace WzUXRibbonTest
{
    /// <summary>
    /// Interaction logic for RibbonPage.xaml
    /// </summary>
    public partial class RibbonPage : Page
    {
        public RibbonPage()
        {
            InitializeComponent();
        }

        public void InitDatatext()
        {

        }

        private void Ribbon_Loaded(object sender, RoutedEventArgs e)
        {
            var ribbon = sender as WzUXRibbon.Controls.Ribbon;
            ribbon.IsMinimized = false;
        }
    }
}
