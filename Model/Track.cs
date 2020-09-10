using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    class Track {
        private string Name { get; set; }

        LinkedList<Section> Sections = new LinkedList<Section>();

        public Track(string name, SectionTypes[] sections) {

        }

    }
}
