﻿using QueryAttack.Model;
using QueryAttack.Services;
using System.ComponentModel;
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

        public async Task<bool> attackStart()
        {
           bool x = await iAttack.StartAttack(connectionService.conn, attackProperties.QuantityOfQueriesToExecute, attackProperties.QueryText);
            return x;
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

            //ONLY FOR TEST - DEVELOPMENT
            attackProperties.QueryText = @"SELECT @@VERSION";
            connProperties.ServerName = @"MARCIN\SQLEXPRESS";
            connProperties.DatabaseName = "CS";
            connProperties.User = "sa";
            connProperties.Password = "dastest";

            ExecuteCommand = new CommandHandler(Execute, () => true);
            CreateConnectionStringCommand = new CommandHandler(ConnectToDatabase, () => true);
            DisconnectAndResetCommand = new CommandHandler(DisconnectAndReset, () => true);
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
