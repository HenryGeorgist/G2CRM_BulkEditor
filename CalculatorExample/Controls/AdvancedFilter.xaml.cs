using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculatorExample.Controls
{
    /// <summary>
    /// Interaction logic for AdvancedFilter.xaml
    /// </summary>
    public partial class AdvancedFilter : Window, System.ComponentModel.INotifyPropertyChanged
    {
        private List<string> _ParseErrors;
        private string _firstParts;
        private string _secondParts;
        private List<string> _headers;
        private string _filePath;
        private string _tableName;
        private string _columnType;
        private bool _AllowSelectionOption = false;
        private bool _UseSelection = false;
        private IEnumerable<long> _selectedKeys;
        private string _selectionColumnName;
        private long _totalRows;
        private object[] _FirstRow;
        private object[] _FirstSelectedRow;
        public event PropertyChangedEventHandler PropertyChanged;

        public string ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; NotifyPropertyChanged(); }
        }
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value;  NotifyPropertyChanged(); }
        }
        public bool AllowSelectionOption
        {
            get { return _AllowSelectionOption; }
            set { _AllowSelectionOption = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(SelectedCount)); }
        }
        public bool UseSelection
        {
            get { return _UseSelection; }
            set
            {
                _UseSelection = value;
                NotifyPropertyChanged();
                //update which selected row and parse.
                if (_UseSelection)//use property
                {
                    TestWindow.SetDataForFirstRow = _FirstSelectedRow;
                }
                else
                {
                    TestWindow.SetDataForFirstRow = _FirstRow;
                }
                TestWindow.Parse();
                //setcolsforfirstrow
                //parse
            }
        }
        public string SelectedCount
        {
            get
            {
                if (_selectedKeys != null)
                {
                    return "Select from current Selection (" + _selectedKeys.Count() + " of " + _totalRows + ")";
                }
                return "";
            }
        }
        public IEnumerable<long> SelectedKeys
        {
            get { return _selectedKeys; }
            set
            {
                _selectedKeys = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(SelectedCount));
            }
        }
        public bool FilterWasSuccessful { get; set; } = false;
        public AdvancedFilter(string filePath, string tablename, IEnumerable<long> uniqueRowIDs = null, string uniqueColumnName = "")
        {

            InitializeComponent();
            _filePath = filePath;
            TableName = tablename;
            SelectedKeys = uniqueRowIDs;
            _selectionColumnName = uniqueColumnName;

            //handle events from the various specialized components
            TestWindow.ErrorsFound += ErrorsFound;
            TestWindow.ParseSuccess += DisplayResult;
            AvailableFunctions.TextToAdd += TextToInsert;
            //assign the proper grid alignment to the textblock that is behaving like a tooltip.
            Grid.SetRow(TestWindow.TextBlock, 3);
            MainGrid.Children.Add(TestWindow.TextBlock);
            //Add the names of the columns to populate the available columns array. (this is currently handled
            Database.Reader.SqLiteReader reader = new Database.Reader.SqLiteReader(filePath, tablename);
            _headers = reader.ColumnNames().ToList();
            TestWindow.SetHeaders = _headers;
            //Add the types of the columns to enforce type strictness.
            List<Type> headertypes = reader.ColumnTypes().ToList();
            TestWindow.SetHeaderTypes = headertypes;
            //Add the first row of data to the database as an object array so that the result will show the result for the first row.
            reader.Open();
            _FirstRow = reader.Row(0);
            if (_selectedKeys != null)
            {
                if (_selectedKeys.Count() > 0)
                {
                    AllowSelectionOption = true;
                    object[] tmp = reader.Column(_selectionColumnName);
                    _FirstSelectedRow = reader.Row(Array.IndexOf(tmp, _selectedKeys.First()));
                }
            }
            _totalRows = reader.RowCount();
            reader.Close();
            TestWindow.SetDataForFirstRow = _FirstRow;
            //To create a selection, set the output type to boolean...
            TestWindow.SetOutputType = FieldCalculationParser.TypeEnum.Bool;
            ColumnType = "True or False";
            //Add the headers to the avaialblefields treeview. (i am adding type as a tooltip for reference)
            for (int i = 0; i < _headers.Count(); i++)
            {
                System.Windows.Controls.TreeViewItem tvi = new TreeViewItem();
                tvi.Header = _headers[i];
                System.Windows.Controls.ToolTip t = new System.Windows.Controls.ToolTip();
                t.Content = headertypes[i].Name;
                tvi.ToolTip = t;
                AvailableFields.Items.Add(tvi);
            }

        }
        private void TextToInsert(string text)
        {
            if (TestWindow.SelectedText != null)
            {
                TestWindow.Text = TestWindow.Text.Remove(TestWindow.CaretIndex, TestWindow.SelectedText.Count());
            }
            _firstParts = TestWindow.Text.Substring(0, TestWindow.CaretIndex);
            _secondParts = TestWindow.Text.Substring(TestWindow.CaretIndex, TestWindow.Text.Count() - TestWindow.CaretIndex);
            InsertText(text);
        }

        private void InsertText(string text)
        {
            TestWindow.Text = _firstParts + text + _secondParts;
            TestWindow.Focus();
            TestWindow.CaretIndex = _firstParts.Count() + text.Count();
            //TestWindow.CaretIndex = _CaratIndex;
            TestWindow.Parse();
        }

        private void DisplayResult()
        {
            Result.Content = TestWindow.Result;
            CmdErrorLog.IsEnabled = false;
            CmdExecute.IsEnabled = true;
        }

        private void ErrorsFound(bool TreeIsNull)
        {
            if (!TreeIsNull)
            {
                _ParseErrors = TestWindow.GetTree.GetParseErrors;
                CmdErrorLog.IsEnabled = true;
            }
            CmdExecute.IsEnabled = false;
            Result.Content = TestWindow.Result;
        }

        private void IsCaseSensitive_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                TestWindow.IsCaseSensitive = true;
            }
        }
        private void IsCaseSensitive_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                TestWindow.IsCaseSensitive = false;
            }
        }
        private void CmdErrorLog_Click(object sender, RoutedEventArgs e)
        {
            System.Text.StringBuilder s = new StringBuilder();
            foreach (string error in _ParseErrors)
            {
                s.AppendLine(error);
            }
            //display s to the user.
            Controls.ErrorWindow errorwindow = new Controls.ErrorWindow(s.ToString());
            errorwindow.ShowDialog();
        }

        private void CmdExecute_Click(object sender, RoutedEventArgs e)
        {
            //this is where you would get the parse tree from the expressionwindow, and then execute the tree for each row.
            FieldCalculationParser.ParseTreeNode tree = TestWindow.GetTree;
            FieldCalculationParser.ParseTreeNode.Initialize();//clear all errors.

            if (tree != null)
            {

                Database.Reader.SqLiteReader reader = new Database.Reader.SqLiteReader(_filePath, _tableName);
                object[] row;
                List<string> usedheaders = tree.GetHeaderNames();//will include duplicates
                string[] uniqueheaders = getUniqueHeaders(usedheaders);//removes duplicates
                tree.SetColNums(uniqueheaders.ToList());//lets the tree know what index in the array of object to look for for each header.
                Int64 count = reader.RowCount();

                List<long> output = new List<long>();

                reader.Open();
                //needs to operate on selected rows or not.

                //ifselected rows, only iterate on the current selection.
                if (UseSelection)
                {
                    object[] tmp = reader.Column(_selectionColumnName);

                    List<int> items = new List<int>();
                    int idx = 0;
                    foreach (long uid in _selectedKeys)
                    {
                        idx = Array.IndexOf(tmp, uid);
                        FieldCalculationParser.ParseTreeNode.RowOrCellNum = idx;
                        if (uniqueheaders.Count() > 0)
                        {
                            row = reader.Row(idx, uniqueheaders);
                            tree.Update(ref row);
                        }
                        if ((bool)tree.Evaluate().GetResult)
                        {
                            //select it!
                            output.Add(uid);
                        }
                    }
                }
                else
                {
                    object[] uniqueIDS = reader.Column(_selectionColumnName);
                    for (Int64 i = 0; i < count; i++)
                    {
                        FieldCalculationParser.ParseTreeNode.RowOrCellNum = (int)i;//whatever.
                        if (uniqueheaders.Count() > 0)
                        {
                            row = reader.Row((int)i, uniqueheaders);
                            tree.Update(ref row);
                        }
                        if ((bool)tree.Evaluate().GetResult)
                        {
                            //select it!
                            output.Add((long)uniqueIDS[i]);
                        }
                    }
                }

                reader.Close();
                if (tree.GetComputeErrors.Count > 1)
                {
                    //do not update selection
                    StringBuilder s = new StringBuilder();
                    foreach (string o in tree.GetComputeErrors)
                    {
                        s.AppendLine(o);
                    }
                    ErrorWindow err = new ErrorWindow(s.ToString());
                    err.Title = "Output";
                    err.ShowDialog();
                }
                else
                {
                    //update _selectedKeys;
                    SelectedKeys = output;
                    //raise event?
                    FilterWasSuccessful = true;
                }
            }
        }
        private string[] getUniqueHeaders(List<string> usedHeaders)
        {
            List<string> uniques = new List<string>();
            foreach (string s in usedHeaders)
            {
                if (!uniques.Contains(s))
                {
                    uniques.Add(s);
                }
            }
            return uniques.ToArray();
        }
        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AvailableFields_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AvailableFields.SelectedItem != null)
            {
                TreeViewItem tvi = (TreeViewItem)AvailableFields.SelectedItem;
                if (tvi != null)
                {
                    TextToInsert("[" + tvi.Header + "]");
                }

            }

        }
        private List<string> GetColumnsFromTree(FieldCalculationParser.ParseTreeNode tree)
        {
            List<string> input = tree.GetHeaderNames();
            List<string> output = new List<string>();
            foreach (string s in input)
            {
                if (!output.Contains(s))
                {
                    output.Add(s);
                }
            }
            return output;
        }
        private object[] GetDataFromDatabaseForUniqueColumns(List<string> uniqueheaders, object[] inputdata)
        {
            List<object> data = new List<object>();
            if (inputdata == null) { return data.ToArray(); }
            foreach (string s in uniqueheaders)
            {
                data.Add(inputdata[_headers.IndexOf(s)]);
            }
            return data.ToArray();
        }
        protected void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
