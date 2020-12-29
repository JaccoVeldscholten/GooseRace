using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GoosePoints : IGenericScope {
        public string name { get; set; }
        public int Points { get; set; }

        void IGenericScope.Add<T>(List<T> list) {  
            // Add points to the goose
            foreach (var goose in list) {
                var goosePoints = goose as GoosePoints;
                if (goose.name == name) {
                    goosePoints.Points += Points;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {      
            // Get the goose with highest score
            int best = 0;
            GoosePoints goosePointsObj = this;
            foreach (var goose in list) {
                var goosePoints = goose as GoosePoints;

                if (goosePoints.Points > best) {
                    best = goosePoints.Points;
                    goosePointsObj = goosePoints;
                }
            }
            return goosePointsObj.name;
        }
    }
}
