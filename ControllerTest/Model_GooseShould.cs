using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest {
    [TestFixture]
    class Model_GooseShould {
        private Competition _competition;

        [SetUp]
        public void SetUp() {
            _competition = new Competition();
        }

        [Test]
        public void GooseName_add_ReturnGooseName() {
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
            Object result = null;
            foreach (Goose goose in _competition.Participants) {
                result = goose.Name;
            }

            Assert.AreEqual(GooseTest.Name, result);
        }

        [Test]
        public void GoosePoints_add_ReturnGoosePoints() {
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
            Object result = null;
            foreach (Goose goose in _competition.Participants) {
                result = goose.Points;
            }

            Assert.AreEqual(GooseTest.Points, result);
        }

        [Test]
        public void GooseEquipment_add_ReturnGooseEquipment() {
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
            Object result = null;
            foreach (Goose goose in _competition.Participants) {
                result = goose.Equipment;
            }

            Assert.AreEqual(GooseTest.Equipment, result);
        }

        [Test]
        public void GooseTeamColor_add_ReturnGooseTeamColor() {
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
            Object result = null;
            foreach (Goose goose in _competition.Participants) {
                result = goose.TeamColor;
            }

            Assert.AreEqual(GooseTest.TeamColor, result);
        }


    }
}
