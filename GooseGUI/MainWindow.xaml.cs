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

    public partial class MainWindow : Window {

        private CurrentRaceStatistics CurRaceStats;
        private PartCompStatistics GooseStats;
        private RaceStatisticsDataContext RaceStats;

        public MainWindow() {
            InitializeComponent();
    
            Images.Init();          // Init images Lib
            Data.Init();

            RaceStats = new RaceStatisticsDataContext();
            GooseStats = new PartCompStatistics();
            CurRaceStats = new CurrentRaceStatistics();

            Data.NextRaceEvent += RaceStats.OnNextRace;

            Data.NextRaceEvent += OnNextRaceEvent;
            Data.NextRace();
            
        }

        private void OnNextRaceEvent(object sender, NextRaceEventArgs e) {           
            Images.ClearCache();
            Visualisation.Initialize(e.Race);

            e.Race.GoosesChanged += OnGoosesChanged;
            e.Race.RaceIsFinished += OnRaceIsFinished;
            
            this.Dispatcher.Invoke(() =>
            {
                e.Race.GoosesChanged += ((DataContext)this.DataContext).OnGoosesChanged;        
            });
            
        }

        protected virtual void OnGoosesChanged(object source, GoosesChangedEventArgs e) {       
            // Receive new Image every race
            this.RaceCanvas.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    this.RaceCanvas.Source = null;
                    this.RaceCanvas.Source = Visualisation.DrawTrack(e.Track);
                }));
            this.Dispatcher.Invoke(() =>                    
            // Refresh the data for stats
            {
                CurRaceStats.participant.Items.Refresh();
                CurRaceStats.sectionTimeData.Items.Refresh();
                CurRaceStats.lapsPerParticipant.Items.Refresh();

                GooseStats.brokenCounter.Items.Refresh();
                GooseStats.sectionSpeed.Items.Refresh();
                GooseStats.partPoints.Items.Refresh();
            });
        }

        public static void OnRaceIsFinished(object soure, EventArgs e) {        
            // Call Next Race when is Finsihed
            Data.NextRace();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) {    
            // Close the application on button press
            Application.Current.Shutdown();
        }

        private void MenuItem_OpenPartAndCompStatistics_Click(object sender, RoutedEventArgs e) {     
            // Open competion screen and receive the information to display
            GooseStats.sectionSpeed.ItemsSource = RaceStats.sectionSpeed;
            GooseStats.brokenCounter.ItemsSource = RaceStats.brokenCounter;
            GooseStats.partPoints.ItemsSource = RaceStats.WinnerStats;

            GooseStats.Show();
        }

        private void MenuItem_CurrentRaceStatistics_Click(object sender, RoutedEventArgs e) {         
            // Open stats screen and receive the infromation display
            CurRaceStats.sectionTimeData.ItemsSource = RaceStats.SectionTimes;
            CurRaceStats.participant.ItemsSource = RaceStats.Gooses;
            CurRaceStats.lapsPerParticipant.ItemsSource = RaceStats.lapsGooses;

            CurRaceStats.Show();
        }
    }
}
