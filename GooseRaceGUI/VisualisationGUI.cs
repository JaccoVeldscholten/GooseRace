using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Controller;
using Model;
using RaceGui;

namespace RaceGUI {
    class VisualisationGUI {

        public enum Direction {
            North,
            East,
            South,
            West
        }

        public struct TrackSize {
            public int width;
            public int height;
        }

        public struct StartPos {
            public int coordX;
            public int coordY;
        }

        internal enum GoosePos {
            Left,
            Right
        }

        public static TrackSize _trackSize;
        public static StartPos _startPos;

        private static Race _race;

        #region IMGconstants
        const int marginTopBottom = 10;
        const int marginRightLeft = 10;
        const int sectionIMGSize = 300;
        const int GooseIMGSize = 64;
        #endregion

        #region graphics
        const string CornerLeftHorizontal = ".\\Assets\\CornerLeftHorizontal.png";
        const string CornerLeftVertical = ".\\Assets\\CornerLeftVertical.png";
        const string CornerRightHorizontal = ".\\Assets\\CornerRightHorizontal.png";
        const string CornerRightVertical = ".\\Assets\\CornerRightVertical.png";
        const string Finish = ".\\Assets\\Finish.png";
        const string Start = ".\\Assets\\Start.png";
        const string TrackHorizontal = ".\\Assets\\TrackHorizontal.png";
        const string TrackVertical = ".\\Assets\\TrackVertical.png";
        const string GrassTile = ".\\Assets\\Grass_Tile.png";
        const string WaterTile = ".\\Assets\\Water.png";

        const string Blue = ".\\Assets\\Goose_Blue.png";
        const string Grey = ".\\Assets\\Goose_Grey.png";
        const string Red = ".\\Assets\\Goose_Red.png";
        const string Yellow = ".\\Assets\\Goose_Yellow.png";
        const string Green = ".\\Assets\\Goose_Orange.png";

        const string Fire = ".\\Assets\\Fire.png";
        #endregion

        
        public static void Init(Race race) {
            // Pass over the Race Object
            _race = race;
        }

        private static Direction CalcDirLeftTurn(Direction d) => (Direction)(((uint)d - 1) % 4);
        private static Direction CalcDirRightTurn(Direction d) => (Direction)(((uint)d + 1) % 4);

        private static Direction DirectionLeftOrRightTurn
            (Direction d, SectionTypes st) {
            return st switch{
                SectionTypes.LeftCorner =>  CalcDirLeftTurn(d),
                SectionTypes.RightCorner => CalcDirRightTurn(d),
                _ => d
            };
        }

        private static void NextPosition(ref int curX, ref int curY, Direction direction) {
            switch (direction) {
                case Direction.North: curY--; break;
                case Direction.East:  curX++; break;
                case Direction.South: curY++; break;
                case Direction.West:  curX--; break;
            }
        }

        private static void SetTrackSize(Track track, Direction curDir) {
           
            int curX = marginTopBottom;
            int curY = marginTopBottom;

            List<int> positionsX = new List<int>();
            List<int> positionsY = new List<int>();

            foreach (Section sec in track.Sections) {

                positionsX.Add(curX);
                positionsY.Add(curY);

                if (sec.SectionType == SectionTypes.LeftCorner) {
                    curDir = CalcDirLeftTurn(curDir);
                }
                else if (sec.SectionType == SectionTypes.RightCorner) {
                    curDir = CalcDirRightTurn(curDir);
                }

                NextPosition(ref curX, ref curY, curDir);
            }

            _trackSize.width  = ((positionsX.Max() + 1) - positionsX.Min()) * sectionIMGSize;
            _trackSize.height = ((positionsY.Max() + 1) - positionsY.Min()) * sectionIMGSize;

            _startPos.coordX = (marginTopBottom - positionsX.Min()) * sectionIMGSize;
            _startPos.coordY = (marginTopBottom - positionsY.Min()) * sectionIMGSize;

        }

