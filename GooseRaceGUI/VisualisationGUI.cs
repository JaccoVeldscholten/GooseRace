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

        public enum GooseSide {
            Left,
            Right
        }

        public struct TrackSize {
           public int width;
           public int height;
        }

        public struct StartPostion {
            public int posX;
            public int posY;
        }

        #region IMGconstants
        const int sectionIMGSize = 200;
        const int GooseIMGSize = 64;
        #endregion

        #region graphics
        const string CornerLeftHorizontal = ".\\Assets\\CornerLeftHorizontal.png";
        const string CornerLeftVertical = ".\\Assets\\CornerLeftVertical.png";
        const string CornerRightHorizontal = ".\\Assets\\CornerRightHorizontal.png";
        const string CornerRightVertical = ".\\Assets\\CornerRightVertical.png";
        const string Finish = ".\\Assets\\Finish.png";
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


        // Typedefs
        public static TrackSize _trackSize;
        public static StartPostion _startPostion;

        public static Direction DirectionChangeLeft(Direction dir) => (Direction)(((uint)dir - 1) % 4); // One Direction Down
        public static Direction DirectionChangeRight(Direction dir) =>(Direction)(((uint)dir + 1) % 4); // One Direction Up

        private static Direction DirectionCheckLeftOrRight(Direction dir, SectionTypes sec) => sec switch
        {
            SectionTypes.LeftCorner => DirectionChangeLeft(dir),
            SectionTypes.RightCorner => DirectionChangeRight(dir),
            _ => dir
        };

        private static string GetSectionFile(SectionTypes sec, Direction dir) {
           return sec switch
            {
                SectionTypes.Straight => ((int)dir % 2) switch
                {
                    0 => TrackVertical,
                    1 => TrackHorizontal,
                    _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
                },
           
                SectionTypes.LeftCorner => (int)dir switch
                {
                    0 => CornerLeftVertical, // NW
                    1 => CornerLeftHorizontal, // SW
                    2 => CornerRightVertical, // SE
                    3 => CornerRightHorizontal, // NE
                    _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
                },
                SectionTypes.RightCorner => (int)dir switch
                {
                    0 => CornerRightHorizontal, //NE
                    1 => CornerLeftVertical, // NW
                    2 => CornerLeftHorizontal, // SW
                    3 => CornerRightVertical, // SE
                    _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
                },
                SectionTypes.StartGrid => Finish,
                SectionTypes.Finish => Finish,
                _ => throw new ArgumentOutOfRangeException(nameof(sec), sec, null)
            };
        }

        private static void DrawSection(int xPos, int yPos, Direction direction, Graphics graphics, Section section) {
            Bitmap sectionBitmap = ImageCache.GetBitmap(GetSectionFile(section.SectionType, direction));
            graphics.DrawImage(sectionBitmap, xPos, yPos, sectionIMGSize, sectionIMGSize);
        }

        private static void MovePosition(ref int xPos, ref int yPos, Direction direction) {
            switch (direction) {
                case Direction.North:
                    yPos -= sectionIMGSize;
                    break;

                case Direction.East:
                    xPos += sectionIMGSize;
                    break;

                case Direction.South:
                    yPos += sectionIMGSize;
                    break;

                case Direction.West:
                    xPos -= sectionIMGSize;
                    break;
            }
        }

        // Draw the track every Tick
        public static BitmapSource DrawTrack(Track track) {
            (int width, int heigth) size;
            (int x, int y) startPosition;
            (size.width, size.heigth, startPosition) = GetTrackSize(track);
            Bitmap Canvas = ImageCache.CreateEmptyBitmap(size.width, size.heigth);
            Graphics g = Graphics.FromImage(Canvas);

            int xPos = _startPostion.posX;          // Recieve start pos X
            int yPos = _startPostion.posY;         // Receive start pos Y

            Direction curDir = Direction.East;            // Start always east

            foreach (Section sec in track.Sections) {
                DrawSection(xPos, yPos, curDir, g, sec);
                curDir = DirectionCheckLeftOrRight(curDir, sec.SectionType);

                MovePosition(ref xPos, ref yPos, curDir);

            }

            return ImageCache.CreateBitmapSourceFromGdiBitmap(Canvas);
        }



        // JUNK


        private static (int width, int height, (int x, int y)) GetTrackSize(Track track) {
            const int sectionSize = 256; // size of image.
            int startX = 10, startY = 10; // start at 10, 10. This way we can determine min and max positions
            int curX = startX, curY = startY;
            List<int> positionsX = new List<int>();
            List<int> positionsY = new List<int>();
            Direction direction = Direction.East; // start east

            // fill lists
            foreach (Section section in track.Sections) {
                // determine direction, set x and y accordingly, add to list.

                // add position to lists
                positionsX.Add(curX);
                positionsY.Add(curY);

                // change direction if needed
                if (section.SectionType == SectionTypes.LeftCorner) {
                    direction = DirectionChangeLeft(direction);
                }
                else if (section.SectionType == SectionTypes.RightCorner) {
                    direction = DirectionChangeRight(direction);
                }

                switch (direction) {
                    case Direction.North:
                        curY--;
                        break;

                    case Direction.East:
                        curX++;
                        break;

                    case Direction.South:
                        curY++;
                        break;

                    case Direction.West:
                        curX--;
                        break;
                }
            }

            // determine min and max positions
            int minX = positionsX.Min();
            int maxX = positionsX.Max() + 1; // give enough room for drawing method
            int minY = positionsY.Min();
            int maxY = positionsY.Max() + 1; // give enough room for drawing method

            // determine size
            int width = (maxX - minX) * sectionSize;
            int height = (maxY - minY) * sectionSize;

            // determine startposition
            int x = (startX - minX) * sectionSize;
            int y = (startY - minY) * sectionSize;

            return (width, height, (x, y));
        }

    }



  
}
