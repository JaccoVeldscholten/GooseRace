using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GooseSectionTimes : IGenericScope {
        // This class handles the Goose his Section times
        public string name { get; set; }
        public TimeSpan Time { get; set; }
        public Section Section { get; set; }

        void IGenericScope.Add<T>(List<T> list) {
            // Add section times to goose
            foreach (var goose in list) {
                var gooseSections = goose as GooseSectionTimes;
                if (gooseSections.name == name && gooseSections.Section == Section) {
                    gooseSections.Time = Time;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {
            // Get the best round time
            GooseSectionTimes fastestGoose = null;
            foreach (T goose in list) {
                var currentTime = goose as GooseSectionTimes;
                if (fastestGoose == null) {
                    fastestGoose = currentTime;
                }
                if (currentTime.Time < fastestGoose.Time) {
                    fastestGoose = currentTime;
                }
            }
            return fastestGoose.name;
        }
        
    }
}
