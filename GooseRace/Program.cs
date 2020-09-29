using Controller;
using System;
using System.Threading;

namespace GooseRace {
    class Program {
        static void Main(string[] args) {
            Data.Initialize();
            Data.NextRace();
            Data.NextRace();
            Data.NextRace();
            Console.SetWindowSize(180, 60);

            Console.WriteLine($"Track: {Data.CurrentRace.Track.Name}");

            Visualisation.Initialize(Data.CurrentRace);
            Visualisation.DrawTrack(Data.CurrentRace.Track);

            // game loop
            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}