using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest {
	public class Model_Goose_Stats {

		public Track track;

		[SetUp]
		public void Setup() {

		}

		[Test]
		public void Check_Broken_Wings() {
			SectionTypes[] testSec = {
				SectionTypes.StartGrid,
			};

			RaceStats<GoosePoints> list = new RaceStats<GoosePoints>();
            IParticipant GoosePiet = new Goose("TestGoose", 0, new Wings() {Quality = 10,  IsBroken = false, Performance = 100,Speed = 100  },TeamColors.Blue);
			Track track = new Track("TestTrackOne", testSec);

			// add to list
			list.AddRaceStatToList(new GoosePoints() { name = GoosePiet.Name, Points = 10 });

			object result = list.GetHighest(); // Check who lost most points
			Assert.AreEqual(GoosePiet.Name, result);
		}

		[Test]
		public void Check_Lost_Wings() {
			SectionTypes[] testSec = {
				SectionTypes.StartGrid,
			};

			RaceStats<LostWings> list = new RaceStats<LostWings>();
			IParticipant GoosePiet = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);
			Track track = new Track("TestTrackOne", testSec);

			// add to list
			list.AddRaceStatToList(new LostWings() { name = GoosePiet.Name});

			object result = list.GetHighest();	// Check who lost most wings
			Assert.AreEqual(GoosePiet.Name, result);
		}

		[Test]
		public void Check_SectionSpeed() {
			SectionTypes[] testSec = {
				SectionTypes.StartGrid,
			};

			RaceStats<SectionSpeed> list = new RaceStats<SectionSpeed>();
			IParticipant GoosePiet = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);
			Track track = new Track("TestTrackOne", testSec);

			// add to list
			list.AddRaceStatToList(new SectionSpeed() { Name = GoosePiet.Name });

			object result = list.GetHighest();  // Check who lost most wings
			Assert.AreEqual(GoosePiet.Name, result);
		}

		[Test]
		public void Check_SectionTimes() {
			SectionTypes[] testSec = {
				SectionTypes.StartGrid,
			};

			RaceStats<GooseSectionTimes> list = new RaceStats<GooseSectionTimes>();
			IParticipant GoosePiet = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);
			Track track = new Track("TestTrackOne", testSec);

			// add to list
			list.AddRaceStatToList(new GooseSectionTimes() { name = GoosePiet.Name });

			object result = list.GetHighest();  // Check who lost most wings
			Assert.AreEqual(GoosePiet.Name, result);
		}

		[Test]
		public void Empty_List_Check() {
			RaceStats<GoosePoints> list = new RaceStats<GoosePoints>();
			object result = list.GetHighest(); 
			Assert.AreEqual("", result);
		}

		[Test]
		public void Check_Race_Stat_List() {
			RaceStats<SectionSpeed> list = new RaceStats<SectionSpeed>();
			IParticipant GoosePiet = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);

			// add to list
			list.AddRaceStatToList(new SectionSpeed() { Name = GoosePiet.Name });

			Assert.AreNotEqual(list, list.GetRaceStatList());
		}
		[Test]
		public void Check_Race_StatSpeed() {
			SectionSpeed sec = new SectionSpeed();
			sec.Speed = 10;
			Assert.AreEqual(sec.Speed, 10);
		}
	}
}
