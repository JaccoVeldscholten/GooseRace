using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public interface IGenericScope {
        public string Name { get; set; }
        public void Add<T>(List<T> list) where T : class, IGenericScope;
        public string GetBest<T>(List<T> list) where T : class, IGenericScope;
    }
}
