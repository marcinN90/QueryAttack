using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Model
{
    public class AttackProperties : INotifyPropertyChanged
    {
        private string _QueryText { get; set; }
        public string QueryText
        {
            get
            {
                return _QueryText;
            }
            set
            {
                _QueryText = value;
                OnPropertyChanged("QueryText");
            }
        }

        private int _QuantityOfQueriesToExecute { get; set; }
        public int QuantityOfQueriesToExecute
        {
            get
            {
                return _QuantityOfQueriesToExecute;
            }
            set
            {
                _QuantityOfQueriesToExecute = value;
                OnPropertyChanged("QuantityOfQueriesToExecute");
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
