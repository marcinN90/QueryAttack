using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Model
{
    public class AttackStatus : INotifyPropertyChanged
    {
        private int _AttackCounter = 10;
        public int AttackCounter
        {
            get
            {
                return _AttackCounter;
            }
            set
            {
                _AttackCounter = value;
                OnPropertyChanged("AttackCounter");       
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
