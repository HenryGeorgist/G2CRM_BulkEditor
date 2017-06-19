using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Reader
{
    public class SqLiteReader
    {
        #region Notes
        #endregion
        #region Fields
        private readonly System.Data.SQLite.SQLiteConnection _connection;
        private readonly string _tableName;
        private bool _isOpen = false;
        private Int64[] _rowIds;
        private string[] _columnNames;
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public SqLiteReader(string filepath, string tablename)
        {
            _tableName = tablename;
            _connection = new System.Data.SQLite.SQLiteConnection("Data Source=" + filepath + ";Version=3;");
            List<Int64> ids = new List<Int64>();
            int idx = 0;
            Open();
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("SELECT rowid FROM '" + _tableName + "'", _connection))
            {
                using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object tmp = reader[0]; //why does "rowid" not work?
                            ids.Add((Int64)tmp);
                        }
                    }
                }
            }
            Close();
            _rowIds = ids.ToArray();
            _columnNames = ColumnNames();
        }
        #endregion
        #region Voids
        public void Open()
        {
            _connection.Open();
            _isOpen = true;
        }
        public void Close()
        {
            _connection.Close();
            _isOpen = false;
        }
        #endregion
        #region Functions
        public Int64 RowCount()
        {
            bool wasOpen = _isOpen;
            if (!_isOpen) { Open(); }
            Int64 result;
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("SELECT Count(*) FROM '" + _tableName + "'", _connection))
            {
                result = (Int64)cmd.ExecuteScalar();
            }
            if (!wasOpen) { Close(); }
            return result;
        }
        public static string[] TableNames(string filepath)
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection("Data Source=" + filepath + ";Version=3;");
            List<string> result = new List<string>();
            conn.Open();

            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", conn))
            {
                using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result.Add((string)reader["name"]);
                        }
                    }
                }
            }
            conn.Close();
            return result.ToArray();
        }
        public object[] Row(int index)
        {
            List<object> row = new List<object>();
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("SELECT * FROM [" + _tableName + "] WHERE rowid=" + _rowIds[index], _connection))
            {
                using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            foreach (string s in _columnNames)
                            {
                                row.Add(reader[s]);
                            }
                        }
                    }
                }
            }
            return row.ToArray();
        }
        public object[] Row(int index, string[] columnNames)
        {
            List<object> row = new List<object>();
            string cmdstring = "SELECT [" + columnNames[0] + "]";
            for (int i = 1; i < columnNames.Count(); i++)
            {
                cmdstring += ",[" + columnNames[i] + "]";
            }
            int idx = 0;
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(cmdstring + " FROM [" + _tableName + "] WHERE rowid=" + _rowIds[index], _connection))
            {
                using (System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            foreach (string s in columnNames)
                            {
                                row.Add(reader[s]);
                            }
                        }
                    }
                }
            }
            return row.ToArray();
        }
        public string[] ColumnNames()
        {
            bool wasopen = _isOpen;
            if (!_isOpen) { Open(); }
            List<string> names = new List<string>();
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("PRAGMA table_info([" + _tableName + "])", _connection))
            {
                System.Data.SQLite.SQLiteDataAdapter adap = new System.Data.SQLite.SQLiteDataAdapter(cmd);
                System.Data.DataTable tab = new System.Data.DataTable();
                adap.Fill(tab);
                string typestring = "";
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    typestring = (string)r[2];
                    if (!typestring.Contains("BLOB"))
                    {
                        names.Add((string)r[1]);
                    }

                }
            }
            if (!wasopen) { Close(); }
            return names.ToArray();
        }
        public Type[] ColumnTypes()
        {
            bool wasopen = _isOpen;
            if (!_isOpen) { Open(); }
            List<Type> types = new List<Type>();
            string typestring = "";
            using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand("PRAGMA table_info([" + _tableName + "])", _connection))
            {
                System.Data.SQLite.SQLiteDataAdapter adap = new System.Data.SQLite.SQLiteDataAdapter(cmd);
                System.Data.DataTable tab = new System.Data.DataTable();
                adap.Fill(tab);
                foreach (System.Data.DataRow r in tab.Rows)
                {
                    typestring = (string)r[2];
                    if (typestring.Contains("INT"))
                    {
                        switch (typestring)
                        {
                            case "INT1":
                            case "TINYINT":
                                types.Add(typeof(byte));
                                break;
                            case "INT2":
                            case "SMALLINT":
                                types.Add(typeof(Int16));
                                break;
                            case "INT4":
                            case "MEDIUMINT":
                                types.Add(typeof(Int32));
                                break;
                            default:
                                types.Add(typeof(Int64));
                                break;
                        }
                    }
                    else if (typestring.Contains("CHAR") | typestring.Contains("CLOB") | typestring.Contains("TEXT"))
                    {
                        types.Add(typeof(string));
                    }
                    else if (typestring.Contains("FLOA"))
                    {
                        types.Add(typeof(Single));
                    }
                    else if (typestring.Contains("REAL") | typestring.Contains("DOUB"))
                    {
                        types.Add(typeof(double));
                    }
                    else if (typestring.Contains("BOOL"))
                    {
                        types.Add(typeof(bool));
                    }
                    else if (typestring.Contains("BLOB"))
                    {
                        //types.Add(typeof(byte));
                    }
                    else
                    {
                        types.Add(typeof(byte));
                    }
                }
            }
            if (!wasopen) { Close(); }
            return types.ToArray();
        }
        #endregion
    }
}
