using QueryAttack.Model;
using QueryAttack.Services;
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
        private Model.Attack _attackStatus;
        public Attack attackStatus
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

        private IAttack _iAttack = new AttackService();
        public IAttack iAttack
        {
            get
            {
                return _iAttack;
            }
        }


        SqlConnection conn;

        public ICommand ExecuteCommand { get; }
        public ICommand CreateConnectionStringCommand { get; }
        public ICommand DisconnectAndResetCommand { get; }

        public void DisconnectAndReset()
        {
            if (conn == null)
            {
                connProperties.ResetProperties();
                return;
            }
            if (conn.State == ConnectionState.Closed)
            {
                connProperties.ResetProperties();
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                _connProperties.ConnectionStatus = "Not Connected";
                connProperties.ResetProperties();
            }
        }


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
                //  aStatus.StartAttack(ref _attackStatus.CounterOfCompletedQueries);
                ThreadStart threadStart = attackStart;
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }

        public void attackStart()
        {
            iAttack.CounterOfCompletedQueries = 0;
            
            for (int i = 0; i < attackProperties.QuantityOfQueriesToExecute; i++)
            {
                SqlCommand comm = new SqlCommand(_attackProperties.QueryText, conn);
                comm.ExecuteNonQuery();
                iAttack.CounterOfCompletedQueries += 1;
            }
        }
        public void ConnectToDatabase()
        {
            connProperties.SetConnectionString();
            conn = new SqlConnection(connProperties.ConnectionString.ConnectionString);
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
        }

        public MainWindowViewModel()
        {
            iAttack.CounterOfCompletedQueries = 0;
            _attackStatus = new Model.Attack();
            _connProperties = new ConnectionProperties();
            _connProperties.ConnectionStatus = "Not Connected";
            _attackProperties = new AttackProperties();
            _attackProperties.QueryText = @"SELECT @@VERSION"; //test

            ExecuteCommand = new CommandHandler(Execute, () => true);
            CreateConnectionStringCommand = new CommandHandler(ConnectToDatabase, () => true);
            DisconnectAndResetCommand = new CommandHandler(DisconnectAndReset, () => true);

            connProperties.ServerName = @"DESKTOP-SLEAS3V\SQL2014";
            connProperties.DatabaseName = "CS";
            connProperties.User = "sa";
            connProperties.Password = "maca2bra";
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
