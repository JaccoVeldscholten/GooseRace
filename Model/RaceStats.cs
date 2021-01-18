using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class RaceStats<T> where T : class, IGenericScope {              // generic class for other data classes
        private List<T> _list = new List<T>();
        
        public void AddRaceStatToList(T stat) {
            stat.Add(_list);
        }

        public List<T> GetRaceStatList() {
            return _list;
        }

        public string GetHighest() {
            if (_list.Count > 0) {
                return _list[0].GetBest(_list);
            } else {
                return "";
            }

        }
    }
}
