using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorExample
{
    public class ViewModel :BaseViewModel
    {
        #region Notes
        #endregion
        #region Fields
        private string _FilePath;
        private List<string> _TableNames;
        private List<string> _ColumnNames;
        #endregion
        #region Properties
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; NotifyPropertyChanged(nameof(FilePath));}
        }
        public List<string> ColumnNames
        {
            get { return _ColumnNames; }
            set { _ColumnNames = value; NotifyPropertyChanged(nameof(ColumnNames)); }
        }
        public List<string> TableNames
        {
            get { return _TableNames; }
            set { _TableNames = value; NotifyPropertyChanged(nameof(TableNames)); }
        }
        #endregion
        #region Constructors
        public ViewModel() : base()
        {

        }
        #endregion
        #region Voids
        public void SetTableNames(string path)
        {
            TableNames = Database.Reader.SqLiteReader.TableNames(FilePath).ToList();
        }
        public void SetColumnNames(string path, string tablename)
        {
            Database.Reader.SqLiteReader reader = new Database.Reader.SqLiteReader(path, tablename);
            ColumnNames = reader.ColumnNames().ToList();
        }
        public override void AddValidationRules()
        {
            AddRule(nameof(FilePath), () => System.IO.File.Exists(FilePath), "Database must exist.");
            AddRule(nameof(FilePath), () => System.IO.Path.GetExtension(FilePath)==".sqlite", "Database must be of the extention .sqlite.");
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Functions
        #endregion
    }
}
