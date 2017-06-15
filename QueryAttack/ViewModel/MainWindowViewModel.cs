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

        public ICommand ExecuteCommand { get; }
        public ICommand CreateConnectionStringCommand { get; }
        public ICommand DisconnectAndResetCommand { get; }

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
                ThreadStart threadStart = attackStart;
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }

        public void attackStart()
        {
            iAttack.StartAttack(conn, attackProperties.QuantityOfQueriesToExecute, attackProperties.QueryText);
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

        public MainWindowViewModel()
        {
            _connProperties = new ConnectionProperties();
            _attackProperties = new AttackProperties();
            attackProperties.QueryText = @"SELECT @@VERSION"; //test
            attackProperties.QuantityOfQueriesToExecute = 0;

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
