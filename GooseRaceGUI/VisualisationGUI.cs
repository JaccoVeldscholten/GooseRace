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

        public enum GoosePos {
            Left,
            Right
        }

        public struct StartPos {
            public int coordX;
            public int coordY;
        }


        public static TrackSize _trackSize;
        public static StartPos _startPos;

        private static Race _race;

        #region IMGconstants
        const int marginTopBottom = 0;      // Margin (to start drawing)
        const int marginRightLeft = 0;      // Margin (to start drawing)
        const int sectionIMGSize = 600;     // Offset to make track bigger
        const int GooseIMGSize = 300;       // Offset to make goose bigger
        const int trackOffset = 6;          // Offset to scale to diffrent Windows
        #endregion

        #region graphics
        const string CornerLeftHorizontal = ".\\Assets\\Track\\CornerLeftHorizontal.png";
        const string CornerLeftVertical = ".\\Assets\\Track\\CornerLeftVertical.png";
        const string CornerRightHorizontal = ".\\Assets\\Track\\CornerRightHorizontal.png";
        const string CornerRightVertical = ".\\Assets\\Track\\CornerRightVertical.png";
        const string Finish = ".\\Assets\\Track\\Finish.png";
        const string Start = ".\\Assets\\Track\\Start.png";
        const string TrackHorizontal = ".\\Assets\\Track\\TrackHorizontal.png";
        const string TrackVertical = ".\\Assets\\Track\\TrackVertical.png";
        const string GrassTile = ".\\Assets\\Track\\Grass_Tile.png";
        const string WaterTile = ".\\Assets\\Track\\Water.png";

        // North Facing Gooses
        const string Blue_North = ".\\Assets\\Gooses\\North\\Goose_Blue.png";
        const string Grey_North = ".\\Assets\\Gooses\\North\\Goose_Grey.png";
        const string Red_North = ".\\Assets\\Gooses\\North\\Goose_Red.png";
        const string Yellow_North = ".\\Assets\\Gooses\\North\\Goose_Yellow.png";
        const string Green_North = ".\\Assets\\Gooses\\North\\Goose_Orange.png";

        // East Facing Gooses
        const string Blue_East = ".\\Assets\\Gooses\\East\\Goose_Blue.png";
        const string Grey_East = ".\\Assets\\Gooses\\East\\Goose_Grey.png";
        const string Red_East = ".\\Assets\\Gooses\\East\\Goose_Red.png";
        const string Yellow_East = ".\\Assets\\Gooses\\East\\Goose_Yellow.png";
        const string Green_East = ".\\Assets\\Gooses\\East\\Goose_Orange.png";

        // South Facing Gooses
        const string Blue_South = ".\\Assets\\Gooses\\South\\Goose_Blue.png";
        const string Grey_South = ".\\Assets\\Gooses\\South\\Goose_Grey.png";
        const string Red_South = ".\\Assets\\Gooses\\South\\Goose_Red.png";
        const string Yellow_South = ".\\Assets\\Gooses\\South\\Goose_Yellow.png";
        const string Green_South = ".\\Assets\\Gooses\\South\\Goose_Orange.png";

        // West Facing Gooses
        const string Blue_West = ".\\Assets\\Gooses\\West\\Goose_Blue.png";
        const string Grey_West = ".\\Assets\\Gooses\\West\\Goose_Grey.png";
        const string Red_West = ".\\Assets\\Gooses\\West\\Goose_Red.png";
        const string Yellow_West = ".\\Assets\\Gooses\\West\\Goose_Yellow.png";
        const string Green_West = ".\\Assets\\Gooses\\West\\Goose_Orange.png";

        const string BrokenGoose = ".\\Assets\\Gooses\\BrokenGoose.png";
        #endregion


        public static void Init(Race race) {
            // Pass over the Race Object
            _race = race;
        }

        private static Direction CalcDirLeftTurn(Direction d) => (Direction)(((uint)d - 1) % 4);
        private static Direction CalcDirRightTurn(Direction d) => (Direction)(((uint)d + 1) % 4);

        private static Direction DirectionLeftOrRightTurn
            (Direction d, SectionTypes st) {
            return st switch
            {
                SectionTypes.LeftCorner => CalcDirLeftTurn(d),
                SectionTypes.RightCorner => CalcDirRightTurn(d),
                _ => d
            };
        }

        private static void NextPosition(ref int curX, ref int curY, Direction direction) {
            switch (direction) {
                case Direction.North: curY--; break;
                case Direction.East: curX++; break;
                case Direction.South: curY++; break;
                case Direction.West: curX--; break;
            }
        }

        private static void SetTrackSize(Track track, Direction curDir) {

            int curX = marginTopBottom;
            int curY = marginRightLeft;

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

            _trackSize.width = ((positionsX.Max() +  trackOffset) - positionsX.Min()) * sectionIMGSize;
            _trackSize.height = ((positionsY.Max() + trackOffset) - positionsY.Min()) * sectionIMGSize;

            _startPos.coordX = (marginTopBottom - positionsX.Min()) * sectionIMGSize;
            _startPos.coordY = (marginRightLeft - positionsY.Min()) * sectionIMGSize;

        }

        private static string GetSectionFile(SectionTypes sec, Direction dir) {
            return sec switch
            {
                SectionTypes.Straight => ((int)dir % 2) switch
                {
                    0 => TrackVertical,
                    1 => TrackHorizontal,
                    _ => throw new ArgumentException(String.Format("{0} is unable calculcate Direction", dir)),
                },
                SectionTypes.LeftCorner => (int)dir switch
                {
                    0 => CornerLeftVertical,
                    1 => CornerLeftHorizontal,
                    2 => CornerRightHorizontal,
                    3 => CornerRightVertical,
                    _ => throw new ArgumentException(String.Format("{0} This section does not exist: ", dir)),
                },
                SectionTypes.RightCorner => (int)dir switch
                {
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

        private static string TeamColorToFilename(TeamColors color, Direction dir) => dir switch
        {
            Direction.North => color switch {
                TeamColors.Blue => Blue_North,
                TeamColors.Grey => Grey_North,
                TeamColors.Red => Red_North,
                TeamColors.Yellow => Yellow_North,
                TeamColors.Green => Green_North,
                _ => throw new ArgumentException(String.Format("{0} This color does not exist: ", color))
            },
            Direction.East => color switch {
                TeamColors.Blue => Blue_East,
                TeamColors.Grey => Grey_East,
                TeamColors.Red => Red_East,
                TeamColors.Yellow => Yellow_East,
                TeamColors.Green => Green_East,
                _ => throw new ArgumentException(String.Format("{0} This color does not exist: ", color))
            },
            Direction.South => color switch {
                TeamColors.Blue => Blue_South,
                TeamColors.Grey => Grey_South,
                TeamColors.Red => Red_South,
                TeamColors.Yellow => Yellow_South,
                TeamColors.Green => Green_South,
                _ => throw new ArgumentException(String.Format("{0} This color does not exist: ", color))
            },
            Direction.West => color switch {
                TeamColors.Blue => Blue_West,
                TeamColors.Grey => Grey_West,
                TeamColors.Red => Red_West,
                TeamColors.Yellow => Yellow_West,
                TeamColors.Green => Green_West,
                _ => throw new ArgumentException(String.Format("{0} This color does not exist: ", color))
            },
            _ => throw new ArgumentException(String.Format("{0} Direction does not exist: ", dir))
        };

        private static (int posX, int posY) SetGooseOffset(GoosePos goosePos, Direction curDir) => goosePos switch
        {
            // Some Offsets to print out the gooses (and let Left stand first onto Right)
            GoosePos.Left when curDir == Direction.North => (-100, 100),    // Coords X & Y
            GoosePos.Left when curDir == Direction.East => (100, -100), 
            GoosePos.Left when curDir == Direction.South => (340, 100), 
            GoosePos.Left when curDir == Direction.West => (-100, 340), 
            GoosePos.Right when curDir == Direction.North => (180, -100), 
            GoosePos.Right when curDir == Direction.East => (-100, 180),
            GoosePos.Right when curDir == Direction.South => (100, -100),
            GoosePos.Right when curDir == Direction.West => (100, 100)  
        };

        private static void DrawBrokenGooseOnSection(Graphics g, int x, int y) {
            g.DrawImage(ImageCache.GetBitmap(BrokenGoose), x, y, GooseIMGSize, GooseIMGSize);
        }

        private static void DrawGoosesOnSection(int posX, int posY, Direction curDir, Graphics graphics, Section sec) {
           
            IParticipant leftParticipant = Data.CurrentRace.GetSectionData(sec).Left;
            IParticipant rightParticipant = Data.CurrentRace.GetSectionData(sec).Right;

            if (leftParticipant != null) {
                (int x, int y) = SetGooseOffset(GoosePos.Left, curDir); 
              
                if (!leftParticipant.Equipment.IsBroken) {
                    DrawSingleGoose(leftParticipant, graphics, curDir, posX + x, posY + y); 
                }
                else {
                    DrawBrokenGooseOnSection(graphics, posX + x, posY + y); 
                }
                    
            }

            if (rightParticipant != null) {
                (int x, int y) = SetGooseOffset(GoosePos.Right, curDir); 
               
                if (!rightParticipant.Equipment.IsBroken) {
                    DrawSingleGoose(rightParticipant, graphics, curDir, posX + x, posY + y); 
                }
                else {
                    DrawBrokenGooseOnSection(graphics, posX + x, posY + y); 
                }
            }
        }

        private static void DrawSingleGoose(IParticipant participant, Graphics g, Direction d, int posX, int posY) {
            Bitmap participantBitmap = ImageCache.GetBitmap(TeamColorToFilename(participant.TeamColor, d));
            g.DrawImage(participantBitmap, posX, posY, GooseIMGSize, GooseIMGSize);
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
                DrawGoosesOnSection(posX, posY, curDir, graphics, sec);
                CursorToNextPosition(ref posX, ref posY, curDir);
            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(Canvas);
        }

    }

}
