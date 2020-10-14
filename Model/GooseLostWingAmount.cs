using System;
using System.Collections.Generic;
using System.Text;

namespace Model {
    public class GooseLostWingAmount : IGooseStorage {

        public int BrokenDownAmount { get; set; }
        public string Name { get; set; }

        public GooseLostWingAmount(string name, int brokenDownAmount) {
            Name = name;
            BrokenDownAmount = brokenDownAmount;
        }

        public void Add<T>(List<T> list) where T : class, IGooseStorage {
            foreach (var driver in list) {
                var currentDriver = driver as GooseLostWingAmount;

                if (driver.Name == Name) {
                    currentDriver.BrokenDownAmount++;
                    return;
                }
            }
            list.Add(this as T);
        }

        public string GetBest<T>(List<T> list) where T : class, IGooseStorage {
            int MostBrokenDown = BrokenDownAmount;
            GooseLostWingAmount BrokenDownAmountObj = this;

            foreach (var driver in list) {
                var currentDriver = driver as GooseLostWingAmount;

                if (currentDriver.BrokenDownAmount > BrokenDownAmount) {
                    BrokenDownAmount = currentDriver.BrokenDownAmount;
                    BrokenDownAmountObj = currentDriver;
                }
            }

            return BrokenDownAmountObj.Name;
        }
    }
}