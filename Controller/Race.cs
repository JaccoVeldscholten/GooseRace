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

        public delegate void onDriversChanged(object Sender, DriversChangedEventArgs driversChangedEventArgs);
        public delegate void onNextRace(object Sender, EventArgs nextRaceEventArgs);

        public event onDriversChanged DriversChanged;
        public event onNextRace NextRace;

        private int amountOfLaps = 2;
        private Dictionary<IParticipant, int> DrivenRounds = new Dictionary<IParticipant, int>();
        private int driversRemoved;


        public Race(Track track, List<IParticipant> participants) {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            RandomizeEquipment();

            // Fill Dictornary with Gooses
            foreach (IParticipant p in Participants) {
                DrivenRounds.Add(p, 0);
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
                //goose.Equipment.Quality = 10;
                goose.Equipment.Quality = _random.Next(1, 100);
                goose.Equipment.Performance = _random.Next(1, 100);

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


        private bool letTheWingsFallOff(IParticipant participant) {
            //if not broken
            if (!participant.Equipment.IsBroken) {
                //create chance to be broke
                if (_random.Next(1, 100) == 1) {
                    participant.Equipment.IsBroken = true;
                    Console.WriteLine("Broken!");
                    return true;
                }
                //Car stays healthy
                else {
                    return false;

                }
            }
            //Create change to b  e repaired
            else {
                //Create chance to be repaired
                if (_random.Next(1, 10) == 1) {
                    participant.Equipment.IsBroken = false;
                    //Quality will be lowered if possible
                    if (participant.Equipment.Quality > 1) participant.Equipment.Quality -= 1;
                    return false;
                }
                //Car will still be broken
                else {
                    return true;

                }
            }
        }

        public bool DriverToNextSection(LinkedListNode<Section> section, LinkedListNode<Section> nextSection, int LeftRight) {

            SectionData sectionValue = GetSectionData(section.Value);
            SectionData nextSectionValue;

            //Check if last section
            if (Track.Sections.Last == section) {
                nextSectionValue = GetSectionData(Track.Sections.First.Value);
            }
            else {
                nextSectionValue = GetSectionData(nextSection.Value);
            }

            //Check if left or right driver
            if (LeftRight == 0) {

                //Check if left driver is crossing finish
                if (section.Value.SectionType == SectionTypes.Finish) {
                    
                     DrivenRounds[sectionValue.Left] += 1;
                     if (DrivenRounds[sectionValue.Left] == amountOfLaps + 1) {
                         sectionValue.Left = null;
                         sectionValue.DistanceLeft = 100;
                         driversRemoved++;

                         return true;
                     }
                }
            }
            else {
                //Check if left driver is crossing finish
                if (section.Value.SectionType == SectionTypes.Finish) {
                    DrivenRounds[sectionValue.Right] += 1;

                    if (DrivenRounds[sectionValue.Right] == amountOfLaps + 1) {
                        sectionValue.Right = null;
                        sectionValue.DistanceRight = 100;
                        driversRemoved++;

                        return true;
                    }
                }
            }


            //Move right driver
            if (nextSectionValue.Left == null) {
                //Move to left section
                if (LeftRight == 0) {
                    nextSectionValue.Left = sectionValue.Left;
                    nextSectionValue.DistanceLeft += sectionValue.DistanceLeft;
                    //Reset values on current tile
                    sectionValue.Left = null;
                    sectionValue.DistanceLeft = 100;
                }
                //Move to right section
                else {
                    nextSectionValue.Left = sectionValue.Right;
                    nextSectionValue.DistanceLeft += sectionValue.DistanceRight;
                    //Reset values on current tile
                    sectionValue.Right = null;
                    sectionValue.DistanceRight = 100;
                }
                return true;
            }
            //Move left driver
            else if (nextSectionValue.Right == null) {
                //Move to left section
                if (LeftRight == 0) {
                    nextSectionValue.Right = sectionValue.Left;
                    nextSectionValue.DistanceRight += sectionValue.DistanceLeft;
                    sectionValue.Left = null;
                    sectionValue.DistanceLeft = 100;
                }
                //Move to right section
                else {
                    nextSectionValue.Right = sectionValue.Right;
                    nextSectionValue.DistanceRight += sectionValue.DistanceRight;
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

        public int calculateDistanceForCar(IParticipant driver) {
            return driver.Equipment.Performance * driver.Equipment.Quality * driver.Equipment.Speed;
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

                        if (!letTheWingsFallOff(sectionValue.Left)) {
                            sectionValue.DistanceLeft -= calculateDistanceForCar(sectionValue.Left);
                        }
                        if (sectionValue.DistanceLeft < 0) {
                            if (!DriverToNextSection(section, section.Next, 0)) {
                                sectionValue.DistanceLeft = 0;
                            }
                        }


                    }
                    if (sectionValue.Right != null) {
                        if (!letTheWingsFallOff(sectionValue.Right)) {
                            sectionValue.DistanceRight -= calculateDistanceForCar(sectionValue.Right);
                        }
                        if (sectionValue.DistanceRight < 0) {
                            if (!DriverToNextSection(section, section.Next, 1)) {
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

            DriversChanged.Invoke(this, new DriversChangedEventArgs(Track));

            if (driversRemoved == Participants.Count) {
                Stop();
            }
        }
        public void Start() {
            timer.Enabled = true;
            Console.WriteLine("Race Started");
        }
        public void Stop() {
            timer.Enabled = false;
            cleanReferences();          // Clean up
            Console.WriteLine("Race Stopped");

            Data.NextRace();
            NextRace.Invoke(this, new EventArgs());

        }

        public void cleanReferences() {
            // Clean References
            DriversChanged = null;
        }


    }
}