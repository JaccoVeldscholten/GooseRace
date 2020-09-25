using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public enum TeamColors {
        Red,
        Green,
        Yellow,
        Grey,
        Blue
    }

    public interface IParticipant {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
    }
}
