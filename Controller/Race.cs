using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller {
    public class Race {

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public Track Track { get; set; }
        public List<IParticipant> Participants = new List<IParticipant>(); 
        public DateTime StartTime { get; set; }

        public Race(Track t, List<IParticipant> participant) {
            Track = t;
            Participants = participant;
            _random = new Random(DateTime.Now.Millisecond);
            RandomizeEquipment();
        }

        public SectionData GetSectionData(Section section) {
            if (_positions.ContainsKey(section)){
                return _positions.GetValueOrDefault(section);

            }
            else{
                SectionData data = new SectionData();
                _positions.Add(section, data);
                return (data);

            }
        }

        public void RandomizeEquipment() {

            foreach (Goose goose in Participants) {
                //goose.Equipment.Quality = 10;
                goose.Equipment.Quality = _random.Next(1, 100);
                goose.Equipment.Performance = _random.Next(1, 100);

            }
        }

    }
}