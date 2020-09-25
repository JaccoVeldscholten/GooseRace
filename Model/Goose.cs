using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Goose : IParticipant {

        public Goose(string name, int points, IEquipment equipment, TeamColors teamColor) {
            Name = name;
            Points = points;
            Equipment = equipment;
            TeamColor = teamColor;
        }

        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
    }
}
