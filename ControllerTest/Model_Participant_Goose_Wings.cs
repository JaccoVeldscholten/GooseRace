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
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            GooseTest.Equipment.IsBroken = true;
            GooseTest.Equipment.Performance = 2;
            GooseTest.Equipment.Quality = 3;
            GooseTest.Equipment.Speed = 4;
            Assert.AreEqual(true, GooseTest.Equipment.IsBroken);
            Assert.AreEqual(2, GooseTest.Equipment.Performance);
            Assert.AreEqual(3, GooseTest.Equipment.Quality);
            Assert.AreEqual(4, GooseTest.Equipment.Speed);
        }
    }
}
