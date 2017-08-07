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

namespace CalculatorExample
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel thevm = (ViewModel)Resources["vm"];
            if (!thevm.HasError)
            {
                if (CMBTableNames.SelectedIndex >= 0)
                {
                    if (CMBColumnNames.SelectedIndex >= 0)
                    {
                        CalculatorView.Controls.Calculator calculator = new CalculatorView.Controls.Calculator(thevm.FilePath, (string)CMBTableNames.SelectedItem, (string)CMBColumnNames.SelectedItem);
                        calculator.ShowDialog();
                    }
                    
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ViewModel thevm = (ViewModel)Resources["vm"];
            if (!thevm.HasError)
            {
                thevm.SetTableNames(thevm.FilePath);
                CMBTableNames.Focus();
                CMBTableNames.SelectedIndex = 0;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ViewModel thevm = (ViewModel)Resources["vm"];
            if (!thevm.HasError)
            {
                if (CMBTableNames.SelectedIndex >= 0)
                {
                    thevm.SetColumnNames(thevm.FilePath,(string)CMBTableNames.SelectedItem);
                    CMBColumnNames.Focus();
                    CMBColumnNames.SelectedIndex = 0;
                }
            }
        }
    }
}
