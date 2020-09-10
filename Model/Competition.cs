using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    class Competition {

        List<IParticipant> Participants = new List<IParticipant>();
        Queue<Track> Tracks = new Queue<Track>();


        public Competition() {}
        public void NextTrack() {

        }
    }
}
