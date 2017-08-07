using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickHelp;

namespace CalculatorView.Controls
{
    public delegate void TextToAddHandler(string text);
    public class FCTreeItem: System.Windows.Controls.TreeViewItem
    {
        #region Notes
        #endregion
        #region Fields
        private FieldCalculationParser.IDisplayToTreeNode _ParseTreeNode;
        #endregion

        #region Events
        public event TextToAddHandler TextToAdd;
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public FCTreeItem(FieldCalculationParser.IDisplayToTreeNode item):base()
        {
            _ParseTreeNode = item;
            base.Header = item.DisplayName;
            System.Windows.Controls.ToolTip t = new System.Windows.Controls.ToolTip();
            t.Content = _ParseTreeNode.FunctionSyntax;
            base.ToolTip = t;
            System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();
            System.Windows.Controls.MenuItem menuitem = new System.Windows.Controls.MenuItem();
            menuitem.Header = "Help for " + _ParseTreeNode.DisplayName;
            menuitem.Click += LaunchHelp;
            base.MouseDoubleClick += FCTreeItem_MouseDoubleClick;
            menu.Items.Add(menuitem);
            base.ContextMenu = menu;
        }
        #endregion
        #region Voids
        private void LaunchHelp(object sender, System.Windows.RoutedEventArgs e)
        {
            QuickHelp.HelpDialog hd = new HelpDialog(_ParseTreeNode.HelpFile, _ParseTreeNode.DisplayName, _ParseTreeNode.GetType().Assembly.FullName, _ParseTreeNode.GetType().Namespace);
            hd.Show();
        }
        private void FCTreeItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                TextToAdd(_ParseTreeNode.FunctionSyntax);
            }
        }
        #endregion
        #region Functions
        #endregion
    }
}
