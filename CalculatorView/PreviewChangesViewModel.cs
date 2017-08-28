using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorView
{
    public class PreviewChangesViewModel
    {
        #region Notes
        #endregion
        #region Fields
        private string _title;
        private string _keyHeader;
        private List<PreviewRowItem> _items = null;
        #endregion
        #region Properties
        public string Title
        {
            get { return _title; }
        }
        public List<PreviewRowItem> Items
        {
            get { return _items; }
        }
        public string KeyHeader
        {
            get { return _keyHeader; }
        }
        #endregion
        #region Constructors
        public PreviewChangesViewModel(string title, string keyHeader, List<PreviewRowItem> items)
        {
            _title = title;
            _items = items;
            _keyHeader = keyHeader;
        }
        #endregion
        #region Voids
        #endregion
        #region Functions
        #endregion
    }
}
