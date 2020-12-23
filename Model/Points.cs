using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class Points : IGenericScope {
        public string name { get; set; }
        public int points { get; set; }

        void IGenericScope.Add<T>(List<T> list) {               // add new points with part to list
            foreach (var part in list) {
                var currentDriver = part as Points;
                if (part.name == name) {
                    currentDriver.points += points;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {         // return best participant, looking at amount of points
            int highPoints = 0;
            Points partPoints = this;
            foreach (var part in list) {
                var currentPart = part as Points;

                if (currentPart.points > highPoints) {
                    highPoints = currentPart.points;
                    partPoints = currentPart;
                }
            }
            return partPoints.name;
        }
    }
}
