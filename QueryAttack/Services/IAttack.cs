using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Services
{
    public interface IAttack
    {
        int CounterOfCompletedQueries { get; set; }
        //public int CounterOfCompletedQueries;
        //void StartAttack();

        //void CancelAttack();
    }
}
