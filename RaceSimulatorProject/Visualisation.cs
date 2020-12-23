using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RaceSimulatorProject {
    public enum TrackDirection {
        N,
        E,
        S,
        W
    }

    static class Visualisation {

        private static TrackDirection trackDirection = TrackDirection.E;       // beginning with standard east position and cursor location
        private static int cursorPositionX = 20;
        private static int cursorPositionY = 20;
        private static Race _currentRace;



        public static void Initialize(Race race) {                          // make new race
            _currentRace = race;
            _currentRace.DriversChanged += OnDriversChanged;
            _currentRace.RaceIsFinished += OnRaceIsFinished;
            Data.NextRaceEvent += OnNextRaceNextRaceEvent;
        }

        public static void DrawTrack(Track track) {                        // draw part by moving cursor
            Track trackToDraw = track;
            
            foreach (Section sec in trackToDraw.Sections) {
                changeCursorPosition();
                drawTrackPart(sec);
            }
        }

        public static void changeCursorPosition() {                         // change cursor to draw new sections
            if (trackDirection == TrackDirection.N) {
                cursorPositionY -= 8;
            } else if (trackDirection == TrackDirection.E) {
                cursorPositionY -= 4;
                cursorPositionX += 4;
            } else if (trackDirection == TrackDirection.S) {
                // do nothing
            } else if (trackDirection == TrackDirection.W) {
                cursorPositionY -= 4;
                cursorPositionX -= 4;
            }
        }

        public static void drawTrackPart(Section section) {
            string[] partToDraw = ShowParticipantsOnTrack(                                            // change placeholders 1 and 2 for empty space or participants first letter
                whichPartToDraw(section), 
                _currentRace.GetSectionData(section).Left, 
                _currentRace.GetSectionData(section).Right
            );
            foreach (string str in partToDraw) {
                Console.SetCursorPosition(cursorPositionX, cursorPositionY);
                Console.WriteLine(str);
                cursorPositionY++;
            }

            ShowBestParticipant();
        }

        public static string[] whichPartToDraw(Section section) {                               // looks which part to draw based on direction
            string[] returnTrackPart = {""};
            if (section.SectionType == SectionTypes.StartGrid) {                                    
                returnTrackPart = _startGridHor;
                trackDirection = TrackDirection.E;
            }

            if (section.SectionType == SectionTypes.Finish) {
                returnTrackPart = _finishHor;
                trackDirection = TrackDirection.E;
            }

            if (section.SectionType == SectionTypes.Straight) {
                if (trackDirection == TrackDirection.E || trackDirection == TrackDirection.W) {
                    returnTrackPart = _finishHorStraight;
                } else if (trackDirection == TrackDirection.N || trackDirection == TrackDirection.S) {
                    returnTrackPart = _finishVertStraight;
                }
                // trackDirection does not have to be changed due to straight
            }

            if (section.SectionType == SectionTypes.LeftCorner) {
                if (trackDirection == TrackDirection.N) {
                    returnTrackPart = _cornerSouthWest;
                    trackDirection = TrackDirection.W;
                } else if (trackDirection == TrackDirection.E) {
                    returnTrackPart = _cornerNorthWest;
                    trackDirection = TrackDirection.N;
                } else if (trackDirection == TrackDirection.S) {
                    returnTrackPart = _cornerNorthEast;
                    trackDirection = TrackDirection.E;
                } else if (trackDirection == TrackDirection.W) {
                    returnTrackPart = _cornerSouthEast;
                    trackDirection = TrackDirection.S;
                }
            }

            if (section.SectionType == SectionTypes.RightCorner) {
                if (trackDirection == TrackDirection.N) {
                    returnTrackPart = _cornerSouthEast;
                    trackDirection = TrackDirection.E;
                } else if (trackDirection == TrackDirection.E) {
                    returnTrackPart = _cornerSouthWest;
                    trackDirection = TrackDirection.S;
                } else if (trackDirection == TrackDirection.S) {
                    returnTrackPart = _cornerNorthWest;
                    trackDirection = TrackDirection.W;
                } else if (trackDirection == TrackDirection.W) {
                    returnTrackPart = _cornerNorthEast;
                    trackDirection = TrackDirection.N;
                }
            }

            return returnTrackPart;
        }

        public static string[] ShowParticipantsOnTrack(string[] trackPart, IParticipant pLeft, IParticipant pRight) {           // replace numbers with participant first letter
            string participantLeft = " ";
            string participantRight = " ";
            string[] returnTrackPart = new string[trackPart.Length];
            if (pLeft != null) {                                                // if participants is available, take first letter
                if (pLeft.Equipment.IsBroken) {                     // show broken participant
                    participantLeft = "x";
                } else {
                    participantLeft = pLeft.Name.Substring(0, 1);
                }
            }
            if (pRight != null) {
                if (pRight.Equipment.IsBroken) {
                    participantRight = "x";
                } else {
                    participantRight = pRight.Name.Substring(0, 1);
                }
            }

            for (int i = 0; i < returnTrackPart.Length; i++) {
                returnTrackPart[i] = trackPart[i].Replace("1", participantLeft).Replace("2", participantRight);
            }

            return returnTrackPart;
        }

        public static void ShowBestParticipant() {
            Console.SetCursorPosition(0, 50);
            Console.WriteLine($"Participant with astest section time: {_currentRace.GetBestParticipantSectionTime()}");
        }

        public static void OnDriversChanged(object source, DriversChangedEventArgs e) {
            DrawTrack(e.track);
            Console.WriteLine("Test event");
        }

        public static void OnRaceIsFinished(object soure, EventArgs e) {
            Console.WriteLine("Race Finished!");
            Data.NextRace();
        }

        public static void OnNextRaceNextRaceEvent(object sender, NextRaceEventArgs e) {
            Initialize(e.race);
            DrawTrack(_currentRace.Track);
        }


        #region graphics
        private static string[] _startGridHor = {
            "════",
            " 1] ",
            "2]  ",
            "════" };
        private static string[] _finishHor = {
            "════",
            " 1▓ ",
            "2 ▓ ",
            "════" };
        private static string[] _finishHorStraight = {
            "════",
            "  1 ",
            " 2  ",
            "════" };
        private static string[] _finishVertStraight = {
            "║  ║",
            "║ 1║",
            "║2 ║",
            "║  ║" };
        private static string[] _cornerSouthWest = {
            "═══╗",
            "  2║",
            " 1 ║",
            "╗  ║" };
        private static string[] _cornerSouthEast = {
            "╔═══",
            "║2  ",
            "║ 1 ",
            "║  ╔" };
        private static string[] _cornerNorthWest = {
            "╝  ║",
            " 1 ║",
            "  2║",
            "═══╝" };
        private static string[] _cornerNorthEast = {
            "║  ╚",
            "║ 1 ",
            "║2  ",
            "╚═══" };
        
        #endregion
    }
}
