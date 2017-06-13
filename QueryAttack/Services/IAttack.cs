using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Services
{
    interface IAttack
    {
        void StartAttack();

        void CancelAttack();
    }
}
