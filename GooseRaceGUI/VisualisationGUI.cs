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
            public int startX;
            public int startY;
        }

        #region IMGconstants
        const int TrackIMGSize = 256;
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
        TrackSize _trackSize;
        StartPostion _startPostion;

        public Direction DirectionChangeLeft(Direction dir) => (Direction)(((uint)dir - 1) % 4); // One Direction Down
        public Direction DirectionChangeRight(Direction dir) =>(Direction)(((uint)dir + 1) % 4); // One Direction Up

        public void SetTrackSize(Track track) {
            int marginLeft = 10;
            int maginRight = 10;

            List<int> posX = new List<int>();
            List<int> posY = new List<int>();
            Direction dir = Direction.East; // Altijd starten op Oost

            foreach (Section sec in track.Sections) {
                
                // Adding Margins to X/Y
                posX.Add(marginLeft);
                posY.Add(maginRight);

                if(sec.SectionType == SectionTypes.LeftCorner) {
                    dir = DirectionChangeLeft(dir);
                }
            }
        }


        // Draw the track every Tick
        public BitmapSource DrawTrack(Track track) {
            SetTrackSize(track);
            Bitmap Canvas = ImageCache.CreateBitmap(10, 10);
            return ImageCache.CreateBitmapSourceFromGdiBitmap(Canvas);

        }
    }
}
