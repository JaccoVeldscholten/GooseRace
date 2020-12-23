using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller {
    public static class Data {
        public static Competition comp { get; set; }
        public static Race currentRace { get; set; }

        public delegate void NextRaceEventHandler(object source, NextRaceEventArgs args);
        static public event NextRaceEventHandler NextRaceEvent;

        public static void Init() {           // init Data
            comp = new Competition();
            AddTrack();
            AddParticipant();
        }

        public static void AddParticipant() {
            IParticipant GoosePiet = new Goose(
                "Piet",                      // name
                0,                          // points
                new Wings() {              // equipment
                    Quality = 10,          
                    IsBroken = false,
                    Performance = 100,
                    Speed = 100
                },               
                TeamColors.Blue             // color
            );
            IParticipant GooseBob = new Goose(
                "Bob",                      // name
                0,                          // points
                new Wings() {              // equipment
                                Quality = 10,
                    IsBroken = false,
                    Performance = 100,
                    Speed = 100
                },
                TeamColors.Grey             // color
            );

            IParticipant GooseSjaak = new Goose(
                "Sjaak",                     // name
                0,                          // points
                new Wings() {              // equipment
                    Quality = 10,          
                    IsBroken = false,
                    Performance = 100,
                    Speed = 10
                },
                TeamColors.Red              // color
            );

            IParticipant GooseMarietje = new Goose(
                "Marietje",                    // name
                0,                          // points
                new Wings() {              // equipment
                    Quality = 10,          
                    IsBroken = false,
                    Performance = 100,
                    Speed = 0
                },
                TeamColors.Green            // color
            );

            // Add those gooses to the race
            comp.Participants.Add(GoosePiet);
            comp.Participants.Add(GooseBob);     
            comp.Participants.Add(GooseSjaak);
            comp.Participants.Add(GooseMarietje);

        }

        public static void AddTrack() {
            SectionTypes[] gardenTrackSections = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner
            };

            SectionTypes[] honkTrackSections = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner
            };

            SectionTypes[] townTrackSections = {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner
            };

            Track honkTrack = new Track("Honk Track", honkTrackSections);           // Most simple track      
            Track gardenTrack = new Track("Garden Track", gardenTrackSections);
            Track townTrack = new Track("Town Track", townTrackSections);

            comp.Tracks.Enqueue(honkTrack);
            comp.Tracks.Enqueue(gardenTrack);
            comp.Tracks.Enqueue(townTrack);
        }   

        public static void NextRace() {            
            if (currentRace != null) {
                currentRace.CleanUp();
            }

            Track nextTrack = comp.NextTrack();
            if (nextTrack != null) {
                currentRace = new Race(nextTrack, comp.Participants);
                currentRace.RaceIsFinished += OnRaceIsFinished;
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = currentRace });
                currentRace.StartRace();
            }
            else {
                currentRace = null;
            }
        }

        public static void OnRaceIsFinished(object sender, EventArgs e) {       // fill competition information
            comp.GivePoints(currentRace.getEndResult());
            comp.GiveSectionTime(currentRace.getRaceStatRoundTime());
            comp.GiveSectionSpeed(currentRace.getRaceSectionSpeed());
            comp.GiveTimesBroken(currentRace.getBrokenCounter());
        }
    }


}
