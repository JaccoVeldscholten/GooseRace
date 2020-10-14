using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    // Storage Procedure of the Goose
    public interface IGooseStorage {

        // Name of the Goose
        public string Name { get; set; }

        // Add Data to generic list
        public void Add<T>(List<T> list) where T : class, IGooseStorage;

        // Get The best Goose
        public string GetBest<T>(List<T> list) where T : class, IGooseStorage;

    }
}
