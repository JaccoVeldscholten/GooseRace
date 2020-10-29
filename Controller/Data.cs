using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using Model;

namespace Controller {
    public static class Data {
        public static Competition comp { get; set; }
        public static Race CurrentRace { get; set; }

        public static event EventHandler<RaceStartEventArgs> NextRaceEventHandler;

        public static void Initialize() {
            comp = new Competition();
            AddParticipants();
            AddTracks();
        }

        static void AddParticipants() {

            comp.Participants.Add(new Goose("Piet", 0, new Wings(10, 10, 10, false), TeamColors.Blue));
            comp.Participants.Add(new Goose("Sjaak", 0, new Wings(10, 10, 10, false), TeamColors.Yellow));
            comp.Participants.Add(new Goose("Marietje", 0, new Wings(10, 10, 10, false), TeamColors.Red));
            comp.Participants.Add(new Goose("Richard", 0, new Wings(10, 10, 10, false), TeamColors.Green));
            comp.Participants.Add(new Goose("Dennis", 0, new Wings(10, 10, 10, false), TeamColors.Grey));
        }

        static void AddTracks() {
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


            Track gardenTrack = new Track("Garden Track", gardenTrackSections);
            Track honkTrack = new Track("Honk Track", honkTrackSections);               // Simple Track
            Track townTrack = new Track("Town Track", townTrackSections);               

            comp.Tracks.Enqueue(gardenTrack);
            comp.Tracks.Enqueue(honkTrack);
            comp.Tracks.Enqueue(townTrack);

        }

        public static void NextRace() {

            if (CurrentRace != null) {
                comp.GivePointsToDriver(CurrentRace.FinishPosition);        // Give points
                NextRaceEventHandler?.Invoke(null, new RaceStartEventArgs() { Race = CurrentRace });
            }

            Race next = new Race(comp.NextTrack(), comp.Participants);
            if (next != null){
                CurrentRace = next;
            }
            else{
                CurrentRace = null;
            }
        }
    }
}
