using QueryAttack.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QueryAttack.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public AttackStatus _attackStatus;

        public  AttackStatus attackStatus
        {
            get
            {
                return _attackStatus;
            }
        }



        public ICommand ExecuteCommand { get; }
        public ICommand CreateConnectionStringCommand { get; }

        SqlConnection conn;
        private void Execute()
        {          
            if (conn == null)
            {
                MessageBox.Show("Not connected to database");
                return;
            }
            if (conn.State == ConnectionState.Closed)
            {
                MessageBox.Show("Not connected to database");
                return;
            }
            if (conn.State == ConnectionState.Open)
            {
                MessageBox.Show("Connected");



                //   conn.Open();
                //  SqlCommand cmd = new SqlCommand(sql, conn);
                // object result = (object)comm.ExecuteScalar();
                for (int i = 0; i < 20; i++)
                {
                    Thread t = new Thread(attackStart);
                    t.Start();
                }       

            }
            MessageBox.Show(QueryText);
        }

        public void attackStart()
        {
            for (int i = 0; i < Interval; i++)
            {
                SqlCommand comm = new SqlCommand(QueryText, conn);
                comm.ExecuteNonQuery();
                executedCounter = i;
                _attackStatus.CounterOfCompletedQueries += 1;
            }
        }
        public void CreateConnectionString()
        {
            CreatedConnectionString = connString.getConnectionString;
            SqlConnectionStringBuilder buildConnString = new SqlConnectionStringBuilder();
            buildConnString.DataSource = "192.168.3.151";
            buildConnString.InitialCatalog = "alvikstorn";
            buildConnString.IntegratedSecurity = false;
            buildConnString.UserID = "sa";
            buildConnString.Password = "daspeab4";
            QueryText = "select count(*) from analyzesensorslog";


            //SqlConnection conn = new SqlConnection(connString.getConnectionString);
            conn = new SqlConnection(buildConnString.ConnectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                MessageBox.Show(Interval + ServerName + DatabaseName + User + Password);   
            }
            if (conn.State == ConnectionState.Open)
                MessageBox.Show("Connected");
            //if (conn.State == ConnectionState.Closed)
            //    MessageBox.Show("Closed");

        }

        private int _executedCounter;
        public int executedCounter
        {
            get
            {
                return _executedCounter;
            }
            set
            {
                _executedCounter = value;
                OnPropertyChanged("executedCounter");
            }
        }
        private Query query;
        public MainWindowViewModel()
        {
            _attackStatus  = new AttackStatus();
           // attackStatus.CounterOfCompletedQueries = 0;

            query = new Query();
            this.Interval = 10;
            executedCounter = 0;
            ExecuteCommand = new CommandHandler(Execute, () => true);
            CreateConnectionStringCommand = new CommandHandler(CreateConnectionString, () => true);

            ServerName = "192.168.3.151";
            DatabaseName = "alvikstorn";
            User = "sa";
            Password = "daspeab4";

        }
        public int Interval
        {
            get
            {
               return query.Interval;
            }
            set
            {
                query.Interval = value;
                OnPropertyChanged("Interval");
            }
        }

        
        public string ServerName
        {
            get
            {
                return connString.ServerName;
            }
            set
            {
                connString.ServerName = value;
                OnPropertyChanged("ServerName");
            }
        }
        public string DatabaseName
        {
            get
            {
                return connString.DatabaseName;
            }
            set
            {
                connString.DatabaseName = value;
                OnPropertyChanged("DatabaseName");
            }
        }
        public string User
        {
            get
            {
                return connString.User;
            }
            set
            {
                connString.User = value;
                OnPropertyChanged("User");
            }
        }
        public string Password
        {
            get
            {
                return connString.Password;
            }
            set
            {
                connString.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public string CreatedConnectionString
        {
            get
            {
                return connString.getConnectionString;
            }
            set
            {
                connString.getConnectionString = String.Format("Data Source={0};Initial Catalog ={1}, User ID={2}; Password ={3}; Trusted_Connection=False", connString.ServerName, DatabaseName, User, Password); ;
                OnPropertyChanged("CreatedConnectionString");
            }
        }

        public string QueryText
        {
            get
            {
                return query.QueryText;
            }
            set
            {
                query.QueryText = value;
                OnPropertyChanged("QueryText");
            }
        }

        public ConnectionProperties connString = new ConnectionProperties();



        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged (string Name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler !=null)
            {
                handler(this, new PropertyChangedEventArgs(Name));
            }
        }
    }
}
