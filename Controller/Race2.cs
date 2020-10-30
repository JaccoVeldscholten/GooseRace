using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace Controller {
    class Race2 {

        public Track Track { get; set; }
        public List<IParticipant> Gooses { get; set; }
        public DateTime StartTime { get; set; }

        private Random _random;
        private Timer _timer;
        private DateTime endTime;

        private Dictionary<Section, SectionData> _positions;
        private Dictionary<IParticipant, int> _lapsFlown;
        private List<IParticipant> _winOrder;
        private EventHandler<GoosesChangedEventArgs> GooseChanged;

        private SectionData finish;

        public event EventHandler RaceFinished;

        #region constants
        private const int timerInterval = 500;
        private const int amountOfLaps = 2;
        #endregion

        // Storage
        private RaceInformation<GooseLapTimes> _LapTime;
        private RaceInformation<GooseFlownTime> _flownTime;
        private RaceInformation<GooseLostWingAmount> _lostWingAmount;


        // Main Timer to Handle every Game Tick
        public void OnTimedEvent(object sender, ElapsedEventArgs e) {
            RandomizeChangeOfBrokenWings();
            RandomizeChangeOfFixingWings();

            MoveGooses(e.SignalTime);
        }


        private void MoveGooses(DateTime elapsedLapTime) {

        }

        // This function will give a chance to a Goose to get his Wings fixed
        private void RandomizeChangeOfFixingWings() {
            foreach (IParticipant goose in Gooses.Where(p => p.Equipment.IsBroken)) {
                if(_random.Next(0,20) == 5) {
                    Debug.WriteLine($"{goose.Name} Got his wings fixed!");
                    goose.Equipment.IsBroken = false;                           // Fixed Wings

                    if (goose.Equipment.Speed > 10) {
                        goose.Equipment.Speed--;                                // Downgrade Speed
                    }
                    if (goose.Equipment.Quality > 10) {
                        goose.Equipment.Speed--;                                // Downgrade Quality
                    }
                }

            }
        }

        // This function will give the goose a chance to lost his wings...
        private void RandomizeChangeOfBrokenWings() {
            List<IParticipant> gooseFlowing = _positions.Values.Where(a => a.Left != null).Select(a => a.Left).Concat(_positions.Values.Where(a => a.Right != null).Select(a => a.Right)).ToList();
            foreach(IParticipant goose in gooseFlowing) {
                if (_random.Next(0,20) == 5) {
                    Debug.WriteLine($"{goose.Name} Lost his wings!");
                    goose.Equipment.IsBroken = true;
                }
            }
        }




        // This function will RandomMize the Gooses there Equipment
        private void RandomizeEquipment() {
            foreach(IParticipant goose in Gooses) {
                goose.Equipment.Performance = _random.Next(1, 11); // Give those wings power OR NOT
                goose.Equipment.Quality     = _random.Next(1, 20); // Give those wings ability to fell off..
            }
        }

        public SectionData GetSectionData(Section sec) {
            if (!_positions.ContainsKey(sec)) {
                _positions.Add(sec, new SectionData());
            }
            return _positions[sec];
        }

        // Setting the right Finish data (Typedef looklike)
        private void InitFinish() {
            finish = GetSectionData(Track.Sections.First(a => a.SectionType == SectionTypes.Finish));
        }

        // This function fills the Storage with empty data
        private void FillRace() {
            _lapsFlown = new Dictionary<IParticipant, int>();

            foreach(IParticipant goose in Gooses) {
                _lapsFlown.Add(goose, -1);              // Start at -1 Because it goes over Finish...
            }
        }

        // This function will Receive the Side the Goose is standing
        private void DetermineSide(IParticipant p, bool side, Section section) {
            if (side) {
                GetSectionData(section).Right = p;
            }
            else {
                GetSectionData(section).Left = p;
            }
        }

        // This Function will Filter all the Sections with StartGrids
        private List<Section> FilterOnStartGrid() {
            List<Section> startGrids = new List<Section>();

            foreach (Section trackSec in Track.Sections) {
                if (trackSec.SectionType == SectionTypes.StartGrid) {
                    startGrids.Add(trackSec);
                }
            }
            startGrids.Reverse();
            return startGrids;
        }

        // This Function will place all the Gooses (Participants) on the start Grid.
        private void FillStart() {
            List<Section> startGrid = FilterOnStartGrid();
            int startPos = 0;
            int startIndex = 0;
            bool gooseSide = false;

            if (Gooses.Count >= startGrid.Count * 2) {
                startPos = startGrid.Count * 2;            // Assign start position
            }
            else if (Gooses.Count < startGrid.Count * 2) {
                startPos = Gooses.Count;
            }

            for (int i = 0; i < startPos; i++) {
                DetermineSide(Gooses[i], gooseSide, startGrid[startIndex]);
                gooseSide = !gooseSide;
                if (i % 2 == 1) {
                    startIndex++;               // Every uneven section up
                }
            }
        }

        private void InitTimer() {
            _timer = new Timer(timerInterval);
            _timer.Elapsed += OnTimedEvent;
        }

        public Race2(Track track, List<IParticipant> gooses) {
            Track = track;
            Gooses = gooses;
            _random         = new Random(DateTime.Now.Millisecond);
            _positions      = new Dictionary<Section, SectionData>();
            _winOrder       = new List<IParticipant>(0);
            _LapTime        = new RaceInformation<GooseLapTimes>();
            _flownTime      = new RaceInformation<GooseFlownTime>();
            _lostWingAmount = new RaceInformation<GooseLostWingAmount>();

            InitTimer();          // Call Timer Initializer
            FillStart();          // Place Goosebois on grid
            FillRace();           // Set all Goosebois on 0 laps flown
            InitFinish();         // Set Arugment to receive Finish data (For when Goose passes)
            RandomizeEquipment(); // Give Goosebois some chances


        }
    }
}
