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
        /*
        [Test]
        public void BrokenCounter_GetBroken_ReturnEqual () {
            IParticipant GooseTest = new Goose(
                "Test",                     // name
                0,                          // points
                new Wings() {              // equipment
                    Quality = 100,
                    IsBroken = false,
                    Performance = 100,
                    Speed = 0
                },
                TeamColors.Blue             // color
            );

            _competition.Participants.Add(GooseTest);
            Track track = _competition.NextTrack();
            Race currentRace = new Race(track, _competition.Participants);
            _competition.GiveTimesBroken(currentRace.getBrokenCounter());

            Assert.AreEqual(_competition.brokeCounter, null);
        }
        */
    }
}