using Controller;
using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ControllerTest {
	class Model_SectionData_Should {

		private Competition _competition;
		public static Race CurrentRace { get; set; }

		[SetUp]
		public void Setup() {}

		[Test]
		public void SectionData_Distance_Left() {
			SectionData sec = new SectionData();
			sec.DistanceLeft = 1;
			Assert.AreEqual(sec.DistanceLeft, 1);
		}
		[Test]
		public void SectionData_Distance_Right() {
			SectionData sec = new SectionData();
			sec.DistanceRight = 1;
			Assert.AreEqual(sec.DistanceRight, 1);
		}
		[Test]
		public void SectionData_Distance_LeftTime() {
			SectionData sec = new SectionData();
			DateTime dt = new DateTime();
			sec.TimeLeft = dt;
			Assert.AreEqual(sec.TimeLeft, dt);
		}
		[Test]
		public void SectionData_Distance_RightTime() {
			SectionData sec = new SectionData();
			DateTime dt = new DateTime();
			sec.TimeRight = dt;
			Assert.AreEqual(sec.TimeRight, dt);
		}
		[Test]
		public void SectionData_Get_With_Goose_Left() {
			IParticipant TestGoose = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);
			SectionData sec = new SectionData();
			sec.Left = TestGoose;
			Assert.AreEqual(sec.Left, TestGoose);
		}
		[Test]
		public void SectionData_Get_With_Goose_Right() {
			IParticipant TestGoose = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);
			SectionData sec = new SectionData();
			sec.Right = TestGoose;
			Assert.AreEqual(sec.Right, TestGoose);
		}




	}
}
