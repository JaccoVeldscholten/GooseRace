using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class SectionSpeed : IGenericScope {
        public string Name { get; set; }
        public int Speed { get; set; }
        public Section Section { get; set; }

        void IGenericScope.Add<T>(List<T> list) {                           // adding new section speed to list
            foreach (var part in list) {
                var currentPart = part as SectionSpeed;
                if (currentPart.Name == Name && currentPart.Section == Section) {
                    currentPart.Speed = Speed;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {             // get best participant with highest speed
            _ = this.Speed;
            SectionSpeed partSectionSpeed = this;

            foreach (var part in list) {
                var currentPart = part as SectionSpeed;
                if (currentPart.Speed > Speed) {
                    Speed = currentPart.Speed;
                    partSectionSpeed = currentPart;
                }
            }
            return partSectionSpeed.Name;
        }
    }
}
