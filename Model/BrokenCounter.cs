using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class BrokenCounter : IGenericScope {
        public string name { get; set; }
        public int timesBroken { get; set; }

        void IGenericScope.Add<T>(List<T> list) {                   // add to list
            foreach (var part in list) {
                var currentDriver = part as BrokenCounter;
                if (part.name == name) {
                    currentDriver.timesBroken++;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGenericScope {     // looping through list, return best
            int brokeDownMost = timesBroken;
            BrokenCounter BrokenDownAmountObj = this;
            foreach (var part in list) {
                var currentPart = part as BrokenCounter;
                if (currentPart.timesBroken > brokeDownMost) {
                    timesBroken = currentPart.timesBroken;
                    BrokenDownAmountObj = currentPart;
                }
            }
            return BrokenDownAmountObj.name;
        }
    }
}
