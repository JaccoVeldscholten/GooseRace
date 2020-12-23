using System;
using System.Collections.Generic;
using System.Text;

namespace Controller {
    public class NextRaceEventArgs : EventArgs {    
        public Race Race { get; set; }
    }
}
