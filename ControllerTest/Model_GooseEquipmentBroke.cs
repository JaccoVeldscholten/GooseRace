using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest {
	public class Model_GooseEquipmentBroke {

		public Track track;
		public RaceStats<LostWings> wingsLostCounter;

		[SetUp]
		public void Setup() {
			SectionTypes[] testSectionsOne = {
				SectionTypes.StartGrid,
				SectionTypes.LeftCorner,
			};
			track = new Track("TestTrackOne", testSectionsOne);

		}

		[Test]
		public void Add_ToEmptyList() {
			wingsLostCounter = new RaceStats<LostWings>();
			IParticipant TestGoose = new Goose(
				"Dummy",                      // name
				0,                          // points
				new Wings() {              // equipment
					Quality = 10,
					IsBroken = false,
					Performance = 100,
					Speed = 100
				},
				TeamColors.Blue             // color
			);
			LostWings lostWing = new LostWings() { Name = TestGoose.Name, TimesWingLost = 1 };   // Save to generic
			wingsLostCounter.AddRaceStatToList(lostWing);

			Assert.IsNotNull(wingsLostCounter);
		}
	}
}
