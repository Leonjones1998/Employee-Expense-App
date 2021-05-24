using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Essentials;
using System.IO;

namespace Coursework.Models
{
    class Database
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var libFolder = FileSystem.AppDataDirectory;
            var dbName = "EmployeeClaim.db1";
            var dbPath = Path.Combine(libFolder, dbName);
            return new SQLiteAsyncConnection(dbPath);
        }
    }
}
