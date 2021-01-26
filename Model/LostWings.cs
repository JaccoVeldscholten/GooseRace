using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class LostWings : IGenericScope {
        public string Name { get; set; }
        public int TimesWingLost { get; set; }

        void IGenericScope.Add<T>(List<T> list) {                  
            foreach (var goose in list) {
                var goosbroken = goose as LostWings;
                if (goose.Name == Name) {
                    goosbroken.TimesWingLost++;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {   
            // Receiving the best Goose
            int lostMostWings = TimesWingLost;
            LostWings LostMostWings = this;         // Generate Generic
            foreach (var goose in list) {
                var currentPart = goose as LostWings;
                if (currentPart.TimesWingLost > lostMostWings) {
                    TimesWingLost = currentPart.TimesWingLost;
                    LostMostWings = currentPart;
                }
            }
            return LostMostWings.Name;
        }
    }
}
