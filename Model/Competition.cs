using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Competition {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        public RaceStats<Points> RaceStatPoints;
        public RaceStats<SectionRoundtime> RaceRoundTime;
        public RaceStats<SectionSpeed> RaceSectionSpeed;
        public RaceStats<BrokenCounter> brokeCounter;
        private int maxPoints = 10;

        public Competition() {                                          // make new competition
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
            RaceStatPoints = new RaceStats<Points>();
            RaceRoundTime = new RaceStats<SectionRoundtime>();
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
                RaceStatPoints.addRaceStatToList(new Points() { name = order[i].Name, points = mP });
                if (mP > 2) {
                    mP -= 2;
                }
            }
        }

        public void GiveSectionTime(RaceStats<SectionRoundtime> rrt) {
            RaceRoundTime = rrt;
        }

        public void GiveSectionSpeed(RaceStats<SectionSpeed> SS) {
            RaceSectionSpeed = SS;
        }

        public void GiveTimesBroken(RaceStats<BrokenCounter> BC) {
            brokeCounter = BC;
        }
    }
}
