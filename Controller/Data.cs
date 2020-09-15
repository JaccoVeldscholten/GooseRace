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
            addTracks();
        }

        static void AddParticipants() {
            IParticipant goosePiet      = new Goose("Piet", new Wings(), TeamColors.Red);
            IParticipant gooseSjaak     = new Goose("Sjaak", new Wings(), TeamColors.Yellow);
            IParticipant gooseMarietje  = new Goose("Marietje", new Wings(), TeamColors.Blue);

            comp.Participants.Add(goosePiet);
            comp.Participants.Add(gooseSjaak);
            comp.Participants.Add(gooseMarietje);
        }

        static void addTracks() {

            SectionTypes[] honkTrackSections = {
                SectionTypes.StartGrid,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            };

            SectionTypes[] gardenTrackSections = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            };


            SectionTypes[] townTrackSections = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
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
