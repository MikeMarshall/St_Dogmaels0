
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using St_Dogmaels.Droid;
using System.IO;


[assembly: Xamarin.Forms.Dependency(typeof(DatabaseConnection_Android))]
namespace St_Dogmaels.Droid
{
    public class DatabaseConnection_Android : IDatabaseConnection
    {
        public SQLiteConnection DbConnection()
        {
            var dbName = "BuildingsDb.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
            return new SQLiteConnection(path);
        }
    }
}