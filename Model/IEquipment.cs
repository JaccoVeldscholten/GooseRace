using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    interface IEquipment {
        int Quality { get; set; }
        int Performance { get; set; }
        int Speed { get; set; }
       bool isBroken { get; set; }
    }
}
