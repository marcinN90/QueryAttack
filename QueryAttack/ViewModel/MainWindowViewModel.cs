﻿using QueryAttack.Model;
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
                    ThreadStart threadStart = attackStart;
                    Thread thread = new Thread(threadStart);
                    thread.Start();
            }
        }

        public void attackStart()
        {
            _attackStatus.CounterOfCompletedQueries = 0;
            for (int i = 0; i < attackProperties.QuantityOfQueriesToExecute; i++)
            {
                SqlCommand comm = new SqlCommand(_attackProperties.QueryText, conn);
                comm.ExecuteNonQuery();
                _attackStatus.CounterOfCompletedQueries += 1;
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
            _attackStatus  = new AttackStatus();
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
