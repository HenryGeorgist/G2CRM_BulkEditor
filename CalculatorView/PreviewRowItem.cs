using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorView
{
    public class PreviewRowItem
    {
        #region Notes
        #endregion
        #region Fields
        private object _key;
        private object _original;
        private object _newValue;
        private bool _hasChanges;
        #endregion
        #region Properties
        public object Key
        {
            get { return _key; }
        }
        public object Original
        {
            get { return _original; }
        }
        public object NewValue
        {
            get { return _newValue; }
        }
        public bool HasChanges
        {
            get { return _hasChanges; }
        }
        #endregion
        #region Constructors
        public PreviewRowItem(object key, object original, object newValue)
        {
            _key = key;
            _original = original;
            _newValue = newValue;
            if (original != null)
            {
                if (original is System.String)
                {
                    _hasChanges = !original.Equals(newValue);
                }else
                {
                    double val1 = Convert.ToDouble(original);
                    double val2 = Convert.ToDouble(newValue);
                    _hasChanges = val1 != val2;
                }

            }

        }
        #endregion
    }
}
