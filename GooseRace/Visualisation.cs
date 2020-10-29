using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using Controller;
using System.Linq;
using static Controller.Race;

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

        // Race start altijd richting het Oosten
        private static Direction _currentDirection = Direction.East;

        public static void Initialize() {
            Data.CurrentRace.GoosesChanged += OnGooseChanged;
            Data.CurrentRace.NextRace += NextRace;
            
        }


        public static string[] DetermineDirSection(SectionTypes sectionType, Direction direction) {

            return sectionType switch  {
                SectionTypes.Straight => ((int)direction % 2) switch {
                    0 => _straightVertical,
                    1 => _straightHorizontal,
                    _ => throw new ArgumentException(String.Format("{0} is unable to change", direction), "direction"),
                },
                SectionTypes.LeftCorner => (int)direction switch {
                    0 => _cornerWestNorth,
                    1 => _cornerSouthWest,
                    2 => _cornerEastSouth,
                    3 => _cornerNorthEast,
                    _ => throw new ArgumentException(String.Format("{0} is unable to return", direction), "direction"),
                },
                SectionTypes.RightCorner => (int)direction switch {
                    0 => _cornerNorthEast,
                    1 => _cornerWestNorth,
                    2 => _cornerSouthWest,
                    3 => _cornerEastSouth,
                    _ => throw new ArgumentException(String.Format("{0} is unable to return", direction), "direction"),
                },
                SectionTypes.StartGrid => _startGridHorizontal,
                SectionTypes.Finish => _finishHorizontal,
                _ => throw new ArgumentException(String.Format("{0} is unable to return", direction), "direction"),
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

        public static void CursorToNextPosition() {
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
                ReplaceOneAndTwo(
                    DetermineDirSection(section.SectionType, _currentDirection),       // Determine section to print
                    Data.CurrentRace.GetSectionData(section).Left,                         // Left  Grid Particpant
                    Data.CurrentRace.GetSectionData(section).Right                         // Right Grid Participant
            );

            // print section__
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

            CursorToNextPosition();


        }

        public static string[] ReplaceOneAndTwo(string[] inputStrings, IParticipant leftParticipant, IParticipant rightParticipant) {
            string[] returnStrings = new string[inputStrings.Length];
            string leftGoose;
            string rightGoose;

            if (leftParticipant == null) { leftGoose = " ";  }
            else {  leftGoose = leftParticipant.Name.Substring(0, 1).ToUpper(); }

            if (rightParticipant == null) { rightGoose = " "; }
            else { rightGoose = rightParticipant.Name.Substring(0, 1).ToUpper(); }

            for (int i = 0; i < returnStrings.Length; i++) {
                returnStrings[i] = inputStrings[i].Replace("1", leftGoose).Replace("2", rightGoose);
            }

            return returnStrings;
        }


        public static void DrawTrack(Track track) {
            Console.WriteLine($"Current best goose: {Data.comp.GoosePoints.GetHighest()}");
            // Print every Section
            foreach (Section trackSection in track.Sections) {
                DetermineSection(trackSection);
            }
        }


        public static void OnGooseChanged(Object source, GoosesChangedEventArgs e) {
            Console.Clear();
            DrawTrack(e.Track);
        }

        public static void NextRace(Object source, EventArgs e) {
            Initialize();
        }

    }
}