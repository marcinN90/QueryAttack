using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Services
{
    public interface IAttack
    {
        int CounterOfCompletedQueries { get; set; }
        void StartAttack(SqlConnection conn, int QueriesToExecute, string QueryText);
    }
}
