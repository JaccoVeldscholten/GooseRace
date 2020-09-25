using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest {
    class Model_Participant_Goose_Values {
        private Competition _competition;

        [SetUp]
        public void SetUp() {
            _competition = new Competition();
        }

        [Test]
        public void GooseName_add_ReturnGooseName() {
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);

            _competition.Participants.Add(GooseTest);
            Object result = null;
            foreach (Goose goosieboi in _competition.Participants) {
                result = goosieboi.Name;
            }

            Assert.AreEqual(GooseTest.Name, result);
        }

        [Test]
        public void GoosePoints_add_ReturnGoosePoints() {
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            Object result = null;
            foreach (Goose goosieboi in _competition.Participants) {
                result = goosieboi.Points;
            }

            Assert.AreEqual(GooseTest.Points, result);
        }

        [Test]
        public void GooseEquipment_add_ReturnGooseEquipment() {
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            Object result = null;
            foreach (Goose goosieboi in _competition.Participants) {
                result = goosieboi.Equipment;
            }

            Assert.AreEqual(GooseTest.Equipment, result);
        }

        [Test]
        public void GooseTeamColor_add_ReturnGooseTeamColor() {
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            Object result = null;
            foreach (Goose goosieboi in _competition.Participants) {
                result = goosieboi.TeamColor;
            }

            Assert.AreEqual(GooseTest.TeamColor, result);
        }
    }
}
