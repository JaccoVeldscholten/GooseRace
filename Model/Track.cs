using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model {
    public class Track {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public LinkedList<Section> arrayToLinkedList(SectionTypes[] sectionTypes) {

            LinkedList<Section> sections = new LinkedList<Section>();

            foreach (SectionTypes sectionType in sectionTypes) {
                sections.AddLast(new Section(sectionType));
            }

            return sections;
        }

        public Track(string name, SectionTypes[] sections) {
            Name = name;
            Sections = arrayToLinkedList(sections);
        }
    }
}
