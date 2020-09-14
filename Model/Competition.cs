﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Competition {

        public List<IParticipant> Participants = new List<IParticipant>();
        public Queue<Track> Tracks = new Queue<Track>();

        public Competition() {}
        public Track NextTrack() {
            if (Tracks.Count == 0){
                return null;
            }
            else{
                return Tracks.Dequeue();
            }
        }
    }
}
