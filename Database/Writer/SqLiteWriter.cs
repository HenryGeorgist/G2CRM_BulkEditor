using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Writer
{
    public class SqLiteWriter
    {
        #region Notes
        #endregion
        #region Fields
        private readonly System.Data.SQLite.SQLiteConnection _connection;
        private readonly string _tableName;
        private bool _isOpen = false;
        private Int64[] _rowIds;
        #endregion
        #region Properties
        public System.Data.SQLite.SQLiteConnection Connection
        {
            get { return _connection; }
        }
        public string TableName
        {
            get { return _tableName; }
        }
        #endregion
        #region Constructors
        public SqLiteWriter(string filepath, string tablename)
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
                            ids.Add((Int64)reader[0]);
                        }
                    }
                }
            }
            Close();
            _rowIds = ids.ToArray();
        }
        #endregion
        #region Voids
        public void UpdateColumn(string ColumnName, Object[] data)
        {
            bool wasOpen = _isOpen;
            if (!_isOpen) { Open(); }
            if (data.Count() == 0) { return; }
            //check if column exists...
            string cmdstring = "UPDATE [" + _tableName + "] SET [" + ColumnName + "]='";
            using (System.Data.SQLite.SQLiteTransaction trans = _connection.BeginTransaction())
            {
                try
                {
                using (System.Data.SQLite.SQLiteCommand cmd = _connection.CreateCommand())
                {
                    cmd.Transaction = trans;
                    for(int i = 0; i < data.Count(); i++)
                    {
                        cmd.CommandText = cmdstring + data[i] + "' WHERE rowid=" + _rowIds[i];
                        cmd.ExecuteNonQuery();
                    }
                }
                trans.Commit();
                }catch(Exception ex)
                {
                    trans.Rollback();
                }

            }
            if (!wasOpen) { Close(); }
        }
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
        #endregion
    }
}
