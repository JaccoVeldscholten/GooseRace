using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Car : IEquipment{
        public int quality;
        public int performance;
        public int speed;
        public bool isbroken;

        public Car() {
            quality = 10;
            performance = 10;
            speed = 10;
            IsBroken = false;
        }

        public int Quality { get => quality; set => quality = value; }
        public int Performance { get => performance; set => performance = value; }
        public int Speed { get => speed; set => speed = value; }
        public bool IsBroken { get => isbroken; set => isbroken = value; }
    }
}
