using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Goose : IParticipant {

        public Goose() {

        }

        string IParticipant.Name { get; set; }
        int IParticipant.Points { get; set; }
        public IEquipment Equipment { get; set; }           
        public TeamColors Teamcolor { get; set; }
    }
}
