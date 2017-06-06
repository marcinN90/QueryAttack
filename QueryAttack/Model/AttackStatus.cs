﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryAttack.Model
{
    public class AttackStatus : INotifyPropertyChanged
    {
        private int _CounterOfCompletedQueries;
        public int CounterOfCompletedQueries
        {
            get
            {
                return _CounterOfCompletedQueries;
            }
            set
            {
                _CounterOfCompletedQueries = value;
                OnPropertyChanged("CounterOfCompletedQueries");       
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
