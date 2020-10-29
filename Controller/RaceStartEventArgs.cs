using System;
using System.Collections.Generic;
using System.Text;

namespace Controller {
    public class RaceStartEventArgs : EventArgs {
        public Race Race { get; set; }
    }
}