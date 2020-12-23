using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    public class DataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TrackName { get; set; }


        public void OnGoosesChanged(object source, GoosesChangedEventArgs e)
        {
            TrackName = e.track.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
