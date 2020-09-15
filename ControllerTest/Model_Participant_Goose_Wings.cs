using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest
{
    [TestFixture]
    class Model_Participant_Goose_Wings
    {
        private Competition _competition;

        [SetUp]
        public void Setup(){
            _competition = new Competition();
        }

        [Test]
        public void AddParticipant_And_Check_Wings_Values(){
            IParticipant testGoose = new Goose("Test Goose", new Wings(), TeamColors.Red);
            _competition.Participants.Add(testGoose);
            testGoose.Equipment.IsBroken = true;
            testGoose.Equipment.Performance = 2;
            testGoose.Equipment.Quality = 3;
            testGoose.Equipment.Speed = 4;
            Assert.AreEqual(true, testGoose.Equipment.IsBroken);
            Assert.AreEqual(2, testGoose.Equipment.Performance);
            Assert.AreEqual(3, testGoose.Equipment.Quality);
            Assert.AreEqual(4, testGoose.Equipment.Speed);
        }
    }
}
