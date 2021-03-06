﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task BackupAllUserDatabasesAsync(IProgress<ProgressReport> progress)
        {
            try
            {
                int count = 0;
                foreach (string database in await GetAllUserDatabasesAsync())
                {

                    if (progress !=null)
                    {
                        var args = new ProgressReport();
                        args.ProgressPercentage = (count / 20) * 100;
                        args.Text = count.ToString();
                        progress.Report(args);
                    }
                    //int count=0;
                  await  BackupDatabaseAsync(database);

                    //progress.Report(count);
                    count++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
        private async Task BackupDatabaseAsync(string databaseName)
        {
            try
            {
                string filePath = BuildBackupPathwithFilename(databaseName);
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = string.Format("BACKUP DATABASE [{0}] TO DISK = '{1}'", databaseName, filePath);
                    using (var command = new SqlCommand(query, connection))
                    {
                     await Task.Run(() =>  connection.Open());
                      await Task.Run(() =>  command.ExecuteNonQuery());
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
                string filename = string.Format("{0}-{1}.bak", databaseName, DateTime.Now.ToString("yyyy-MM-dd"));

                return Path.Combine(_backupFolderFullPath, filename);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //throw new NotImplementedException();
        }

        private async Task<IEnumerable<string>> GetAllUserDatabasesAsync()
        {
            try
            {
                var databases = new List<string>();
                DataTable databasesTable=null;

                using (var connection = new SqlConnection(_connectionString))
                {
                   await Task.Run(() => connection.Open());
                    await Task.Run(() => databasesTable = connection.GetSchema("Databases"));
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
