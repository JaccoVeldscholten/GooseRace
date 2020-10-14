using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GooseFlownTime : IGooseStorage{

        private int Distance { get; set; }
        public string Name { get; set; }

        public GooseFlownTime(string name, int distance) {
            Name = name;
            Distance = distance;
        }

        public void Add<T>(List<T> list) where T : class, IGooseStorage {
            foreach (var Goose in list) {
                var currentGoose = Goose as GooseFlownTime;
                if (currentGoose.Name == Name) {
                    currentGoose.Distance += Distance;
                    return;
                }
            }
            list.Add(this as T);
        }


        public string GetBest<T>(List<T> list) where T : class, IGooseStorage {
            int DistandeFlown = Distance;
            GooseFlownTime DistandeFlownObj = this;

            foreach (var Goose in list) {
                var currentGoose = Goose as GooseFlownTime;

                if (currentGoose.Distance > Distance) {
                    Distance = currentGoose.Distance;
                    DistandeFlownObj = currentGoose;
                }
            }

            return DistandeFlownObj.Name;
        }
    }
}
