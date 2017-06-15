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

        private ConnectionService _connectionService;
        public ConnectionService connectionService
        {
            get
            {
                return _connectionService;
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

        private void Execute()
        {
            if (!connectionService.IsConnected())
            {
                MessageBox.Show("Not connected to database");
                return;
            }
            else 
            {
                ThreadStart threadStart = attackStart;
                Thread thread = new Thread(threadStart);
                thread.Start();
            }
        }

        public void attackStart()
        {
            iAttack.StartAttack(connectionService.conn, attackProperties.QuantityOfQueriesToExecute, attackProperties.QueryText);
        }

        public void ConnectToDatabase()
        {
            if (!connectionService.Connect(connProperties.ServerName, connProperties.DatabaseName, connProperties.User, connProperties.Password))
            {
                connProperties.ConnectionStatus = "Problem with connection";
            }
            else
            {
                connProperties.ConnectionStatus = "Connnected";
            }    
        }

        public void DisconnectAndReset()
        {
            connProperties.ResetProperties();
            if (!connectionService.IsConnected())
            {
                return;
            }
                _connProperties.ConnectionStatus = "Not Connected";
        }

        public MainWindowViewModel()
        {
            _connProperties = new ConnectionProperties();
            _attackProperties = new AttackProperties();
            _connectionService = new ConnectionService();
            attackProperties.QueryText = @"SELECT @@VERSION"; //test

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
