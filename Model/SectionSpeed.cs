using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class SectionSpeed : IGenericScope {
        public string name { get; set; }
        public int speed { get; set; }
        public Section section { get; set; }

        void IGenericScope.Add<T>(List<T> list) {                           // adding new section speed to list
            foreach (var part in list) {
                var currentPart = part as SectionSpeed;
                if (currentPart.name == name && currentPart.section == section) {
                    currentPart.speed = speed;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {             // get best participant with highest speed
            int highSpeed = this.speed;
            SectionSpeed partSectionSpeed = this;

            foreach (var part in list) {
                var currentPart = part as SectionSpeed;
                if (currentPart.speed > speed) {
                    speed = currentPart.speed;
                    partSectionSpeed = currentPart;
                }
            }
            return partSectionSpeed.name;
        }
    }
}
