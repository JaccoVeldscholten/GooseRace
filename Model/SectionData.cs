using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class SectionData {

        // Left Goose  
        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public DateTime startTimeLeft { get; set; }

        // Right Goose

        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }
        public DateTime startTimeRight { get; set; }


        public SectionData() {

        }

    }
}
