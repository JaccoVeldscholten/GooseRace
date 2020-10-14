using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Model;

namespace Controller {
    public class Race {

        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;

        private Dictionary<Section, SectionData> _positions;

        private Timer timer;

        public delegate void onGoosesChanged(object Sender, GoosesChangedEventArgs GoosesChangedEventArgs);
        public delegate void onNextRace(object Sender, EventArgs nextRaceEventArgs);

        public event onGoosesChanged GoosesChanged;
        public event onNextRace NextRace;

        private int amountOfLaps = 2;
        private Dictionary<IParticipant, int> DrivenRounds = new Dictionary<IParticipant, int>();
        private int goosesRemoved;

        // Dictonary to keep track of the Finished Gooses
        public Dictionary<int, string> FinishPosition = new Dictionary<int, string>();


        //Generic list to keep track of section times of the goose
        public RaceInformation<GooseLapTimes> lapTimes = new RaceInformation<GooseLapTimes>();

        //Generic list to keep track how many the goose flown
        public RaceInformation<GooseFlownTime> gooseFlownTime = new RaceInformation<GooseFlownTime>();

        //Generic list to storage how many times the Goose lost his wings
        public RaceInformation<GooseLostWingAmount> lostWingsAmount = new RaceInformation<GooseLostWingAmount>();


        public Race(Track track, List<IParticipant> participants) {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            RandomizeEquipment();

            // Fill Dictornary with Gooses
            foreach (IParticipant gooses in Participants) {
                DrivenRounds.Add(gooses, 0);
            }

            PlaceGoosesOnGrid();


            timer = new Timer(500);
            timer.Elapsed += OnTimedEvent;
            Start();

        }

        public SectionData GetSectionData(Section section) {
            if (!_positions.ContainsKey(section)) {
                _positions.Add(section, new SectionData());
            }
            return _positions[section];
        }

        public void RandomizeEquipment() {

            foreach (Goose goose in Participants) { 
                goose.Equipment.Quality     = _random.Next(1, 100);             // Random Quality per goose
                goose.Equipment.Performance = _random.Next(1, 100);         // Random Performance per goose
            }
        }

        public void PlaceGoosesOnGrid() {
            // create List of startgrids, from front to back
            List<Section> startGrids = GetStartGrids();

            // look at amount of participants and amount of start places.
            int amountToPlace = 0;
            if (Participants.Count >= startGrids.Count * 2) {
                amountToPlace = startGrids.Count * 2;
            }
            else if (Participants.Count < startGrids.Count * 2) {
                amountToPlace = Participants.Count;
            }


            bool whatSide = false; // false is left, true is right
            int currentStartGridIndex = 0;
            for (int i = 0; i < amountToPlace; i++) {
                // place
                PlaceParticipant(Participants[i], whatSide, startGrids[currentStartGridIndex]);
                // flip whatSide
                whatSide = !whatSide;
                // up section index on every uneven number for i
                if (i % 2 == 1) {
                    currentStartGridIndex++;
                }

            }

        }

        public List<Section> GetStartGrids() {
            List<Section> startGridSections = new List<Section>();

            // Fill the list with sections
            foreach (Section trackSection in Track.Sections) {
                if (trackSection.SectionType == SectionTypes.StartGrid) {
                    startGridSections.Add(trackSection);                    // Only start
                }

            }

            // Reverse list because LIFO
            startGridSections.Reverse();

            return startGridSections;
        }

        public void PlaceParticipant(IParticipant p, bool whatSide, Section section) {
            if (whatSide) {
                GetSectionData(section).Right = p;
            }
            else {
                GetSectionData(section).Left = p;
            }
        }


        private bool LetTheWingsFallOff(IParticipant participant) {

            if (!participant.Equipment.IsBroken) {
                if (_random.Next(1, 20) == 10) {            // Change to let the wings fall off. Change 1 on 20
                    participant.Equipment.IsBroken = true;  // Goose lost wing
                    lostWingsAmount.AddItemToList(new GooseLostWingAmount(participant.Name, 1));
                    Console.WriteLine("Shit i my Lost wings! HONK!");       
                    return true;
                }
                else { return false; }  // Not this time
            }
            else {
                if (_random.Next(1, 10) == 1) {
                    participant.Equipment.IsBroken = false;
                    //Quality will be lowered if possible
                    if (participant.Equipment.Quality > 1) {
                        participant.Equipment.Quality -= 1;
                    }
                    return false;
                }
                else { return true; }
            }
        }

