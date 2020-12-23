using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Controller;
using Model;


namespace GooseGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private CurrentRaceStatistics curRaceStats;
        private PartCompStatistics partCompStats;
        private RaceStatisticsDataContext raceStats;

        public MainWindow() {
            InitializeComponent();
    
            Images.Init();
            Data.Init();

            raceStats = new RaceStatisticsDataContext();            // make all screens in advance to add data
            curRaceStats = new CurrentRaceStatistics();
            partCompStats = new PartCompStatistics();

            Data.NextRaceEvent += raceStats.OnNextRace;

            Data.NextRaceEvent += OnNextRaceEvent;
            Data.NextRace();
            
        }

        private void OnNextRaceEvent(object sender, NextRaceEventArgs e) {              // connect events when next race is called
            Images.ClearCache();
            Visualisation.Initialize(e.Race);

            e.Race.GoosesChanged += OnGoosesChanged;
            e.Race.RaceIsFinished += OnRaceIsFinished;
            
            this.Dispatcher.Invoke(() =>
            {
                e.Race.GoosesChanged += ((DataContext)this.DataContext).OnGoosesChanged;          // add event
            });
            
        }

        protected virtual void OnGoosesChanged(object source, GoosesChangedEventArgs e) {         // change track images when drives change position
            this.RaceCanvas.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    this.RaceCanvas.Source = null;
                    this.RaceCanvas.Source = Visualisation.DrawTrack(e.track);
                }));
            this.Dispatcher.Invoke(() =>                        // refresh all lists to show new data
            {
                curRaceStats.participant.Items.Refresh();
                curRaceStats.sectionTimeData.Items.Refresh();
                curRaceStats.lapsPerParticipant.Items.Refresh();

                partCompStats.brokenCounter.Items.Refresh();
                partCompStats.sectionSpeed.Items.Refresh();
                partCompStats.partPoints.Items.Refresh();
            });
        }

        public static void OnRaceIsFinished(object soure, EventArgs e) {        // when race is finished, call next track
            Data.NextRace();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) {    // exit app when pressed exit
            Application.Current.Shutdown();
        }

        private void MenuItem_OpenPartAndCompStatistics_Click(object sender, RoutedEventArgs e) {       // open competition screen
            partCompStats.sectionSpeed.ItemsSource = raceStats.sectionSpeed;
            partCompStats.brokenCounter.ItemsSource = raceStats.brokenCounter;
            partCompStats.partPoints.ItemsSource = raceStats.participantsFinishOrder;

            partCompStats.Show();
        }

        private void MenuItem_CurrentRaceStatistics_Click(object sender, RoutedEventArgs e) {           // open race statistics
            curRaceStats.sectionTimeData.ItemsSource = raceStats.SectionTimes;
            curRaceStats.participant.ItemsSource = raceStats.Participants;
            curRaceStats.lapsPerParticipant.ItemsSource = raceStats.lapsPerParticipant;

            curRaceStats.Show();
        }
    }
}
