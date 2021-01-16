using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;
using Controller;

namespace ControllerTest {

    [TestFixture]
    class Model_BrokenCounterShould {
        private Competition _competition;

        [SetUp]
        public void SetUp() {
            _competition = new Competition();
        }
    }
}