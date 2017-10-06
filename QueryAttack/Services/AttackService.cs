using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Services
{
    public class AttackService : IAttack, INotifyPropertyChanged
    {
        private int _CounterOfCompletedQueries;
        public int CounterOfCompletedQueries
        {
            get
            {
                return _CounterOfCompletedQueries;
            }
            set
            {
                _CounterOfCompletedQueries = value;
                OnPropertyChanged("CounterOfCompletedQueries");
            }
        }

        public bool StartAttack(SqlConnection conn, int QueriesToExcecute, string QueryText)
        {
            try
            {

                CounterOfCompletedQueries = 0;
                for (int i = 0; i < QueriesToExcecute; i++)
                {
                    SqlCommand command = new SqlCommand(QueryText, conn);
                    command.ExecuteNonQuery();
                    CounterOfCompletedQueries += 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string Name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }
        }

    }
}
