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
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        private readonly Random _random;
        private Dictionary<Section, SectionData> _positions;
        private readonly Timer timer;
        private readonly int amountOfLaps = 2;
        public List<IParticipant> participantsFinishOrder;
        private int removedPartcipants = 0;
        public Dictionary<IParticipant, int> lapsPerParticipant;
        public RaceStats<SectionRoundtime> RaceStatRoundtime;
        public RaceStats<SectionSpeed> sectionSpeed;
        public RaceStats<BrokenCounter> brokenCounter;

        public delegate void GoosesChangedEventHandler(object source, GoosesChangedEventArgs args);   // event handler to see if drivers have changed
        public event GoosesChangedEventHandler GoosesChanged;                                         // event to let drivers change

        public delegate void RaceIsFinishedEventHandler(object source, EventArgs args);                 // event handler to see if race is finished
        public event RaceIsFinishedEventHandler RaceIsFinished;                                         // event to let race finish

        public Race(Track track, List<IParticipant> participants) {                                     // constructor making standard race
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            participantsFinishOrder = new List<IParticipant>();
            RaceStatRoundtime = new RaceStats<SectionRoundtime>();
            sectionSpeed = new RaceStats<SectionSpeed>();
            brokenCounter = new RaceStats<BrokenCounter>();
            timer = new Timer(300);
            timer.Elapsed += OnTimedEvent;
            SetStartPos();
            RandomizeEquipment();
            SetParticipantLaps();
        }

        public SectionData GetSectionData(Section section) {                // get section data, if it doesnt exist: create new
            if (!_positions.TryGetValue(section, out SectionData returnSectionData)) {
                SectionData newSectionData = new SectionData();
                _positions.Add(section, newSectionData);
                returnSectionData = newSectionData;
            }
            return returnSectionData;
        }

        public void SetStartPos() {                                   // give each competing participant a start position at random
            int tempArray = 0;
            List<Section> startGrids = DetermineStartGrids();               // make grid positions
            bool placeRight = false;
            for (int i = 0; Participants.Count > i; i++) {                // check that each participant is taken care of
                PlaceParticipant(Participants[i], placeRight, startGrids[tempArray]);      // place participants
                placeRight = !placeRight;
                if (i % 2 == 1) {
                    tempArray++;
                }
            }
        }

        public void SetParticipantLaps() {                                  // set lap of participant to -1 due to passing finish on start
            lapsPerParticipant = new Dictionary<IParticipant, int>();
            foreach (IParticipant part in Participants) {
                lapsPerParticipant.Add(part, -1);
            }
        }

        public bool ParticipantIsFinished(IParticipant part) {              // participant is done when amount of laps is enough
            return lapsPerParticipant[part] >= amountOfLaps;
        }

        public void UpdateLap(IParticipant part, DateTime elapsedDateTime) {    // participant amount of laps +1 
            lapsPerParticipant[part]++;
        }

        private void UpdateLapOfParticipant(Section section, SectionData sectionData, Side side, DateTime elapsedDateTime) {        // update laptime of participant
            if (side == Side.Right) {
                UpdateLap(sectionData.Right, elapsedDateTime);
                if (ParticipantIsFinished(sectionData.Right)) {
                    participantsFinishOrder.Add(sectionData.Right);
                    sectionData.Right = null;
                    removedPartcipants++;
                }
            } else if (side == Side.Left) {
                UpdateLap(sectionData.Left, elapsedDateTime);
                if (ParticipantIsFinished(sectionData.Left)) {
                    participantsFinishOrder.Add(sectionData.Left);
                    sectionData.Left = null;
                    removedPartcipants++;
                }
            }
        }

        public List<IParticipant> getEndResult() {                      // make finish order of participants
            return participantsFinishOrder;
        }

        public RaceStats<SectionRoundtime> getRaceStatRoundTime() {     // get race section times
            return RaceStatRoundtime;
        }

        public RaceStats<SectionSpeed> getRaceSectionSpeed() {          // get race sections speeds
            return sectionSpeed;
        }

        public RaceStats<BrokenCounter> getBrokenCounter() {            // get race amount of broken of participants
            return brokenCounter;
        }

        public List<Section> DetermineStartGrids() {                    // see where to put startgrids
            List<Section> startGridSections = new List<Section>();

            foreach (Section trackPart in Track.Sections) {
                if (trackPart.SectionType == SectionTypes.StartGrid) {
                    startGridSections.Add(trackPart);
                }
            }
            startGridSections.Reverse();

            return startGridSections;
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
            foreach (Goose goose in Participants) {
                goose.Equipment.Quality = _random.Next(5, 10);      // Quality cant never a 0 so 5 is good
                goose.Equipment.Performance = _random.Next(5, 10);  // Performance min 5. Otherwise its slow
                goose.Equipment.Speed = goose.Equipment.Performance * goose.Equipment.Quality;
            }
        }

        public void StartRace() {                                   // start timer of the race (and race itself)
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
            foreach (IParticipant p in Participants) {
                if (!p.Equipment.IsBroken) {
                    // Wings not broken
                    if (_random.Next(1, (25 + p.Equipment.Quality)) == 1) {
                        p.Equipment.IsBroken = true;
                        BrokenCounter bc = new BrokenCounter() { name = p.Name, timesBroken = 1};
                        brokenCounter.addRaceStatToList(bc);

                    }
                } else {
                    // Wings is broken
                    if (_random.Next(0, 2) == 1) {
                         p.Equipment.Quality--;
                         p.Equipment.Speed = p.Equipment.Performance * p.Equipment.Quality;
                         p.Equipment.IsBroken = false;
                        if (p.Equipment.Quality < 10) {
                            p.Equipment.Quality = 10;
                        }
                        if (p.Equipment.Speed < 10) {
                            p.Equipment.Speed = 10;
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
                int freePlaces = AnyPlacesLeftOnSection(nextSectionData);
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
                int freePlaces = AnyPlacesLeftOnSection(nextSectionData);
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
                int freePlaces = AnyPlacesLeftOnSection(nextSectionData);
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

            if (start == Side.Right) {
                if (end == Side.Right) {
                    nextSectionData.Right = currentSectionData.Right;
                    nextSectionData.TimeRight = elapsedDateTime;
                    nextSectionData.DistanceRight = currentSectionData.DistanceRight - 100;
                } else if (end == Side.Left) {
                    nextSectionData.Left = currentSectionData.Right;
                    nextSectionData.TimeLeft = elapsedDateTime;
                    nextSectionData.DistanceLeft = currentSectionData.DistanceRight - 100;
                }

                // ================== DATA ==================
                RaceStatRoundtime.addRaceStatToList(new SectionRoundtime() {
                    name = currentSectionData.Right.Name,
                    time = elapsedDateTime - currentSectionData.TimeRight,
                    section = currentSection
                }
                );
                sectionSpeed.addRaceStatToList(new SectionSpeed() {
                    name = currentSectionData.Right.Name,
                    section = currentSection,
                    speed = currentSectionData.Right.Equipment.Speed
                }
                ); 
                // ==========================================


                // reset current section data
                currentSectionData.Right = null;
                currentSectionData.DistanceRight = 0;

            } else if (start == Side.Left) {
                if (end == Side.Right) {                                                                       // add racedata to list
                    nextSectionData.Right = currentSectionData.Left;
                    nextSectionData.TimeRight = elapsedDateTime;
                    nextSectionData.DistanceRight = currentSectionData.DistanceLeft - 100;
                } else if (end == Side.Left) {                                                               // add racedata to list      
                    nextSectionData.Left = currentSectionData.Left;
                    nextSectionData.TimeLeft = elapsedDateTime;
                    nextSectionData.DistanceLeft = currentSectionData.DistanceLeft - 100;
                }

                // ================== DATA ==================
                RaceStatRoundtime.addRaceStatToList(new SectionRoundtime() {                                // add racedata to list
                    name = currentSectionData.Left.Name,
                    time = elapsedDateTime - currentSectionData.TimeLeft,
                    section = currentSection
                }
                );
                sectionSpeed.addRaceStatToList(new SectionSpeed() {                                         // add racedata to list
                    name = currentSectionData.Left.Name,
                    section = currentSection,
                    speed = currentSectionData.Left.Equipment.Speed
                }
                );
                // ==========================================


                // reset current section data
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

        public int AnyPlacesLeftOnSection(SectionData sectionData) {    // check if there are places left on the section
            /*      0 - nothing free
                    1 - left free
                    2 - right free
                    3 - left and right free  */
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
            if (removedPartcipants == Participants.Count) {
                returnBool = true;
            }
            return returnBool;
        }


        protected virtual void OnGoosesChanged(Track track) {
            GoosesChanged?.Invoke(this, new GoosesChangedEventArgs() { track = track });
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
