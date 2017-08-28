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
    /// Interaction logic for PreviewChanges.xaml
    /// </summary>
    public partial class PreviewChanges : Window
    {
        public PreviewChanges(PreviewChangesViewModel pcv)
        {
            DataContext = pcv;
            
            InitializeComponent();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            if (e.PropertyName == nameof(PreviewRowItem.Key))
            {
                e.Column.Header = ((PreviewChangesViewModel)DataContext).KeyHeader;
            }
            if(e.PropertyName == nameof(PreviewRowItem.HasChanges))
            {
                e.Column.Header = "Has Changes";
                
            }
            if (e.PropertyName == nameof(PreviewRowItem.NewValue))
            {
                e.Column.Header = "New Value";
            }
        }
    }
}
