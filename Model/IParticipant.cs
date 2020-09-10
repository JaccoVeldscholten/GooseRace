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
        string Name { get; set; }
        int Points { get; set; }
        IEquipment Equipment { get; set; }
        TeamColors Teamcolor { get; set; }

    }
}
