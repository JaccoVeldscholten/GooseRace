using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model {
    public class Track : Section {
        public string Name { get; set; }

        public LinkedList<SectionTypes> Sections = new LinkedList<SectionTypes>();


        public Track(string trackName, SectionTypes[] sections) {
            Name = trackName;

            foreach (SectionTypes sec in sections){
                Sections.AddLast(sec);
            }

        }

    }
}
