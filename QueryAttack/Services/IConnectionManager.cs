using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Services
{
    interface IConnectionManager
    {
        void createConnection();

        void connectToDatabase();

        void disconnectFromDatabase();
    }
}
