using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Car  {
        public int quality;
        public int performance;
        public int speed;
        public bool isbroken;

        public Car() { }

        int Quality { get => quality; set => quality = value; }
        public int Performance { get => performance; set => performance = value; }
        public int Speed { get => speed; set => speed = value; }
        public bool IsBroken { get => isbroken; set => isbroken = value; }

    }
}
