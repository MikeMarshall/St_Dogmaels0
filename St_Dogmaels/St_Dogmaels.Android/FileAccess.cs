using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(St_Dogmaels.Droid.FileAccess))]
namespace St_Dogmaels.Droid
{
    class FileAccess : St_Dogmaels.Services.ILocalFile
    {
        private string PersonalFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public string ReadAll(string name)
        {
            string filePath = Path.Combine(PersonalFolderPath, name);
            if (File.Exists(filePath)) return File.ReadAllText(filePath);
            return null;
        }

        public void WriteAll(string name, string data)
        {
            string filePath = Path.Combine(PersonalFolderPath, name);
            File.WriteAllText(filePath, data);
        }
    }
}