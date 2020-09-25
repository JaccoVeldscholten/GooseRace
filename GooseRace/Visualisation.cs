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
        private static string[] _finishHorizontal = { "----", " 1# ", "2 # ", "----" };
        private static string[] _startGridHorizontal = { "----", " 1] ", "2]  ", "----" };

        private static string[] _straightHorizontal = { "----", "  1 ", " 2  ", "----" };
        private static string[] _straightVertical = { "|  |", "|2 |", "| 1|", "|  |" };

        private static string[] _cornerNorthEast = { " /--", "/1  ", "| 2 ", "|  /" };
        private static string[] _cornerEastSouth =
        {
            @"|  \",
            @"| 1 ",
            @"\2  ",
            @" \--"
        };
        private static string[] _cornerSouthWest =
        {
            "/  |",
            " 1 |",
            "  2/",
            "--/ "
        };
        private static string[] _cornerWestNorth =
{
            @"--\ ",
            @"  1\",
            @" 2 |",
            @"\  |"
        };
        #endregion

        private const int _cursorStartPosX = 30;
        private const int _cursorStartPosY = 10;

        private static int _cPosX = _cursorStartPosX;
        private static int _cPosY = _cursorStartPosY;

        private static Race _currentRace;

        // Race start altijd richting het Oosten
        private static Direction _currentDirection = Direction.East;

        public static void Initialize(Race race) {
            // Setup
            _currentRace = race;
        }


        public static string[] SectionTypeToGraphic(SectionTypes sectionType, Direction direction) {
            return sectionType switch
            {
                SectionTypes.Straight => ((int)direction % 2) switch
                {
                    0 => _straightVertical,
                    1 => _straightHorizontal,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                },
                SectionTypes.LeftCorner => (int)direction switch
                {
                    0 => _cornerWestNorth,
                    1 => _cornerSouthWest,
                    2 => _cornerEastSouth,
                    3 => _cornerNorthEast,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                },
                SectionTypes.RightCorner => (int)direction switch
                {
                    0 => _cornerNorthEast,
                    1 => _cornerWestNorth,
                    2 => _cornerSouthWest,
                    3 => _cornerEastSouth,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
                },
                SectionTypes.StartGrid => _startGridHorizontal,
                SectionTypes.Finish => _finishHorizontal,
                _ => throw new ArgumentOutOfRangeException(nameof(sectionType), sectionType, null)
            };
        }
        public static Direction ChangeDirectionLeft(Direction direction) {


            return direction switch
            {
                Direction.North => Direction.West,
                Direction.East => Direction.North,
                Direction.South => Direction.East,
                Direction.West => Direction.South,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };




        }

        public static Direction ChangeDirectionRight(Direction d) {
            return d switch
            {
                Direction.North => Direction.East,
                Direction.East => Direction.South,
                Direction.South => Direction.West,
                Direction.West => Direction.North,
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
            };
        }



        public static void ChangeCursorToNextPosition() {
            switch (_currentDirection) {
                case Direction.North:
                    _cPosY -= 4;
                    break;
                case Direction.East:
                    _cPosX += 4;
                    break;
                case Direction.South:
                    _cPosY += 4;
                    break;
                case Direction.West:
                    _cPosX -= 4;
                    break;
            }
        }


        public static void DrawSingleSection(Section section) {
            // first determine section string
            string[] sectionStrings = ReplacePlaceHolders(
                SectionTypeToGraphic(section.SectionType, _currentDirection),
                _currentRace.GetSectionData(section).Left, _currentRace.GetSectionData(section).Right
            );

            // print section
            int tempY = _cPosY;
            foreach (string s in sectionStrings) {
                Console.SetCursorPosition(_cPosX, tempY);
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
            ChangeCursorToNextPosition();

        }

        public static void DrawTrack(Track track) {
            // to test, first draw string.
            foreach (Section trackSection in track.Sections) {
                DrawSingleSection(trackSection);
            }
        }


        public static string[] ReplacePlaceHolders(string[] inputStrings, IParticipant leftParticipant, IParticipant rightParticipant) {
            // create returnStrings array
            string[] returnStrings = new string[inputStrings.Length];

            // gather letters from Participants, letter will be a whitespace when participant is null;
            string lP = leftParticipant == null ? " " : leftParticipant.Name.Substring(0, 1).ToUpper();
            string rP = rightParticipant == null ? " " : rightParticipant.Name.Substring(0, 1).ToUpper();

            // replace string 1 and 2 with participants
            for (int i = 0; i < returnStrings.Length; i++) {
                returnStrings[i] = inputStrings[i].Replace("1", lP).Replace("2", rP);
            }

            return returnStrings;
        }



    }
}