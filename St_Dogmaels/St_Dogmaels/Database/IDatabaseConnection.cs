using System;
using System.Collections.Generic;
using System.Text;

namespace St_Dogmaels
{
    public interface IDatabaseConnection
    {
        SQLite.SQLiteConnection DbConnection();
    }
}
