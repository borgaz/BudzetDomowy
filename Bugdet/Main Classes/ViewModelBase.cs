using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Main_Classes
{
    // pomysl byl taki, zeby nie powtarzac kodu i dziedziczyc w kilku miejscach ViewModelBase, zamiast implementowac INotifyPropertyChanged, 
    // ale zapomnialem, ze tutaj nie ma wielodziedziczenia, a kilka klad dziedziczy juz po Window... Na razie zostawiam, bo moze cos zaradze na to ;P
    abstract public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
