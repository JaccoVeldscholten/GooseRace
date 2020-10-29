﻿using System;
namespace Model {
    public class SectionInformation {

        public Section Section { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public Direction Direction;

        public SectionInformation(Section section, int x, int y, Direction direction) {
            Section = section;
            X = x;
            Y = y;
            Direction = direction;
        }
    }
}