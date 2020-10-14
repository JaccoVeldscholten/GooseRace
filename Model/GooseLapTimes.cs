using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GooseLapTimes : IGooseStorage {
        public TimeSpan Time { get; set; }
        public Section Section { get; set; }
        public string Name { get; set; }

        public GooseLapTimes(string name, TimeSpan time, Section section) {
            Name = name;
            Time = time;
            Section = section;
        }

        public void Add<T>(List<T> list) where T : class, IGooseStorage {
            foreach (var goose in list) {
                var currentgoose = goose as GooseLapTimes;
                if (currentgoose.Name == Name && currentgoose.Section == Section) {
                    currentgoose.Time = Time;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGooseStorage {
            TimeSpan FastestSector = this.Time;
            GooseLapTimes gooseSectionTimes = this;

            foreach (var goose in list) {
                var currentgoose = goose as GooseLapTimes;

                if (currentgoose.Time > Time) {
                    Time = currentgoose.Time;
                    gooseSectionTimes = currentgoose;
                }
            }

            return gooseSectionTimes.Name;
        }
    }
}