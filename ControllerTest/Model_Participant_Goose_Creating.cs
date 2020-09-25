using System;
using System.Collections.Generic;
using System.Text;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Model_Pariticpant_Goose_Creating {

        private Competition _competition;

        [SetUp]
        public void Setup(){
            _competition = new Competition();
        }

        [Test]
        public void AddParticipant_And_Check_Count(){
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            Assert.NotZero(_competition.Participants.Count);
        }

        [Test]
        public void AddParticipant_And_Check_Values() {
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            Assert.IsNotEmpty(GooseTest.Name);
            Assert.Zero(GooseTest.Points);
            Assert.NotNull(GooseTest.Equipment);
            Assert.NotNull(GooseTest.TeamColor);
        }

        [Test]
        public void AddParticipant_And_Check_Points(){
            IParticipant GooseTest = new Goose("Test", 0, new Wings(10, 10, 10, false), TeamColors.Blue);
            _competition.Participants.Add(GooseTest);
            GooseTest.Points = 10;
            Assert.AreEqual(10, GooseTest.Points);
        }
    }
}