        public bool MoveGooseToNextSection(LinkedListNode<Section> section, LinkedListNode<Section> nextSection, int LeftRight, DateTime CurrentTime) { 

                SectionData sectionValue = GetSectionData(section.Value);
                SectionData nextSectionValue;

                //Check if last section
                if (Track.Sections.Last == section) {
                    nextSectionValue = GetSectionData(Track.Sections.First.Value);
                }
                else {
                    nextSectionValue = GetSectionData(nextSection.Value);
                }

                //Check if left or right driver crosses finish
                if (LeftRight == 0) {

                    //Check if left driver is crossing finish
                    if (section.Value.SectionType == SectionTypes.Finish) {
                        DrivenRounds[sectionValue.Left] += 1;

                        if (DrivenRounds[sectionValue.Left] == amountOfLaps + 1) {
                            lapTimes.AddItemToList(new GooseLapTimes(sectionValue.Left.Name, CurrentTime - sectionValue.startTimeLeft, section.Value));

                            FinishPosition.Add(FinishPosition.Count + 1, sectionValue.Left.Name);
                            sectionValue.Left = null;
                            sectionValue.DistanceLeft = 100;
                            goosesRemoved++;
                            return true;
                        }
                    }
                }
                else {
                    //Check if left driver is crossing finish
                    if (section.Value.SectionType == SectionTypes.Finish) {
                        DrivenRounds[sectionValue.Right] += 1;

                        if (DrivenRounds[sectionValue.Right] == amountOfLaps + 1) {
                            lapTimes.AddItemToList(new GooseLapTimes(sectionValue.Right.Name, CurrentTime - sectionValue.startTimeLeft, section.Value));

                            FinishPosition.Add(FinishPosition.Count + 1, sectionValue.Right.Name);
                            sectionValue.Right = null;
                            sectionValue.DistanceRight = 100;
                            goosesRemoved++;

                            return true;
                        }
                    }
                }


                //Move to left section
                if (nextSectionValue.Left == null) {
                    //Move the left driver
                    if (LeftRight == 0) {
                        lapTimes.AddItemToList(new GooseLapTimes(sectionValue.Left.Name, CurrentTime - sectionValue.startTimeLeft, section.Value));

                        nextSectionValue.Left = sectionValue.Left;
                        nextSectionValue.DistanceLeft += sectionValue.DistanceLeft;
                        nextSectionValue.startTimeLeft = CurrentTime;
                        //Reset values on current tile
                        sectionValue.Left = null;
                        sectionValue.DistanceLeft = 100;
                    }
                    // Move the right driver
                    else {
                        lapTimes.AddItemToList(new GooseLapTimes(sectionValue.Right.Name, CurrentTime - sectionValue.startTimeLeft, section.Value));


                        nextSectionValue.Left = sectionValue.Right;
                        nextSectionValue.DistanceLeft += sectionValue.DistanceRight;
                        nextSectionValue.startTimeLeft = CurrentTime;
                        //Reset values on current tile
                        sectionValue.Right = null;
                        sectionValue.DistanceRight = 100;
                    }
                    return true;
                }
                //Move to right section
                else if (nextSectionValue.Right == null) {
                    //Move the left driver
                    if (LeftRight == 0) {
                        lapTimes.AddItemToList(new GooseLapTimes(sectionValue.Left.Name, CurrentTime - sectionValue.startTimeLeft, section.Value));

                        nextSectionValue.Right = sectionValue.Left;
                        nextSectionValue.DistanceRight += sectionValue.DistanceLeft;
                        nextSectionValue.startTimeRight = CurrentTime;
                        sectionValue.Left = null;
                        sectionValue.DistanceLeft = 100;
                    }
                    //Move the right driver
                    else {
                        lapTimes.AddItemToList(new GooseLapTimes(sectionValue.Right.Name, CurrentTime - sectionValue.startTimeLeft, section.Value));

                        nextSectionValue.Right = sectionValue.Right;
                        nextSectionValue.DistanceRight += sectionValue.DistanceRight;
                        nextSectionValue.startTimeRight = CurrentTime;
                        //Reset values on current tile
                        sectionValue.Right = null;
                        sectionValue.DistanceRight = 100;
                    }


                    return true;
                }

                else {
                    return false;
                }
            }

        public int CalcWings(IParticipant goose) {
            return goose.Equipment.Performance * goose.Equipment.Quality * goose.Equipment.Speed;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e) {

            //Loop through sections
            LinkedListNode<Section> section = Track.Sections.Last;
            SectionData sectionValue = GetSectionData(section.Value);

            LinkedListNode<Section> previousSection = section;
            SectionData previousSectionValue = GetSectionData(previousSection.Value);

            for (int i = 0; i < Track.Sections.Count; i++) {
                if (sectionValue.Left != null || sectionValue.Right != null) {

                    if (sectionValue.Left != null) {

                        if (!LetTheWingsFallOff(sectionValue.Left)) {
                            sectionValue.DistanceLeft -= CalcWings(sectionValue.Left);

                            //Add driven distance to list
                            gooseFlownTime.AddItemToList(new GooseFlownTime(sectionValue.Left.Name, CalcWings(sectionValue.Left)));
                        }
                        if (sectionValue.DistanceLeft < 0) {
                            if (!MoveGooseToNextSection(section, section.Next, 0, e.SignalTime)) {
                                sectionValue.DistanceLeft = 0;
                            }
                        }


                    }
                    if (sectionValue.Right != null) {
                        if (!LetTheWingsFallOff(sectionValue.Right)) {
                            sectionValue.DistanceRight -= CalcWings(sectionValue.Right);

                            //Add driven distance to list
                            gooseFlownTime.AddItemToList(new GooseFlownTime(sectionValue.Right.Name, CalcWings(sectionValue.Right)));
                        }
                        if (sectionValue.DistanceRight < 0) {
                            if (!MoveGooseToNextSection(section, section.Next, 1, e.SignalTime)) {
                                sectionValue.DistanceRight = 0;
                            }
                        }
                    }
                }

                if (previousSection.Previous != null) {
                    previousSection = previousSection.Previous;
                    previousSectionValue = GetSectionData(previousSection.Value);
                    section = previousSection;
                    sectionValue = GetSectionData(previousSection.Value);
                }
                else {
                    section = previousSection;
                    sectionValue = GetSectionData(previousSection.Value);

                    previousSection = Track.Sections.First;
                    previousSectionValue = GetSectionData(previousSection.Value);

                }

            }

            GoosesChanged.Invoke(this, new GoosesChangedEventArgs(Track));

            if (goosesRemoved == Participants.Count) {
                Stop();
            }

        }
        public void Start() {
            timer.Enabled = true;
            Console.WriteLine("Race Started");
        }
        public void Stop() {
            timer.Enabled = false;
            CleanReferences();          // Clean up
            Console.WriteLine("Race Stopped");

            Data.NextRace();
            NextRace.Invoke(this, new EventArgs());

        }

        public void CleanReferences() {
            // Clean References
            GoosesChanged = null;
        }


    }
}