using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class RaceInformation<T> where T : class, IGooseStorage{

        // Generic List
        private List<T> _list = new List<T>();

        // Add item to Goose
        public void AddItemToList(T item) {
            item.Add(_list);
        }

        // Get Beste Goose
        public string GetHighest() {
            if (_list.Count > 0) {
                return _list[0].GetBest(_list);
            }
            return " ";
        }

    }
}
