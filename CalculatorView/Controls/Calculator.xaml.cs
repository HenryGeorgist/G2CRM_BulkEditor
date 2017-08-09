using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalculatorView.Controls
{
    /// <summary>
    /// Interaction logic for Calculator.xaml
    /// </summary>
    public partial class Calculator : Window, System.ComponentModel.INotifyPropertyChanged
    {
        private List<string> _ParseErrors;
        private string _firstParts;
        private string _secondParts;
        private List<string> _headers;
        private string _filePath;
        private string _tableName;
        private string _columnName;
        private string _columnType;
        private bool _AllowSelectionOption = false;
        private bool _UseSelection = false;
        private IEnumerable<long> _selectedKeys;
        private string _selectionColumnName;
        private long _totalRows;
        private object[] _FirstRow;
        private object[] _FirstSelectedRow;
        public event PropertyChangedEventHandler PropertyChanged;

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; NotifyPropertyChanged(); }//notify property changed.
        }
        public string ColumnType
        {
            get { return _columnType; }
            set { _columnType = value; NotifyPropertyChanged(); }//notify property changed.
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
                    return "Use Selection (" + _selectedKeys.Count() + " of " + _totalRows + ")";
                }
                return "";
            }
        }
        public Calculator()
        {

            InitializeComponent();
            //handle events from the various specialized components
            TestWindow.ErrorsFound += ErrorsFound;
            TestWindow.ParseSuccess += DisplayResult;
            AvailableFunctions.TextToAdd += TextToInsert;
            //assign the proper grid alignment to the textblock that is behaving like a tooltip.
            Grid.SetRow(TestWindow.TextBlock, 3);
            MainGrid.Children.Add(TestWindow.TextBlock);
            //Add the names of the columns to populate the available columns array. (this is currently handled
            _headers = new List<string>();
            _headers.Add("Column A");
            _headers.Add("Column B");
            _headers.Add("Column C");
            _headers.Add("Column D");
            _headers.Add("Column E");
            TestWindow.SetHeaders = _headers;
            //Add the types of the columns to enforce type strictness.
            List<Type> headertypes = new List<Type>();
            headertypes.Add(typeof(Int32));
            headertypes.Add(typeof(string));
            headertypes.Add(typeof(bool));
            headertypes.Add(typeof(Single));
            headertypes.Add(typeof(Int32));
            TestWindow.SetHeaderTypes = headertypes;
            //Add the first row of data to the database as an object array so that the result will show the result for the first row.
            object[] exampledata = new object[5];
            exampledata[0] = 1;
            exampledata[1] = "This is a string";
            exampledata[2] = true;
            exampledata[3] = 2.2;
            exampledata[4] = -3;
            TestWindow.SetDataForFirstRow = exampledata;
            //To create a selection, set the output type to boolean... otherwize it will default to undefined which allows any output.
            //TestWindow.SetOutputType = FieldCalculationParser.TypeEnum.Bool;
            //To operate as an editor for a specific column, pass the column type to the expression window, and the tree will only produce that output type.

            //Add the headers to the avaialblefields treeview. (i am adding type as a tooltip for reference)
            for (int i = 0; i < _headers.Count(); i++)
            {
                System.Windows.Controls.TreeViewItem tvi = new TreeViewItem();
                tvi.Header = _headers[i];
                System.Windows.Controls.ToolTip t = new System.Windows.Controls.ToolTip();
                t.Content = headertypes[i].ToString();
                tvi.ToolTip = t;
                AvailableFields.Items.Add(tvi);
            }

        }
        public Calculator(string filePath, string tablename, string operatingColumnName, IEnumerable<long> uniqueRowIDs = null, string uniqueColumnName = "")
        {

            InitializeComponent();
            _filePath = filePath;
            _tableName = tablename;
            _selectedKeys = uniqueRowIDs;
            _selectionColumnName = uniqueColumnName;
            ColumnName = operatingColumnName;

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
            switch (headertypes[_headers.IndexOf(_columnName)].Name.ToString().ToLower())
            {
                case "double":
                case "single":
                case "int":
                case "int16":
                case "int32":
                case "int64":
                    TestWindow.SetOutputType = FieldCalculationParser.TypeEnum.Num;//could set to decimal and int if you want more specification...
                    ColumnType = "Numeric";
                    break;
                case "bool":
                    TestWindow.SetOutputType = FieldCalculationParser.TypeEnum.Bool;
                    ColumnType = "True or False";
                    break;
                case "string":
                    TestWindow.SetOutputType = FieldCalculationParser.TypeEnum.Str;
                    ColumnType = "Text";
                    break;
                default:
                    TestWindow.SetOutputType = FieldCalculationParser.TypeEnum.UnDeclared;
                    ColumnType = "No output restrictions";
                    break;
            }

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
                List<string> usedheaders = tree.GetHeaderNames();
                string[] uniqueheaders = getUniqueHeaders(usedheaders);
                tree.SetColNums(uniqueheaders.ToList());
                Int64 count = reader.RowCount();

                List<object> output = new List<object>();

                reader.Open();
                //needs to operate on selected rows or not.

                //ifselected rows, update the entire column, but put the original value in for non selected rows.
                if (UseSelection)
                {
                    int currentSelectedRow = 0;
                    int selectionIndex = 0;
                    object[] tmp = reader.Column(_selectionColumnName);
                    object[] original = reader.Column(_columnName);
                    List<int> items = new List<int>();
                    foreach (long uid in _selectedKeys)
                    {
                        items.Add(Array.IndexOf(tmp, uid));
                    }
                    currentSelectedRow = items[selectionIndex];
                    for (Int64 i = 0; i < count; i++)
                    {
                        if (i == currentSelectedRow)
                        {
                            FieldCalculationParser.ParseTreeNode.RowOrCellNum = (int)i;//whatever.
                            if (uniqueheaders.Count() > 0)
                            {
                                row = reader.Row((int)i, uniqueheaders);
                                tree.Update(ref row);
                            }
                            output.Add(tree.Evaluate().GetResult);
                            selectionIndex++;
                            if (!(items.Count > selectionIndex))
                            {
                                currentSelectedRow = items[selectionIndex];
                            }
                        }
                        else
                        {
                            output.Add(original[i]);
                        }

                    }
                }
                else
                {
                    for (Int64 i = 0; i < count; i++)
                    {
                        FieldCalculationParser.ParseTreeNode.RowOrCellNum = (int)i;//whatever.
                        if (uniqueheaders.Count() > 0)
                        {
                            row = reader.Row((int)i, uniqueheaders);
                            tree.Update(ref row);
                        }
                        output.Add(tree.Evaluate().GetResult);
                    }
                }

                reader.Close();
                if (tree.GetComputeErrors.Count > 1)
                {
                    //do not write to file.
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
                    Database.Writer.SqLiteWriter writer = new Database.Writer.SqLiteWriter(_filePath, _tableName);
                    writer.UpdateColumn(_columnName, output.ToArray());
                    Close();
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
