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
        }

        static void AddTracks() {

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

            SectionTypes[] gardenTrackSections = {
                // ToDo
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

            Track honkTrack = new Track("Honk Track", honkTrackSections);
            Track gardenTrack = new Track("Garden Track", gardenTrackSections);
            Track townTrack = new Track("Town Track", townTrackSections);

            comp.Tracks.Enqueue(honkTrack);
            comp.Tracks.Enqueue(gardenTrack);
            comp.Tracks.Enqueue(townTrack);

        }

        public static void NextRace() {
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
