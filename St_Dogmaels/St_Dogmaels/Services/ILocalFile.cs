using System;
using System.Collections.Generic;
using System.Text;

namespace St_Dogmaels.Services
{
    public interface ILocalFile
    {
        void WriteAll(string name, string s);
        string ReadAll(string name);
    }
}
