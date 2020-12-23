using System;
using System.ComponentModel.Design;
using Controller;
using System.Threading;
using Model;

namespace RaceSimulatorProject {
    class Program {
        static void Main(string[] args) {
            Data.Initialize();
            Data.NextRace();
            Console.SetWindowSize(120, 50);

            Visualisation.Initialize(Data.currentRace);
            Visualisation.DrawTrack(Data.currentRace.Track);
            for (;;){
                
                Thread.Sleep(100);
            }
        }
    }
}
