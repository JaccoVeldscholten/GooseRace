using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller {
    public static class Data {
        public static Competition Comp { get; set; }
        public static Race CurrentRace { get; set; }

        public delegate void NextRaceEventHandler(object source, NextRaceEventArgs args);
        static public event NextRaceEventHandler NextRaceEvent;

        public static void Init() {           // init Data
            Comp = new Competition();
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

            IParticipant GooseArjan = new Goose(
                "Arjan",                    // name
                0,                          // points
                new Wings()
                {   // equipment
                    Quality = 10,
                    IsBroken = false,
                    Performance = 100,
                    Speed = 0
                },
                TeamColors.Yellow            // color
            );

            // Add those gooses to the race
            Comp.Participants.Add(GoosePiet);
            Comp.Participants.Add(GooseBob);     
            Comp.Participants.Add(GooseSjaak);
            Comp.Participants.Add(GooseMarietje);
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
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.RightCorner
            };

            Track honkTrack = new Track("Honk Track", honkTrackSections);           // Most simple track      
            Track gardenTrack = new Track("Garden Track", gardenTrackSections);
            Track townTrack = new Track("Town Track", townTrackSections);
           
            Comp.Tracks.Enqueue(gardenTrack);
            Comp.Tracks.Enqueue(honkTrack);
            Comp.Tracks.Enqueue(townTrack);
        }   

        public static void NextRace() {            
            if (CurrentRace != null) {
                CurrentRace.CleanUp();
            }

            Track nextTrack = Comp.NextTrack();
            if (nextTrack != null) {
                CurrentRace = new Race(nextTrack, Comp.Participants);
                CurrentRace.RaceIsFinished += OnRaceIsFinished;
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = CurrentRace });
                CurrentRace.StartRace();
            }
            else {
                CurrentRace = null;
            }
        }

        public static void OnRaceIsFinished(object sender, EventArgs e) {       
            // Give points & stats to gooses
            Comp.GivePoints(CurrentRace.GetEndResult());
            Comp.GiveSectionTime(CurrentRace.GetRaceStatRoundTime());
            Comp.GiveSectionSpeed(CurrentRace.GetRaceSectionSpeed());
            Comp.GiveTimesBroken(CurrentRace.GetwingsLostCounter());
        }
    }


}
