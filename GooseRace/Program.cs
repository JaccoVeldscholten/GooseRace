using Controller;
using System;
using System.Threading;

namespace GooseRace {
    class Program {
        static void Main(string[] args) {
            Data.Initialize();

            for (; ; ){
                Data.NextRace();
                //Console.WriteLine(Data.CurrentRace.Track.Name);
                Thread.Sleep(100);
            }

        }
    }
}
