using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class SectionData {
        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        int DistanceRight { get; set; }

        public SectionData() {

        }

    }
}
