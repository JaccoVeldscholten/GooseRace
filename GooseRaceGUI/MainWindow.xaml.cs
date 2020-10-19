using Model;
using Controller;
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
using RaceGUI;

namespace GooseRaceGUI {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.GoosesChanged += OnGooseChange;
            Data.CurrentRace.NextRace += NextRace;
        }

        public void Initialize() {
            Data.CurrentRace.GoosesChanged += OnGooseChange;
            Data.CurrentRace.NextRace += NextRace;
            //ImageCache.ClearCache();
        }


        public void OnGooseChange(Object sender, GoosesChangedEventArgs e) {
            TrackImage.Dispatcher.BeginInvoke(
            DispatcherPriority.Render,
            new Action(() => {
                TrackImage.Source = null;
                TrackImage.Source = VisualisationGUI.DrawTrack(Data.CurrentRace.Track);
            }));

        }

        public void NextRace(Object source, EventArgs e) {
            Initialize();
        }

    }
}
