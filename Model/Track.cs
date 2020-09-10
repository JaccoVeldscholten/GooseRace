using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Track {
        string Name { get; set; }

        LinkedList<Section> Sections = new LinkedList<Section>();

        Track(string name, SectionTypes[] sections) {

        }

    }
}
