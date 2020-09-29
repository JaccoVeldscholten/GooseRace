using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Controller;
using System.Linq;

namespace GooseRace {

    public enum Direction {
        North,
        East,
        South, 
        West
    }

    static class Visualisation {

        // Graphics
        #region graphics
        private static string[] _finishHorizontal =
        {
            "════",
            " 1▓ ", 
            "2 ▓ ",
            "════"
        };
        private static string[] _startGridHorizontal = 
        {
            "════",
            " 1] ",
            "2]  ",
            "════"
        };

        private static string[] _straightHorizontal = 
        {
            "════",
            "  1 ",
            " 2  ",
            "════"
        };
        private static string[] _straightVertical = 
        {
            "║  ║",
            "║2 ║",
            "║ 1║",
            "║  ║"
        };

        private static string[] _cornerNorthEast = 
        {
            "╔═══",
            "║1  ",
            "║ 2 ",
            "║  ╔"
        };
        private static string[] _cornerEastSouth =
        {
            "║  ╚",
            "║ 1 ",
            "║2  ",
            "╚═══"
        };
        private static string[] _cornerSouthWest =
        {
            "╝  ║",
            " 1 ║",
            "  2║",
            "═══╝"
        };
        private static string[] _cornerWestNorth =
{
            "═══╗ ",
            "  1║",
            " 2 ║",
            "╗  ║"
        };
        #endregion

        private const int _cursorStartPosX = 30;
        private const int _cursorStartPosY = 10;

        private static int _cursorPosX = _cursorStartPosX;
        private static int _cursorPosY = _cursorStartPosY;

        private static Race _currentRace;

        // Race start altijd richting het Oosten
        private static Direction _currentDirection = Direction.East;

        public static void Initialize(Race race) {
            _currentRace = race;
        }


        public static string[] DetermineDirSection(SectionTypes sectionType, Direction direction) {

            return sectionType switch
            {
                SectionTypes.Straight => ((int)direction % 2) switch
                {
                    0 => _straightVertical,
                    1 => _straightHorizontal,
                    _ => throw new ArgumentException(String.Format("{0} is unable to change", direction), "direction"),
                },
                SectionTypes.LeftCorner => (int)direction switch
                {
                    0 => _cornerWestNorth,
                    1 => _cornerSouthWest,
                    2 => _cornerEastSouth,
                    3 => _cornerNorthEast,
                    _ => throw new ArgumentException(String.Format("{0} is unable to change", direction), "direction"),
                },
                SectionTypes.RightCorner => (int)direction switch
                {
                    0 => _cornerNorthEast,
                    1 => _cornerWestNorth,
                    2 => _cornerSouthWest,
                    3 => _cornerEastSouth,
                    _ => throw new ArgumentException(String.Format("{0} is unable to change", direction), "direction"),
                },
                SectionTypes.StartGrid => _startGridHorizontal,
                SectionTypes.Finish => _finishHorizontal,
                _ => throw new ArgumentException(String.Format("{0} is unable to change", direction), "direction"),
            };
        }


        public static Direction ChangeDirectionLeft(Direction direction) {

            return direction switch {
                Direction.North => Direction.West,
                Direction.East => Direction.North,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                _ => throw new ArgumentException(String.Format("{0} is unable to change", direction),"direction"),
            };
        }

        public static Direction ChangeDirectionRight(Direction direction) {

            return direction switch {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new ArgumentException(String.Format("{0} is unable to change", direction), "direction"),
            };
        }

        public static void cursorToNextPosition() {
            switch (_currentDirection) {
                case Direction.North:
                    _cursorPosY -= 4;
                    break;
                case Direction.East:
                    _cursorPosX += 4;
                    break;
                case Direction.South:
                    _cursorPosY += 4;
                    break;
                case Direction.West:
                    _cursorPosX -= 4;
                    break;
            }
        }


        public static void DetermineSection(Section section) {
            // Determine what section to print & Replace chars
            string[] sectionStrings = 
                ReplacePlaceHolders(
                    DetermineDirSection(section.SectionType, _currentDirection),       // Determine section to print
                    _currentRace.GetSectionData(section).Left,                         // Left  Grid Particpant
                    _currentRace.GetSectionData(section).Right                         // Right Grid Participant
            );

            // print section
            int tempY = _cursorPosY;
            foreach (string s in sectionStrings) {
                Console.SetCursorPosition(_cursorPosX, tempY);
                Console.Write(s);
                tempY++;
            }

            // flip direction if corner piece
            if (section.SectionType == SectionTypes.RightCorner) {
                _currentDirection = ChangeDirectionRight(_currentDirection);
            }

            else if (section.SectionType == SectionTypes.LeftCorner) {
                _currentDirection = ChangeDirectionLeft(_currentDirection);
            }

            // change cursor position based on current.
            cursorToNextPosition();

        }

        public static string[] ReplacePlaceHolders(string[] DeterminedSections, IParticipant leftParticipant, IParticipant rightParticipant) {
            string[] replacedSections = new string[DeterminedSections.Length];
            string leftParticipantString;
            string rightParticipantString;

            // Get First left letter of Participant Left
            if (leftParticipant == null) { leftParticipantString = " "; }
            else { leftParticipantString = leftParticipant.Name.Substring(0, 1).ToUpper(); }

            // Get First letter of Participant Right
            if (rightParticipant == null) { rightParticipantString = " "; }
            else { rightParticipantString = rightParticipant.Name.Substring(0, 1).ToUpper(); }

            for (int i = 0; i < DeterminedSections.Length; i++) {
                // For each section in the string array
                DeterminedSections[i] = DeterminedSections[i].Replace("1", leftParticipantString).Replace("2", rightParticipantString);
            }

            return replacedSections;
        }


        public static void DrawTrack(Track track) {
            // Print every Section
            foreach (Section trackSection in track.Sections) {
                DetermineSection(trackSection);
            }
        }




    }
}