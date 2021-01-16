using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Controller;
using Model;

namespace GooseGUI {
    public static class Visualisation {

        #region IMG_Resources

        // TrackParts
        const string CornerRightVertical = ".\\Assets\\Track\\CornerLeftHorizontal.png";
        const string CornerLeftVertical = ".\\Assets\\Track\\CornerLeftVertical.png";

        const string CornerRightHorizontal = ".\\Assets\\Track\\CornerRightHorizontal.png";
        const string CornerLeftHorizontal = ".\\Assets\\Track\\CornerRightVertical.png";

        const string Finish = ".\\Assets\\Track\\Finish.png";
        const string Start = ".\\Assets\\Track\\Start.png";
        const string TrackHorizontal = ".\\Assets\\Track\\TrackHorizontal.png";
        const string TrackVertical = ".\\Assets\\Track\\TrackVertical.png";

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

        const string Broken = ".\\Assets\\Gooses\\BrokenGoose.png";


        #endregion

        internal enum Direction {
            North,
            East,
            South,
            West
        }

        internal enum Side {
            Left,
            Right
        }

        private static Race race;
        private const int SectionDimension = 256;
        private const int DriverDimension = 64;
        private const string Message = "Teamcolor not found.";

        public static void Initialize(Race r) {
            race = r;
        }

        public static BitmapSource DrawTrack(Track track) {                                         // draws track on given position

            (int width, int heigth) size;
            (int x, int y) startPosition;
            (size.width, size.heigth, startPosition) = GetTrackSize(track);

            Bitmap canvas = Images.EmptyBitmap(size.width, size.heigth);                            // make images and canvas
            Graphics g = Graphics.FromImage(canvas);

            int xPosition = startPosition.x;
            int yPosition = startPosition.y;
            Direction currentDirection = Direction.East;

            foreach (Section section in track.Sections) {                                          
                DrawTrackPieceSection(xPosition, yPosition, currentDirection, g, section);       

                currentDirection = SectionDirection(currentDirection, section.SectionType);
                DrawParticipantsOnSection(xPosition, yPosition, currentDirection, g, section);

                MoveImagePosition(ref xPosition, ref yPosition, currentDirection);             
            }
            return Images.CreateBitmapSourceFromGdiBitmap(canvas);

        }

        private static (int width, int height, (int x, int y)) GetTrackSize(Track track) {        
            Direction direction = Direction.East;               // Always start from East
            const int sectionSize = SectionDimension;
            int startPositionX = 0;
            int startPositionY = 0;
            int currentX = startPositionX;
            int currentY = startPositionY;
            List<int> positionsXBuffer = new List<int>();
            List<int> positionsYBuffer = new List<int>();

            foreach (Section section in track.Sections) {                                
                                                                                               
                positionsXBuffer.Add(currentX);
                positionsYBuffer.Add(currentY);

                // change direction
                if (section.SectionType == SectionTypes.LeftCorner) {
                    direction = (Direction)(((uint)direction - 1) % 4);
                }
                else if (section.SectionType == SectionTypes.RightCorner) {
                    direction = (Direction)(((uint)direction + 1) % 4);
                }
                NextPosition(ref currentX, ref currentY, direction);
            }

            // check min and max position
            int minPositionX = positionsXBuffer.Min();
            int maxPositionX = positionsXBuffer.Max() + 1;
            int minPositionY = positionsYBuffer.Min();
            int maxPositionY = positionsYBuffer.Max() + 1;

            int width = (maxPositionX - minPositionX) * sectionSize;                        // check max size of track
            int height = (maxPositionY - minPositionY) * sectionSize;

            // check where start postitions need to be
            int x = (startPositionX - minPositionX) * sectionSize;
            int y = (startPositionY - minPositionY) * sectionSize;

            return (width, height, (x, y));
        }

        private static void NextPosition(ref int currentX, ref int currentY, Direction direction) {
            // Determine position
            switch (direction) {
                case Direction.North:
                    currentY--;
                    break;
                case Direction.East:
                    currentX++;
                    break;
                case Direction.South:
                    currentY++;
                    break;
                case Direction.West:
                    currentX--;
                    break;
            }
        }

        private static Direction SectionDirection(Direction direction, SectionTypes sectionType) {      // check which direction is needed

            Direction returnDirection = direction;
            switch (sectionType) {
                case SectionTypes.LeftCorner:
                    returnDirection = (Direction)(((uint)direction - 1) % 4);
                    break;
                case SectionTypes.RightCorner:
                    returnDirection = (Direction)(((uint)direction + 1) % 4);
                    break;
                default:
                    break;
            }

            return returnDirection;
        }


        private static string GetImageFromSectionType(SectionTypes sectionType, Direction direction) {          // get image name
            string returnImage;

            if (sectionType == SectionTypes.Straight) {
                // section is straight
                if ((int)direction % 2 == 0) {
                    returnImage = TrackVertical;
                }
                else {
                    returnImage = TrackHorizontal;
                }
            }
            else if (sectionType == SectionTypes.LeftCorner) {
                // section goes left
                if ((int)direction == 0) {
                    returnImage = CornerLeftVertical;
                }
                else if ((int)direction == 1) {
                    returnImage = CornerRightVertical;
                }
                else if ((int)direction == 2) {
                    returnImage = CornerRightHorizontal;
                }
                else {
                    returnImage = CornerLeftHorizontal;
                }
            }
            else if (sectionType == SectionTypes.RightCorner) {
                // section goes right
                if ((int)direction == 0) {
                    returnImage = CornerLeftHorizontal;
                }
                else if ((int)direction == 1) {
                    returnImage = CornerLeftVertical;
                }
                else if ((int)direction == 2) {
                    returnImage = CornerRightVertical;
                }
                else {
                    returnImage = CornerRightHorizontal;
                }
            }
            else if (sectionType == SectionTypes.StartGrid) {
                returnImage = Start;
            }
            else {
                returnImage = Finish;
            }

            return returnImage;
        }

