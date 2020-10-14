using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Competition {

        public List<IParticipant> Participants = new List<IParticipant>();
        public Queue<Track> Tracks = new Queue<Track>();

        // Generic for points
        public RaceInformation<GoosePoints> GoosePoints { get; set; }


        public Competition() {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            GoosePoints = new RaceInformation<GoosePoints>();
        }
        public Track NextTrack() {
            if (Tracks.Count == 0){
                return null;
            }
            else{
                return Tracks.Dequeue();
            }
        }

        public void GivePointsToDriver(Dictionary<int, string> FinishPosition) {
            foreach (KeyValuePair<int, string> position in FinishPosition) {
                int points = position.Key switch {
                    // Binary cause its fun
                    1 => 32,
                    2 => 16,
                    3 => 8,
                    4 => 4,
                    5 => 2,
                    6 => 1,
                    _ => 0,
                };
                GoosePoints.AddItemToList(new GoosePoints(position.Value, points));
            }
        }
    }
}
