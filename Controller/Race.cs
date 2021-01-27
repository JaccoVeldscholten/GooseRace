using System;
using System.Collections.Generic;
using System.Text;
using Model;
using System.Timers;
using System.Diagnostics;

namespace Controller {

    enum Side {
        Left,
        Right
    }

    public class Race {
        
        // Used Objects
        public Track Track { get; set; }
        public DateTime StartTime { get; set; }
        private  Random random;
        private  Timer timer;

        // Used integers
        private  int maxLaps = 2;
        private int removedGooses = 0;

        // Lists
        public List<IParticipant> Gooses { get; set; }
        private Dictionary<Section, SectionData> Positions;
        public List<IParticipant> WinnerList;
        public Dictionary<IParticipant, int> Laps;

        // Used stats
        public RaceStats<GooseSectionTimes> RaceStatRoundtime;
        public RaceStats<SectionSpeed> sectionSpeed;
        public RaceStats<LostWings> wingsLostCounter;

        // Events
        public delegate void GoosesChangedEventHandler(object source, GoosesChangedEventArgs args);  
        public event GoosesChangedEventHandler GoosesChanged;                                         

        public delegate void RaceIsFinishedEventHandler(object source, EventArgs args);                 
        public event RaceIsFinishedEventHandler RaceIsFinished;                                         

