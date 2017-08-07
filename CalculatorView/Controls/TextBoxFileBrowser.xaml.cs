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

namespace CalculatorView.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxFileBrowser.xaml
    /// </summary>
    public partial class TextBoxFileBrowser : UserControl
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(TextBoxFileBrowser), new PropertyMetadata("C:\\"));

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public TextBoxFileBrowser()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fd = new Microsoft.Win32.OpenFileDialog();
            fd.Filter = "SQLite files (*.sqlite) |*.sqlite";
            if (fd.ShowDialog() == false)
            {
                return;
            }
            else
            {
                if (fd.FileName.Equals(""))
                {

                }
                else
                {
                    Path = fd.FileName;
                }
            }
        }
    }
}
