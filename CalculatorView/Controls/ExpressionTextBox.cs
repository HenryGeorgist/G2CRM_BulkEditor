using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorView.Controls
{
    public delegate void ErrorsFoundHandler(bool TreeIsNull);
    public delegate void ParseSuccessHandler();
    public class ExpressionTextBox: System.Windows.Controls.TextBox
    {
        #region Notes
        #endregion
        #region Fields
        private Stack<System.Windows.Media.SolidColorBrush> _ParenColors;
        private List<System.Windows.Media.SolidColorBrush> _colorList;
        private System.Windows.Controls.TextBlock _TextBlock;
        private System.ComponentModel.BackgroundWorker _BW;
        private int _Numlines;
        private System.Diagnostics.Stopwatch _s;
        private FieldCalculationParser.ParseTreeNode _tree;
        private string _Result;
        private bool _IsCaseSensitive;
        private List<string> _headers;
        private List<Type> _HeaderTypes;
        private Object[] _FirstRowOfData;
        private FieldCalculationParser.TypeEnum _OutputType;
        #endregion
        #region Events
        public event ErrorsFoundHandler ErrorsFound;
        public event ParseSuccessHandler ParseSuccess;
        #endregion
        #region Properties
        public bool IsCaseSensitive
        {
            get { return _IsCaseSensitive; }
            set { _IsCaseSensitive = value; }
        }
        public FieldCalculationParser.ParseTreeNode GetTree{get { return _tree; }}
        public string Result { get { return _Result; } }
        public List<string> SetHeaders { set { _headers = value; } }
        public List<Type> SetHeaderTypes { set { _HeaderTypes = value; } }
        public System.Windows.Controls.TextBlock TextBlock { get { return _TextBlock; }}
        public object[] SetDataForFirstRow { set { _FirstRowOfData = value; } }
        public FieldCalculationParser.TypeEnum SetOutputType
        {
            set
            {
                _OutputType = value;
            }
        }
        #endregion
        #region Constructors
        public ExpressionTextBox()
        {
            _OutputType = FieldCalculationParser.TypeEnum.UnDeclared;
            _s = new System.Diagnostics.Stopwatch();
            _BW = new System.ComponentModel.BackgroundWorker();
            _BW.DoWork += swStart;
            _BW.RunWorkerCompleted += RemoveTextBlock;
            _BW.WorkerSupportsCancellation = true;
            TextWrapping = System.Windows.TextWrapping.Wrap;
            _Numlines = 1;
            _TextBlock = new System.Windows.Controls.TextBlock();
            _TextBlock.TextWrapping = System.Windows.TextWrapping.Wrap;
            _TextBlock.Margin = new System.Windows.Thickness(5, 16, 5, 0);
            _TextBlock.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(230, 240, 240, 240));
            _TextBlock.Effect = new System.Windows.Media.Effects.DropShadowEffect();
            _TextBlock.Visibility = System.Windows.Visibility.Hidden;
            _TextBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _TextBlock.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            _TextBlock.MouseEnter += CancelBackgroundWorker;
            _TextBlock.MouseLeave += StartBackgroundWorker;
            System.Windows.Controls.Grid.SetRow(_TextBlock, 1);
            _colorList = new List<System.Windows.Media.SolidColorBrush>();
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 0)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 255, 0)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 255)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 204, 204)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 130, 0)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 255, 128)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 157, 0, 255)));
            _colorList.Add(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 102, 204, 0)));
            KeyUp += ExpressionTextBox_KeyUp;
        }

        private void ExpressionTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Parse();
        }
        #endregion
        #region Voids
        #region TextBlock
        private void ShiftDownward()
        {
            _TextBlock.Margin = new System.Windows.Thickness(_TextBlock.Margin.Left, 5 + (16 * base.LineCount), _TextBlock.Margin.Right, _TextBlock.Margin.Bottom);
            _Numlines = base.LineCount;
        }
        private void RemoveTextBlock(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            _TextBlock.Visibility = System.Windows.Visibility.Hidden;
        }
        private void StartBackgroundWorker(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _s.Restart();
            if (!_BW.IsBusy) { _BW.RunWorkerAsync(); }
        }
        private  void CancelBackgroundWorker(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _s.Stop();
        }
        private void swStart(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (_s.ElapsedMilliseconds < 2000)
            {

            }
        }
        private void SetRichText(int pos,FieldCalculationParser.TokenEnum token, string text, string helpdoc)
        {
            _TextBlock.Visibility = System.Windows.Visibility.Hidden;
            _TextBlock.MaxWidth = base.ActualWidth - ((_TextBlock.Margin.Left - base.Margin.Left) + (_TextBlock.Margin.Right - base.Margin.Right));
            if (helpdoc == "")
            {
                if (token == FieldCalculationParser.TokenEnum.LPAREN)
                {
                    _ParenColors.Push(_colorList[_colorList.Count % _colorList.Count]);
                    _TextBlock.Inlines.Add(text);
                    _TextBlock.Inlines.Last().Foreground = _ParenColors.Peek();
                }else if(token == FieldCalculationParser.TokenEnum.RPAREN)
                {
                    if (_ParenColors.Count > 0)
                    {
                        _TextBlock.Inlines.Add(text);
                        _TextBlock.Inlines.Last().Foreground = _ParenColors.Pop();
                    }else
                    {
                        _TextBlock.Inlines.Add(text);
                    }
                }else
                {
                    _TextBlock.Inlines.Add(text);
                }
            }else
            {
                System.Windows.Documents.Bold r = new System.Windows.Documents.Bold(new System.Windows.Documents.Run(text));
                System.Windows.Documents.Hyperlink link = new System.Windows.Documents.Hyperlink(r);
                link.Tag = text;
                link.NavigateUri = new System.Uri(helpdoc, System.UriKind.Relative);
                link.IsEnabled = true;
                link.Click += link_RequestNavigate;
                _TextBlock.Inlines.Add(link);
            }
            _TextBlock.Visibility = System.Windows.Visibility.Visible;
        }
        private void link_RequestNavigate(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Documents.Hyperlink link = (System.Windows.Documents.Hyperlink)sender;
            string[] tmp = link.NavigateUri.ToString().Split(new Char[] { '.' });
            string filestring = tmp[1];
            for(int i = 2; i < tmp.Count(); i++)
            {
                filestring = filestring + "." + tmp[i];
            }
            QuickHelp.HelpDialog hd = new QuickHelp.HelpDialog(filestring, link.Tag.ToString(), _tree.GetType().Assembly.FullName, _tree.GetType().Namespace);
            hd.Show();
        }
        #endregion
        public void Parse()
        {
            if (_FirstRowOfData == null)
            {
                _Result = "No Rows In Database";//doesnt work with mvvm very well..
                //raiseevent parsesuccess
                return;
            }
            _ParenColors = new Stack<System.Windows.Media.SolidColorBrush>();
            _TextBlock.Inlines.Clear();
            if(_Numlines!=base.LineCount){ShiftDownward();}
            _s.Restart();
            if (!_BW.IsBusy) { _BW.RunWorkerAsync(); }
            byte[] strbytes = new System.Text.UTF8Encoding().GetBytes(base.Text);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(strbytes);
            FieldCalculationParser.Scanner scanner = new FieldCalculationParser.Scanner(ms);
            FieldCalculationParser.Parser parser = new FieldCalculationParser.Parser(scanner);
            parser.TokenFound += SetRichText;
            FieldCalculationParser.ParseTreeNode.IsCaseSensitive = _IsCaseSensitive;
            FieldCalculationParser.ParseTreeNode.RowOrCellNum = 0;
            try
            {
                _tree = parser.Parse(_headers, _HeaderTypes, _OutputType);
                _TextBlock.Inlines.Add(" ");//why?
                if (_tree == null)
                {
                    _Result = "";
                    ErrorsFound(true);
                } else
                {
                    if (_tree.GetParseErrors.Count() > 0)
                    {
                        if (_tree.ContainsError()) {
                            _Result = "Tree Contains Parse Errors";
                            ErrorsFound(false);
                        } else
                        {
                            _Result = "Invalid Syntax, Check Error Log";
                            ErrorsFound(false);
                        }
                    } else
                    {
                        if (_tree.containsVariable())
                        {
                            List<string> selectedheaders = GetColumnsFromTree(_tree);
                            if (selectedheaders.Count() != 0)
                            {
                                _tree.SetColNums(selectedheaders);
                                object[] selectedvalues = GetDataFromDatabaseForUniqueColumns(selectedheaders);
                                _tree.Update(ref selectedvalues);
                            }
                        }
                        _Result = _tree.Evaluate().GetResult.ToString();
                        ParseSuccess();
                    }
                }
            }catch(Exception ex)
            {
                _Result = "Exception";
                ErrorsFound(true);
            }
        }
        public void Expression_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Parse();
        }
        #endregion
        #region Functions
        private List<string> GetColumnsFromTree(FieldCalculationParser.ParseTreeNode tree)
        {
            List<string> input = tree.GetHeaderNames();
            List<string> output = new List<string>();
            foreach(string s in input)
            {
                if (!output.Contains(s))
                {
                    output.Add(s);
                }
            }
            return output;
        }
        private object[] GetDataFromDatabaseForUniqueColumns(List<string> uniqueheaders)
        {
            List<object> data = new List<object>();
            if (_FirstRowOfData == null) { return data.ToArray(); }
            foreach(string s in uniqueheaders)
            {
                data.Add(_FirstRowOfData[_headers.IndexOf(s)]);
            }
            return data.ToArray();
        }
        #endregion
    }
}
