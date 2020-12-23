using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Track : Section {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sections) {
            Name = name;
            Sections = arrayToLinkedList(sections);
        }

        public LinkedList<Section> arrayToLinkedList(SectionTypes[] sections) {
            LinkedList<Section> newLinkedList = new LinkedList<Section>();
            foreach (SectionTypes secType in sections) {
                newLinkedList.AddLast(new Section(secType));
            }
            return newLinkedList;

        }
    }
}
