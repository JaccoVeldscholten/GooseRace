using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Wings : IEquipment{
        public int quality;
        public int performance;
        public int speed;
        public bool isbroken;

        public Wings(int quality, int performance, int speed, bool isBroken) {
            Quality = quality;
            Performance = performance;
            Speed = speed;
            IsBroken = isBroken;
        }

        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
    }
}
