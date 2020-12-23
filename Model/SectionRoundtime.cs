using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class SectionRoundtime : IGenericScope {
        public string name { get; set; }
        public TimeSpan time { get; set; }
        public Section section { get; set; }

        void IGenericScope.Add<T>(List<T> list) {
            foreach (var part in list) {
                var currentPart = part as SectionRoundtime;
                if (currentPart.name == name && currentPart.section == section) {
                    currentPart.time = time;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {
            SectionRoundtime partBestTime = null;
            foreach (T part in list) {
                var currentTime = part as SectionRoundtime;
                if (partBestTime == null) {
                    partBestTime = currentTime;
                }
                if (currentTime.time < partBestTime.time) {
                    partBestTime = currentTime;
                }
            }
            return partBestTime.name;
        }
        
    }
}
