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
        // race
        public event PropertyChangedEventHandler PropertyChanged;
        public Race CurrentRace { get; set; }
        private RaceStats<SectionRoundtime> sectionTimeStorage;
        public List<SectionRoundtime> SectionTimes { get; set; }

        // competition
        public List<IParticipant> Participants { get; set; }
        public string BestSectionTime { get; set; }
        public List<BrokenCounter> brokenCounter;
        public List<SectionSpeed> sectionSpeed;
        public Dictionary<IParticipant, int> lapsPerParticipant;
        public List<IParticipant> participantsFinishOrder;

        public RaceStatisticsDataContext()
        {
            SectionTimes = new List<SectionRoundtime>();
        }

        public void OnNextRace(object sender, NextRaceEventArgs e)
        {
            CurrentRace = e.Race;
            sectionTimeStorage = CurrentRace.RaceStatRoundtime;
            
            e.Race.GoosesChanged += OnGoosesChanged;
        }

        public void OnGoosesChanged(object sender, GoosesChangedEventArgs e)      // every change of drivers: update the lists with new data
        {
            SectionTimes = sectionTimeStorage.getRaceStatList();
            Participants = CurrentRace.Participants;

            //BestSectionTime = sectionTimeStorage.GetHighest();
            brokenCounter = CurrentRace.brokenCounter.getRaceStatList();
            sectionSpeed = CurrentRace.sectionSpeed.getRaceStatList();
            lapsPerParticipant = CurrentRace.lapsPerParticipant;
            participantsFinishOrder = CurrentRace.participantsFinishOrder;


            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));

        }
    }
}
