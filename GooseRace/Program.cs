using Controller;
using System;
using System.Threading;

namespace GooseRace {
    class Program {
        static void Main(string[] args) {
            Data.Initialize();
            Data.NextRace();

            Console.WriteLine($"Track: {Data.CurrentRace.Track.Name}");
            Visualisation.Initialize();
            Visualisation.DrawTrack(Data.CurrentRace.Track);
            //Console.SetWindowSize(10, 60);

            // game loop
            for (; ; )
            {
                Thread.Sleep(3000);
            }
        }
    }
}