using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GoosePoints : IGooseStorage {
        
        private int Points { get; set; }
        
        public string Name { get; set; }

        public GoosePoints(String name, int points) {
            Name = name;
            Points = points;
        }

        public void Add<T>(List<T> list) where T: class, IGooseStorage {

            foreach (var goose in list) {
                var currentDriver = goose as GoosePoints;

                if (goose.Name == Name) {
                    currentDriver.Points += Points;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGooseStorage {

            int highestPoints = 0;
            GoosePoints goosePoints = this;

            foreach (var goose in list) {
                var currentDriver = goose as GoosePoints;

                if (currentDriver.Points > highestPoints) {
                    highestPoints = currentDriver.Points;
                    goosePoints = currentDriver;
                }
            }

            return goosePoints.Name;            // Return the goose with highest score
        }

    }
}