        private static void MoveImagePosition(ref int xPosition, ref int yPosition, Direction direction) {          // change direction
            switch (direction) {
                case Direction.North:
                    yPosition -= SectionDimension;
                    break;
                case Direction.East:
                    xPosition += SectionDimension;
                    break;
                case Direction.South:
                    yPosition += SectionDimension;
                    break;
                case Direction.West:
                    xPosition -= SectionDimension;
                    break;
            }
        }


        private static void DrawTrackPieceSection(int xPosition, int yPosition, Direction direction, Graphics g, Section section) {        // draw track from files
            Bitmap sectionBitmap = Images.GetBitmap(GetImageFromSectionType(section.SectionType, direction));
            g.DrawImage(sectionBitmap, xPosition, yPosition, SectionDimension, SectionDimension);
        }

        private static void DrawBrokenImageOnPosition(Graphics g, int x, int y) {
            g.DrawImage(Images.GetBitmap(Broken), x, y, DriverDimension, DriverDimension);                                                      // put image on given position
        }
        private static void DrawGooseOnPosition(IParticipant participant, Graphics g, Direction d, int xPos, int yPos) {
            Bitmap participantBitmap = Images.GetBitmap(TeamColorToFilename(participant.TeamColor, d));
            g.DrawImage(participantBitmap, xPos, yPos, DriverDimension, DriverDimension);
        }


        private static void DrawParticipantsOnSection(int xPos, int yPos, Direction currentDirection, Graphics g, Section section) {            // draw participants on the track
            IParticipant leftParticipant = race.GetSectionData(section).Left;
            IParticipant rightParticipant = race.GetSectionData(section).Right;

            if (leftParticipant != null) {
                (int x, int y) = OffsetParticipants(Side.Left, currentDirection);                                                                // draw participants on offset postion
                DrawGooseOnPosition(leftParticipant, g, currentDirection, xPos + x, yPos + y);
                if (leftParticipant.Equipment.IsBroken) {
                    DrawBrokenImageOnPosition(g, xPos + x, yPos + y);
                }
            }

            if (rightParticipant != null) {
                (int x, int y) = OffsetParticipants(Side.Right, currentDirection);
                DrawGooseOnPosition(rightParticipant, g, currentDirection, xPos + x, yPos + y);
                if (rightParticipant.Equipment.IsBroken) {
                    DrawBrokenImageOnPosition(g, xPos + x, yPos + y);
                }
            }
        }

        private static (int x, int y) OffsetParticipants(Side side, Direction currentDirection) {
            // Creating Offsets for the screen
            (int x, int y) returnOffset = (0, 0);
            if (side == Side.Left)                                  // left side
            {
                switch (currentDirection) {
                    case Direction.North:
                        returnOffset = (62, 82);
                        break;
                    case Direction.East:
                        returnOffset = (110, 62);
                        break;
                    case Direction.South:
                        returnOffset = (130, 110);
                        break;
                    case Direction.West:
                        returnOffset = (82, 130);
                        break;
                }
            }
            else {                                                // right side   
                switch (currentDirection) {
                    case Direction.North:
                        returnOffset = (130, 110);
                        break;
                    case Direction.East:
                        returnOffset = (82, 130);
                        break;
                    case Direction.South:
                        returnOffset = (62, 82);
                        break;
                    case Direction.West:
                        returnOffset = (110, 62);
                        break;
                }
            }
            return returnOffset;
        }



        private static string TeamColorToFilename(TeamColors color, Direction d) {          // change color, switch case test
            return d switch
            {
                Direction.North => color switch
                {
                    TeamColors.Red => Red_North,
                    TeamColors.Green => Green_North,
                    TeamColors.Yellow => Yellow_North,
                    TeamColors.Grey => Grey_North,
                    TeamColors.Blue => Blue_North,
                    _ => throw new ArgumentOutOfRangeException(nameof(color), color, "Eror color not in enum")
                },

                Direction.East => color switch
                {
                    TeamColors.Red => Red_East,
                    TeamColors.Green => Green_East,
                    TeamColors.Yellow => Yellow_East,
                    TeamColors.Grey => Grey_East,
                    TeamColors.Blue => Blue_East,
                    _ => throw new ArgumentOutOfRangeException(nameof(color), color, Message)
                },

                Direction.South => color switch
                {
                    TeamColors.Red => Red_South,
                    TeamColors.Green => Green_South,
                    TeamColors.Yellow => Yellow_South,
                    TeamColors.Grey => Grey_South,
                    TeamColors.Blue => Blue_South,
                    _ => throw new ArgumentOutOfRangeException(nameof(color), color, "Eror color not in enum")
                },

                Direction.West => color switch
                {
                    TeamColors.Red => Red_West,
                    TeamColors.Green => Green_West,
                    TeamColors.Yellow => Yellow_West,
                    TeamColors.Grey => Grey_West,
                    TeamColors.Blue => Blue_West,
                    _ => throw new ArgumentOutOfRangeException(nameof(color), color, "Eror color not in enum")
                },
                _ => throw new ArgumentOutOfRangeException(nameof(d), d, "Eror color not in enum")
            };
        }
    }
}