        private static string GetSectionFile(SectionTypes sec, Direction dir) {
            return sec switch {
                SectionTypes.Straight => ((int)dir % 2) switch {
                    0 => TrackVertical,
                    1 => TrackHorizontal,
                    _ => throw new ArgumentException(String.Format("{0} is unable calculcate Direction", dir)),
                },
                SectionTypes.LeftCorner => (int)dir switch {
                    0 => CornerLeftVertical, 
                    1 => CornerLeftHorizontal, 
                    2 => CornerRightHorizontal, 
                    3 => CornerRightVertical,
                    _ => throw new ArgumentException(String.Format("{0} This section does not exist: ", dir)),
                },
                SectionTypes.RightCorner => (int)dir switch {
                    0 => CornerRightVertical, 
                    1 => CornerLeftVertical,
                    2 => CornerLeftHorizontal, 
                    3 => CornerRightHorizontal,
                    _ => throw new ArgumentException(String.Format("{0} This section does not exist: ", dir)),
                },
                SectionTypes.StartGrid => Start,
                SectionTypes.Finish => Finish,
                _ => throw new ArgumentException(String.Format("{0} This section does not exist: ", dir)),
            };
        }

        private static void CursorToNextPosition(ref int xPos, ref int yPos, Direction direction) {
            switch (direction) {
                case Direction.North: yPos -= sectionIMGSize; break;
                case Direction.East: xPos += sectionIMGSize; break;
                case Direction.South: yPos += sectionIMGSize; break;
                case Direction.West: xPos -= sectionIMGSize; break;
            }
        }


        //########## Particpants #############//


        private static (int x, int y) GetParticipantOffset(GoosePos goosePos, Direction currentDirection) => goosePos switch
        {
            GoosePos.Left when currentDirection == Direction.North => (60, 80), // side to side: 60, 96
            GoosePos.Left when currentDirection == Direction.East => (112, 60), // side to side: 96, 60
            GoosePos.Left when currentDirection == Direction.South => (132, 112), // side to side: 132, 96
            GoosePos.Left when currentDirection == Direction.West => (80, 132), // side to side: 96, 132
            GoosePos.Right when currentDirection == Direction.North => (132, 112), // side to side: 132, 96
            GoosePos.Right when currentDirection == Direction.East => (80, 132), // side to side: 96, 132
            GoosePos.Right when currentDirection == Direction.South => (60, 80), // side to side: 60, 96
            GoosePos.Right when currentDirection == Direction.West => (112, 60), // side to side: 96, 60
            _ => (0, 0) // default
        };

        private static void DrawGoosesOnGUI(int Posx, int posY, Direction curDir, Graphics graphics, Section sec) {
            // look for participants
            IParticipant leftParticipant = _race.GetSectionData(section).Left;
            IParticipant rightParticipant = _race.GetSectionData(section).Right;

            if (leftParticipant != null) {
                (int x, int y) = GetParticipantOffset(Side.Left, currentDirection); // get x&y offset for participant
                DrawParticipantOnCoord(leftParticipant, g, currentDirection, xPos + x, yPos + y); // draw participant
                if (leftParticipant.Equipment.IsBroken)
                    DrawBrokenImageOnCoord(g, xPos + x, yPos + y); // draw broken image on top of participant if participant is broken.
            }

            if (rightParticipant != null) {
                (int x, int y) = GetParticipantOffset(Side.Right, currentDirection); // get x&y offset for participant
                DrawParticipantOnCoord(rightParticipant, g, currentDirection, xPos + x, yPos + y); // draw participant
                if (rightParticipant.Equipment.IsBroken)
                    DrawBrokenImageOnCoord(g, xPos + x, yPos + y); // draw broken image on top of participant if participant is broken.
            }
        }




        private static void DrawSingleSection(int xPos, int yPos, ref Direction direction, Graphics g, Section section) {
            var sectionBitmap = ImageCache.GetBitmap(GetSectionFile(section.SectionType, direction));
            g.DrawImage(sectionBitmap, xPos, yPos, sectionIMGSize, sectionIMGSize);
        }

        public static BitmapSource DrawTrack(Track track) {
            Direction curDir = Direction.East;              // Always start from east

            SetTrackSize(track, curDir);
            Bitmap Canvas = ImageCache.CreateEmptyBitmap(_trackSize.width, _trackSize.height);
            Graphics graphics = Graphics.FromImage(Canvas);

            int posX = _startPos.coordX;
            int posY = _startPos.coordY;
  
            foreach (Section sec in track.Sections) {
                DrawSingleSection(posX, posY, ref curDir, graphics, sec);
                curDir = DirectionLeftOrRightTurn(curDir, sec.SectionType);
                DrawGoosesOnGUI(posX, posY, curDir, graphics, sec);
                CursorToNextPosition(ref posX, ref posY, curDir);
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(Canvas);
        }

    }

}
