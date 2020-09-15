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
            IParticipant testGoose = new Goose("Test Goose", new Wings(), TeamColors.Red);
            _competition.Participants.Add(testGoose);
            Assert.NotZero(_competition.Participants.Count);
        }

        [Test]
        public void AddParticipant_And_Check_Values() {
            IParticipant testGoose = new Goose("Test Goose", new Wings(), TeamColors.Red);
            _competition.Participants.Add(testGoose);
            Assert.IsNotEmpty(testGoose.Name);
            Assert.Zero(testGoose.Points);
            Assert.NotNull(testGoose.Equipment);
            Assert.NotNull(testGoose.Teamcolor);
        }

        [Test]
        public void AddParticipant_And_Check_Points(){
            IParticipant testGoose = new Goose("Test Goose", new Wings(), TeamColors.Red);
            _competition.Participants.Add(testGoose);
            testGoose.Points = 10;
            Assert.AreEqual(10, testGoose.Points);
        }
    }
}
