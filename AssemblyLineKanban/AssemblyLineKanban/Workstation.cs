/*
* FILE: Workstation.cs
* PROJECT: PROG3070 - Final Project
* PROGRAMMERS: TRAN PHUOC NGUYEN LAI, SON PHAM HOANG
* FIRST VERSION: 12/17/2020
* DESCRIPTION: This file includes the information of a workstation.
*/

using System.ComponentModel;
using System.Windows.Media;

namespace AssemblyLineKanban
{
    class Workstation : INotifyPropertyChanged
    {
        private Brush bgColorStatus;
        public Brush BgColorStatus
        {
            get
            {
                return bgColorStatus;
            }
            set
            {
                bgColorStatus = value;
                OnPropertyChanged("BgColorStatus");
            }
        }

        private string workstationLabel;
        public string WorkstationLabel
        {
            get
            {
                return workstationLabel;
            }
            set
            {
                workstationLabel = value;
                OnPropertyChanged("WorkstationLabel");
            }
        }

        private string workstationStatus;
        public string WorkstationStatus
        {
            get
            {
                return workstationStatus;
            }
            set
            {
                workstationStatus = value;
                OnPropertyChanged("WorkstationStatus");
            }
        }

        private int orderTarget;
        public int OrderTarget
        {
            get
            {
                return orderTarget;
            }
            set
            {
                orderTarget = value;
                OnPropertyChanged("OrderTarget");
            }
        }

        private int produced;
        public int Produced
        {
            get
            {
                return produced;
            }
            set
            {
                produced = value;
                OnPropertyChanged("Produced");
            }
        }

        private int passed;
        public int Passed
        {
            get
            {
                return passed;
            }
            set
            {
                passed = value;
                OnPropertyChanged("Passed");
            }
        }

        private int failed;
        public int Failed
        {
            get
            {
                return failed;
            }
            set
            {
                failed = value;
                OnPropertyChanged("Failed");
            }
        }

        private double yield;
        public double Yield
        {
            get
            {
                return yield;
            }
            set
            {
                yield = value;
                OnPropertyChanged("Yield");
            }
        }

        public Workstation()
        {
            BgColorStatus = Brushes.White;
            WorkstationLabel = "Workstation";
            WorkstationStatus = "Inactive";
            OrderTarget = 0;
            Produced = 0;
            Passed = 0;
            Failed = 0;
            Yield = 0.0f;
        }
         
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
