using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WorkstationAndon
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

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        private Brush bgColorHarness; 
        public Brush BgColorHarness
        {
            get
            {
                return bgColorHarness;
            }
            set
            {
                bgColorHarness = value;
                OnPropertyChanged("BgColorHarness");
            }
        }

        private Brush bgColorReflector;
        public Brush BgColorReflector
        {
            get
            {
                return bgColorReflector;
            }
            set
            {
                bgColorReflector = value;
                OnPropertyChanged("BgColorReflector");
            }
        }

        private Brush bgColorHousing;
        public Brush BgColorHousing
        {
            get
            {
                return bgColorHousing;
            }
            set
            {
                bgColorHousing = value;
                OnPropertyChanged("BgColorHousing");
            }
        }

        private Brush bgColorLens;
        public Brush BgColorLens
        {
            get
            {
                return bgColorLens;
            }
            set
            {
                bgColorLens = value;
                OnPropertyChanged("BgColorLens");
            }
        }

        private Brush bgColorBulb;
        public Brush BgColorBulb
        {
            get
            {
                return bgColorBulb;
            }
            set
            {
                bgColorBulb = value;
                OnPropertyChanged("BgColorBulb");
            }
        }

        private Brush bgColorBezel;
        public Brush BgColorBezel
        {
            get
            {
                return bgColorBezel;
            }
            set
            {
                bgColorBezel = value;
                OnPropertyChanged("BgColorBezel");
            }
        }

        private int currentHarness;
        public int CurrentHarness
        {
            get
            {
                return currentHarness;
            }
            set
            {
                currentHarness = value;
                OnPropertyChanged("CurrentHarness");
                
            }
        }

        private int currentReflector;
        public int CurrentReflector
        {
            get
            {
                return currentReflector;
            }
            set
            {
                currentReflector = value;
                OnPropertyChanged("CurrentReflector");
            }
        }

        private int currentHousing;
        public int CurrentHousing
        {
            get
            {
                return currentHousing;
            }
            set
            {
                currentHousing = value;
                OnPropertyChanged("CurrentHousing");
            }
        }

        private int currentLens;
        public int CurrentLens
        {
            get
            {
                return currentLens;
            }
            set
            {
                currentLens = value;
                OnPropertyChanged("CurrentLens");
            }
        }

        private int currentBulb;
        public int CurrentBulb
        {
            get
            {
                return currentBulb;
            }
            set
            {
                currentBulb = value;
                OnPropertyChanged("CurrentBulb");
            }
        }

        private int currentBezel;
        public int CurrentBezel
        {
            get
            {
                return currentBezel;
            }
            set
            {
                currentBezel = value;
                OnPropertyChanged("CurrentBezel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {                
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));               
            }
        }

        public Workstation()
        {
            CurrentHarness = 0;
            CurrentReflector = 0;
            CurrentHousing = 0;
            CurrentLens = 0;
            CurrentBulb = 0;
            CurrentBezel = 0;
            Status = "Not Connected";
            BgColorStatus = Brushes.White;
        }

        public Workstation(int harness, int reflector, int housing, int lens, int bulb, int bezel)
        {
            CurrentHarness = harness;
            CurrentReflector = reflector;
            CurrentHousing = housing;
            CurrentLens = lens;
            CurrentBulb = bulb;
            CurrentBezel = bezel;
        }
    }
}