        public Race(Track track, List<IParticipant> gooses) {                                     
            Track = track;
            Gooses = gooses;
            random = new Random(DateTime.Now.Millisecond);
            Positions = new Dictionary<Section, SectionData>();
            WinnerList = new List<IParticipant>();
            RaceStatRoundtime = new RaceStats<GooseSectionTimes>();
            sectionSpeed = new RaceStats<SectionSpeed>();
            wingsLostCounter = new RaceStats<LostWings>();
            Laps = new Dictionary<IParticipant, int>();
            timer = new Timer(200);
            timer.Elapsed += OnTimedEvent;
            SetStartPos();
            RandomizeEquipment();
            SetParticipantLaps();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public SectionData GetSectionData(Section section) {                // get section data, if it doesnt exist: create new
            if (!Positions.TryGetValue(section, out SectionData returnSectionData)) {
                SectionData newSectionData = new SectionData();
                Positions.Add(section, newSectionData);
                returnSectionData = newSectionData;
            }
            return returnSectionData;
        }


        public void SetStartPos() {                                   // give each competing participant a start position at random
            int tempArray = 0;
            List<Section> startGrids = DetermineStartGrids();               // make grid positions
            bool placeRight = false;
            for (int i = 0; Gooses.Count > i; i++) {                // check that each participant is taken care of
                PlaceParticipant(Gooses[i], placeRight, startGrids[tempArray]);      // place participants
                placeRight = !placeRight;
                if (i % 2 == 1) {
                    tempArray++;
                }
            }
        }

        public void SetParticipantLaps() {                                  // set lap of participant to -1 due to passing finish on start
            foreach (IParticipant part in Gooses) {
                Laps.Add(part, -1);
            }
        }

        public bool ParticipantIsFinished(IParticipant goose) {              // participant is done when amount of laps is enough
            return Laps[goose] >= maxLaps;
        }

        public void UpdateLap(IParticipant goose, DateTime elapsedDateTime) {    // participant amount of laps +1 
            Laps[goose]++;
        }

        private void UpdateLapOfParticipant(Section section, SectionData sectionData, Side side, DateTime elapsedDateTime) {        // update laptime of participant
            if (side == Side.Right) {
                UpdateLap(sectionData.Right, elapsedDateTime);
                if (ParticipantIsFinished(sectionData.Right)) {
                    WinnerList.Add(sectionData.Right);
                    sectionData.Right = null;
                    removedGooses++;
                }
            } else if (side == Side.Left) {
                UpdateLap(sectionData.Left, elapsedDateTime);
                if (ParticipantIsFinished(sectionData.Left)) {
                    WinnerList.Add(sectionData.Left);
                    sectionData.Left = null;
                    removedGooses++;
                }
            }
        }

        public List<IParticipant> GetEndResult() {
            // Return Finish order
            return WinnerList;
        }



        public RaceStats<SectionSpeed> GetRaceSectionSpeed() {       
            // Reciever for getting Section speeds for stats
            return sectionSpeed;
        }

        public RaceStats<LostWings> GetwingsLostCounter() {        
            // Receiver for getting the Broken count for stats
            return wingsLostCounter;
        }
        public RaceStats<GooseSectionTimes> GetRaceStatRoundTime() {
            // Receiver for getting the Round Times for stats
            return RaceStatRoundtime;
        }


        public List<Section> DetermineStartGrids() {                
            // Add the StartGrids to the list.
            List<Section> startList = new List<Section>();

            foreach (Section tp in Track.Sections) {
                if (tp.SectionType == SectionTypes.StartGrid) {
                    startList.Add(tp);
                }
            }
            startList.Reverse();

            return startList;
        }

        public void PlaceParticipant(IParticipant part, bool placeRight, Section section) {     // place participants on the grid
            if (placeRight) {
                GetSectionData(section).Right = part;
            } else {
                GetSectionData(section).Left = part;
            }
        }

        public void RandomizeEquipment() {    
            // Generating Random Equipment per goose
            foreach (Goose goose in Gooses) {
                goose.Equipment.Quality = random.Next(4, 11);      // Quality cant never a 0 so 5 is good
                goose.Equipment.Performance = random.Next(4, 11);  // Performance min 5. Otherwise its slow
                goose.Equipment.Speed = goose.Equipment.Performance * goose.Equipment.Quality;  // Speed is based on the Performance & Quality
            }
        }

        public void StartRace() {                               
            timer.Start();
        }

        public void OnTimedEvent(object sender, ElapsedEventArgs e) {   // each tick this event is fired
            LetTheWingsBreakOrFix();
            MoveParticipants(e.SignalTime);

            if (CheckIfRaceIsFinished()) {
                OnRaceIsFinished();
            }

        }

        public void LetTheWingsBreakOrFix() {              
            foreach (IParticipant goose in Gooses) {
                if (!goose.Equipment.IsBroken) {
                    // Wings not broken
                    if (random.Next(0, 50) == 10) {         // Change of 5% to get busted
                        goose.Equipment.IsBroken = true;
                        LostWings lostWing = new LostWings() { Name = goose.Name, TimesWingLost = 1};   // Save to generic
                        wingsLostCounter.AddRaceStatToList(lostWing);

                    }
                } else {
                    // Wing is broken
                    if (random.Next(0, 5) == 1) {              // Change of 25% to get fixed 
                        goose.Equipment.Quality--;              // Lower the quality every time it brokes
                        goose.Equipment.Speed = goose.Equipment.Performance * goose.Equipment.Quality;  // Decrease speed when wing failed
                        goose.Equipment.IsBroken = false;
                        if (goose.Equipment.Quality < 10) {
                            goose.Equipment.Quality = 10;       // Fix so the goose will not stop
                                                                // There is no Safety Goose Around here
                        }
                        if (goose.Equipment.Speed < 10) {
                            goose.Equipment.Speed = 10;         // Fix so the goose will not stop
                                                                // There is no Safety Goose Around here
                        }
                    }

                }

            }
        }

        public void MoveParticipants(DateTime elapsedDateTime) {                // move participants 
            LinkedListNode<Section> currentNode = Track.Sections.Last;
            Section nextNodeValue;
            while (currentNode != null) {
                if (currentNode.Next != null) {
                    nextNodeValue = currentNode.Next.Value;
                } else {
                    nextNodeValue = Track.Sections.First.Value;
                }
                ParticipantsMoveSectionData(currentNode.Value, nextNodeValue, elapsedDateTime);

                currentNode = currentNode.Previous;
            }
        }

        public void ParticipantsMoveSectionData(Section currentSection, Section nextSection, DateTime elapsedDateTime) {    // move participants if there is space
            SectionData currentSectionData = GetSectionData(currentSection);
            SectionData nextSectionData = GetSectionData(nextSection);

            // check if there is place infront + if the participant equipment is not broken
            if (currentSectionData.Left != null && !currentSectionData.Left.Equipment.IsBroken) {
                currentSectionData.DistanceLeft += SpeedOfParticipant(currentSectionData.Left);
            }

            if (currentSectionData.Right != null && !currentSectionData.Right.Equipment.IsBroken) {
                currentSectionData.DistanceRight += SpeedOfParticipant(currentSectionData.Right);
            }

            // both participants of a grid want to move
            if (currentSectionData.DistanceLeft >= 100 && currentSectionData.DistanceRight >= 100) {
                int freePlaces = PlaceLeft(nextSectionData);
                if (freePlaces == 0) {      // let participants wait with moving
                    currentSectionData.DistanceRight = 90;
                    currentSectionData.DistanceLeft = 90;
                } else if (freePlaces == 3) {
                    MoveParticipant(currentSection, nextSection, Side.Left, Side.Left, false, elapsedDateTime);
                    MoveParticipant(currentSection, nextSection, Side.Right, Side.Right, false, elapsedDateTime);
                } else {
                    if (currentSectionData.DistanceLeft >= currentSectionData.DistanceRight) {
                        // prefe r left
                        if (freePlaces == 1) {
                            MoveParticipant(currentSection, nextSection, Side.Left, Side.Left, true, elapsedDateTime);
                        } else if (freePlaces == 2) {
                            MoveParticipant(currentSection, nextSection, Side.Left, Side.Right, true, elapsedDateTime);
                        }
                        // place rights
                    } else {
                        if (freePlaces == 1) {
                            MoveParticipant(currentSection, nextSection, Side.Right, Side.Left, true, elapsedDateTime);
                        } else if (freePlaces == 2) {
                            MoveParticipant(currentSection, nextSection, Side.Right, Side.Right, true, elapsedDateTime);
                        }
                    }
                }

                // participant left wants to move
            } else if (currentSectionData.DistanceLeft >= 100) {
                // for freesections, prefer same spot, otherwise take other
                int freePlaces = PlaceLeft(nextSectionData);
                if (freePlaces == 0) {
                    currentSectionData.DistanceLeft = 90;
                } else if (freePlaces == 3 || freePlaces == 1) {
                    // move from left to left
                    MoveParticipant(currentSection, nextSection, Side.Left, Side.Left, false, elapsedDateTime);         // move left to left
                } else if (freePlaces == 2) {
                    // move from left to right
                    MoveParticipant(currentSection, nextSection, Side.Left, Side.Right, false, elapsedDateTime);        // move left to right
                }


                                                                                                                        // participant right wants to move
            } else if (currentSectionData.DistanceRight >= 100) {
                int freePlaces = PlaceLeft(nextSectionData);
                if (freePlaces == 0) {
                    currentSectionData.DistanceRight = 90;
                } else if (freePlaces == 3 || freePlaces == 2) {
                    MoveParticipant(currentSection, nextSection, Side.Right, Side.Right, false, elapsedDateTime);       // move right pos to right
                } else if (freePlaces == 1) {
                    MoveParticipant(currentSection, nextSection, Side.Right, Side.Left, false, elapsedDateTime);        // move right pos to left
                }
            }
            
        }

        private void MoveParticipant(Section currentSection, Section nextSection, Side start, Side end, bool correctOtherSide, DateTime elapsedDateTime) {      // make participant move
            SectionData currentSectionData = GetSectionData(currentSection);
            SectionData nextSectionData = GetSectionData(nextSection);

            // Check the side the goose is starting from
            if (start == Side.Right) {
                if (end == Side.Right) {
                    nextSectionData.Right = currentSectionData.Right;
                    nextSectionData.TimeRight = elapsedDateTime;
                    nextSectionData.DistanceRight = currentSectionData.DistanceRight - 100;
                } 
                // Als het geen links is dan moet het rechts zijn
                else if (end == Side.Left) {
                    nextSectionData.Left = currentSectionData.Right;
                    nextSectionData.TimeLeft = elapsedDateTime;
                    nextSectionData.DistanceLeft = currentSectionData.DistanceRight - 100;
                }

                RaceStatRoundtime.AddRaceStatToList(new GooseSectionTimes() {
                    Name = currentSectionData.Right.Name,
                    Time = elapsedDateTime - currentSectionData.TimeRight,
                    Section = currentSection
                }
                );
                sectionSpeed.AddRaceStatToList(new SectionSpeed() {
                    Name = currentSectionData.Right.Name,
                    Section = currentSection,
                    Speed = currentSectionData.Right.Equipment.Speed
                }
                );

                currentSectionData.Right = null;
                currentSectionData.DistanceRight = 0;

            } 
            else if (start == Side.Left) {
                if (end == Side.Right) {                                                                       // add racedata to list
                    nextSectionData.Right = currentSectionData.Left;
                    nextSectionData.TimeRight = elapsedDateTime;
                    nextSectionData.DistanceRight = currentSectionData.DistanceLeft - 100;
                } else if (end == Side.Left) {                                                               // add racedata to list      
                    nextSectionData.Left = currentSectionData.Left;
                    nextSectionData.TimeLeft = elapsedDateTime;
                    nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - 100;
                }

                RaceStatRoundtime.AddRaceStatToList(new GooseSectionTimes() {                                // add racedata to list
                    Name = currentSectionData.Left.Name,
                    Time = elapsedDateTime - currentSectionData.TimeLeft,
                    Section = currentSection
                }
                );
                sectionSpeed.AddRaceStatToList(new SectionSpeed() {                                         // add racedata to list
                    Name = currentSectionData.Left.Name,
                    Section = currentSection,
                    Speed = currentSectionData.Left.Equipment.Speed
                }
                );

                currentSectionData.Left = null;
                currentSectionData.DistanceLeft = 0;
            }

            if (nextSection.SectionType == SectionTypes.Finish) {
                if (end == Side.Right) {
                    UpdateLapOfParticipant(nextSection, nextSectionData, Side.Right, elapsedDateTime);          // update participant lap
                } else if (end == Side.Left) {
                    UpdateLapOfParticipant(nextSection, nextSectionData, Side.Left, elapsedDateTime);           // update participant lap
                }
            }

            if (start == Side.Right && correctOtherSide) {
                currentSectionData.DistanceLeft = 90;
            } else if (start == Side.Left && correctOtherSide) {
                currentSectionData.DistanceRight = 90;
            }

            OnGoosesChanged(Track);                                // fire event of drivers to tell other handlers that drivers have moved
        }

        public int SpeedOfParticipant(IParticipant part) {          // get the speed of participant
            int speed = part.Equipment.Speed;
            return speed;
        }

        public int PlaceLeft(SectionData sectionData) {    // check if there are places left on the section
            int returnValue = 0;
            if (sectionData.Left == null) {
                returnValue += 1;
            }
            if (sectionData.Right == null) {
                returnValue += 2;
            }
            return returnValue;
        }

        public string GetBestParticipantSectionTime() {         // returns best participant time
            return RaceStatRoundtime.GetHighest();
        }

        private bool CheckIfRaceIsFinished() {                  // checks if race is finished
            bool returnBool = false;
            if (removedGooses == Gooses.Count) {
                returnBool = true;
            }
            return returnBool;
        }


        protected virtual void OnGoosesChanged(Track track) {
            GoosesChanged?.Invoke(this, new GoosesChangedEventArgs() { Track = track });
        }

        protected virtual void OnRaceIsFinished() {
            RaceIsFinished?.Invoke(this, EventArgs.Empty);
        }

        public void CleanUp() {             // cleans the race
            timer.Stop();
            GoosesChanged = null;
            RaceIsFinished = null;
        }
    }
}
