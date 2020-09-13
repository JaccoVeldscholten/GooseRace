using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model {
    public class Track : Section{
        string Name { get; set; }

        LinkedList<Section> Sections = new LinkedList<Section>();

        public Track(string trackName, SectionTypes[] sectionsRec) {
            Name = trackName;

            foreach (Section.SectionType sec in sectionsRec) {
                Sections.AddLast(sec);
            }

        }

    }
}
