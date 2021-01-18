using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Controller;
using Model;

namespace GooseGUI
{
    class RaceStatisticsDataContext : INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        public Race CurrentRace { get; set; }
        private RaceStats<GooseSectionTimes> sectionTimeStorage;
        public List<GooseSectionTimes> SectionTimes { get; set; }

        public List<IParticipant> Gooses { get; set; }
        public string BestSectionTime { get; set; }
        public List<LostWings> brokenCounter;
        public List<SectionSpeed> sectionSpeed;
        public Dictionary<IParticipant, int> lapsGooses;
        public List<IParticipant> WinnerStats;

        public RaceStatisticsDataContext() {
            SectionTimes = new List<GooseSectionTimes>();
        }

        public void OnNextRace(object sender, NextRaceEventArgs e) {
            CurrentRace = e.Race;
            sectionTimeStorage = CurrentRace.RaceStatRoundtime;
            
            e.Race.GoosesChanged += OnGoosesChanged;
        }

        public void OnGoosesChanged(object sender, GoosesChangedEventArgs e) {
            // On every Change the data of the Goose will be fetched and updated
            SectionTimes = sectionTimeStorage.GetRaceStatList();
            Gooses = CurrentRace.Gooses;

            lapsGooses = CurrentRace.Laps;                                  // Receiveing all laps 
            brokenCounter = CurrentRace.wingsLostCounter.GetRaceStatList();    // Receiving Stats broken
            sectionSpeed = CurrentRace.sectionSpeed.GetRaceStatList();      // Receiving section speeds

            WinnerStats = CurrentRace.WinnerList;               // Winners

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));

        }
    }
}
