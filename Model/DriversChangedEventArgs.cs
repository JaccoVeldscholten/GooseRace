using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GoosesChangedEventArgs : EventArgs {
        public Track Track { get; set; }
    }
}
