using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorView.Controls
{
    public class AvailableFunctionsTreeView: System.Windows.Controls.TreeView
    {
        #region Notes
        #endregion
        #region Fields
        #endregion
        #region Events
        public event TextToAddHandler TextToAdd; //delegate was defined at the namespace level in the fctreeitem class
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public AvailableFunctionsTreeView() : base()
        {
            System.Reflection.Assembly dll = System.Reflection.Assembly.Load("FieldCalculationParser");
            Type[] s = dll.GetTypes();
            List<System.Windows.Controls.TreeViewItem> maintree = new List<System.Windows.Controls.TreeViewItem>();
            foreach (string en in System.Enum.GetNames(typeof(FieldCalculationParser.DisplayTypes)))
            {
                System.Windows.Controls.TreeViewItem item = new System.Windows.Controls.TreeViewItem();
                item.Header = en;
                maintree.Add(item);
            }
            FieldCalculationParser.ParseTreeNode.Initialize();
            FieldCalculationParser.IDisplayToTreeNode displayItem;
            Type output = null;
            foreach(Type t in s)
            {
                output = t.GetInterface("FieldCalculationParser.IDisplayToTreeNode");
                if (output != null)
                {
                    System.Runtime.Remoting.ObjectHandle oh = System.Activator.CreateInstance("FieldCalculationParser", t.FullName);//requires an empty constructor for all classes that implement idisplaytotreenode
                    displayItem = (FieldCalculationParser.IDisplayToTreeNode)oh.Unwrap();
                    FCTreeItem fc = new FCTreeItem(displayItem);
                    fc.TextToAdd += UpdateTextBox;
                    foreach(System.Windows.Controls.TreeViewItem tvi in maintree)
                    {
                        if (tvi.Header.Equals(displayItem.DisplayType.ToString()))
                        {
                            tvi.Items.Add(fc);
                        }
                    }
                }
            }
            foreach (System.Windows.Controls.TreeViewItem tvi in maintree)
            {
                base.Items.Add(tvi);
            }

        }
        #endregion
        #region Voids
        private void UpdateTextBox(string text)
        {
            TextToAdd(text);
        }
        #endregion
        #region Functions
        #endregion
    }
}
