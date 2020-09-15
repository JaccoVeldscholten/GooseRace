using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Goose : IParticipant {

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors Teamcolor { get; set; }

        public Goose(String naam, Car carRec, TeamColors teamColor) {
            Name = Name;
            Equipment = carRec;
            Teamcolor = teamColor;
        }

    }
}
