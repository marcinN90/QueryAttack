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
        private AttackStatus _attackStatus;
        public AttackStatus attackStatus
        {
            get
            {
                return _attackStatus;
            }
        }

        private ConnectionProperties _connProperties;
        public ConnectionProperties connProperties
        {
            get
            {
                return _connProperties;
            }
        }

        private AttackProperties _attackProperties;
        public AttackProperties attackProperties
        {
            get
            {
                return _attackProperties;
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
                attackStart();   
            }
            MessageBox.Show(QueryText);
        }

        public void attackStart()
        {
            for (int i = 0; i < attackProperties.QuantityOfQueriesToExecute; i++)
            {
                SqlCommand comm = new SqlCommand(QueryText, conn);
                comm.ExecuteNonQuery();
                _attackStatus.CounterOfCompletedQueries += 1;
            }
        }
        public void CreateConnectionString()
        {
            CreatedConnectionString = connString.getConnectionString;
            SqlConnectionStringBuilder buildConnString = new SqlConnectionStringBuilder();

            //testy
            //buildConnString.DataSource = "192.168.3.151";
            //buildConnString.InitialCatalog = "alvikstorn";
            //buildConnString.IntegratedSecurity = false;
            //buildConnString.UserID = "sa";
            //buildConnString.Password = "daspeab4";
            //QueryText = "select count(*) from analyzesensorslog";

            buildConnString.DataSource = @"DESKTOP-SLEAS3V\SQL2014";
            buildConnString.InitialCatalog = "CS";
            buildConnString.IntegratedSecurity = false;
            buildConnString.UserID = "sa";
            buildConnString.Password = "maca2bra";
            QueryText = "select @@VERSION";


              //SqlConnection conn = new SqlConnection(connString.getConnectionString);
              conn = new SqlConnection(buildConnString.ConnectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            if (conn.State == ConnectionState.Open)
            {
                connProperties.ConnectionStatus = "Connnected";
            }
            //if (conn.State == ConnectionState.Closed)
            //    MessageBox.Show("Closed");

        }


        private AttackProperties query;
        public MainWindowViewModel()
        {
            _attackStatus  = new AttackStatus();
            _connProperties = new ConnectionProperties();
            _connProperties.ConnectionStatus = "Not Connected";
            _attackProperties = new AttackProperties();

            query = new AttackProperties();            
            ExecuteCommand = new CommandHandler(Execute, () => true);
            CreateConnectionStringCommand = new CommandHandler(CreateConnectionString, () => true);

            connProperties.ServerName = @"DESKTOP-SLEAS3V\SQL2014";
            connProperties.DatabaseName = "CS";
            connProperties.User = "sa";
            connProperties.Password = "maca2bra";
        }

        public string CreatedConnectionString
        {
            get
            {
                return connString.getConnectionString;
            }
            set
            {
                connString.getConnectionString = String.Format("Data Source={0};Initial Catalog ={1}, User ID={2}; Password ={3}; Trusted_Connection=False", connProperties.ServerName, connProperties.DatabaseName, connProperties.User, connProperties.Password); ;
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
