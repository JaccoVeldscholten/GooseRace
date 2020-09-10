using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    enum SectionTypes {
        Straight,
        LeftCorner,
        RightCorner,
        StartGrid,
        Finish
    }

    class Section {

        SectionTypes SectionType { get; set; }

        public Section() { }
    }
}
