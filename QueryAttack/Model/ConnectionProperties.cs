using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Model
{
    public class ConnectionProperties : INotifyPropertyChanged
    {
        private string _ServerName;
        public string ServerName
        {
            get
            {
                return _ServerName;
            }
            set
            {
                _ServerName = value;
                OnPropertyChanged("ServerName");
            }
        }

        private string _ConnectionStatus;
        public string ConnectionStatus
        {
            get
            {
                return _ConnectionStatus;
            }
            set
            {
                _ConnectionStatus = value;
                OnPropertyChanged("ConnectionStatus");
            }
        }
        private string _DatabaseName;
        public string DatabaseName
        {
            get
            {
                return _DatabaseName;
            }
            set
            {
                _DatabaseName = value;
                OnPropertyChanged("DatabaseName");
            }
        }

        private string _User { get; set; }
        public string User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
                OnPropertyChanged("User");
            }
        }

        private string _Password { get; set; }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged("Password");
            }
        }

        private SqlConnectionStringBuilder _ConnectionString;
        public SqlConnectionStringBuilder ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
        }
        public void SetConnectionString()
        {
            _ConnectionString.DataSource = ServerName;
            _ConnectionString.InitialCatalog = DatabaseName;
            _ConnectionString.IntegratedSecurity = false;
            _ConnectionString.UserID = User;
            _ConnectionString.Password = Password;
            OnPropertyChanged("ConnectionString");
        }
        //testy
        //buildConnString.DataSource = "192.168.3.151";
        //buildConnString.InitialCatalog = "alvikstorn";
        //buildConnString.IntegratedSecurity = false;
        //buildConnString.UserID = "sa";
        //buildConnString.Password = "daspeab4";
        //QueryText = "select count(*) from analyzesensorslog";

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
