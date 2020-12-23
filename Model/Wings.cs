using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Wings : IEquipment {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }


    }
}
