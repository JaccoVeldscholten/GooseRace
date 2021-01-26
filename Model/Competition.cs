using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Competition {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        public RaceStats<GoosePoints> RaceStatPoints;
        public RaceStats<GooseSectionTimes> RaceRoundTime;
        public RaceStats<SectionSpeed> RaceSectionSpeed;
        public RaceStats<LostWings> brokeCounter;
        private int maxPoints = 10;

        public Competition() {                                          // make new competition
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            RaceStatPoints = new RaceStats<GoosePoints>();
            RaceRoundTime = new RaceStats<GooseSectionTimes>();
        }
        public Track NextTrack() {                                      // get next track, dequeue current
            Track nextTrack;
            if (Tracks.Count != 0) {
                nextTrack = Tracks.Dequeue();
            } else {
                nextTrack = null;
            }
            return nextTrack;
        }

        public void GivePoints(List<IParticipant> order) {              // give participating participants points, given through the order
            int mP = maxPoints;
            for (int i = 0; i < order.Count; i++) {
                RaceStatPoints.AddRaceStatToList(new GoosePoints() { Name = order[i].Name, Points = mP });
                if (mP > 2) {
                    mP -= 2;
                }
            }
        }

        public void GiveSectionTime(RaceStats<GooseSectionTimes> rrt) {
            RaceRoundTime = rrt;
        }

        public void GiveSectionSpeed(RaceStats<SectionSpeed> SS) {
            RaceSectionSpeed = SS;
        }

        public void GiveTimesBroken(RaceStats<LostWings> BC) {
            brokeCounter = BC;
        }
    }
}
