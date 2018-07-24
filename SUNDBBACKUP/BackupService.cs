using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace SUNDBBACKUP
{
    public class BackupService
    {
        private readonly string _connectionString;
        private readonly string _backupFolderFullPath;
        private readonly string[] _systemDatabases = { "master", "tempdb", "model", "msdb" };
        public BackupService(string connectionString, string backupFolderFullPath)
        {
            _connectionString = connectionString;
            _backupFolderFullPath = backupFolderFullPath;
        }
        private void BackupAllUserDatabases()
        {
            try
            {
                foreach (string database in GetAllUserDatabases())
                {
                    BackupDatabase(database);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        public void BackupDatabase(string databaseName)
        {
            try
            {
                string filePath = BuildBackupPathwithFilename(databaseName);
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = string.Format("BACKUP DATABASE [{0}] TO DISK = '{1}'", databaseName, filePath);
                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        private string BuildBackupPathwithFilename(string databaseName)
        {
            try
            {
                string filename = string.Format("{0}-{1}", databaseName, DateTime.Now.ToString("yyyy-MM-dd"));

                return Path.Combine(_backupFolderFullPath, filename);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //throw new NotImplementedException();
        }

        private IEnumerable<string> GetAllUserDatabases()
        {
            try
            {
                var databases = new List<string>();
                DataTable databasesTable;

                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    databasesTable = connection.GetSchema("Databases");
                    connection.Close();
                }
                foreach (DataRow row in databasesTable.Rows)
                {
                    string databaseName = row["database_name"].ToString();//string.Format("{0}-{1}.bak",databaseName)
                    if (_systemDatabases.Contains(databaseName))
                    {
                        continue;
                    }
                    databases.Add(databaseName);
                }
                return databases;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

    }
}
