using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest {
	public class Model_Track_Info {

		public Track track;

		[SetUp]
		public void Setup() {

		}

		[Test]
		public void Add_Info_To_Track() {
			SectionTypes[] testSectionsOne = {
				SectionTypes.StartGrid,
				SectionTypes.LeftCorner,
			};
			track = new Track("TestTrackOne", testSectionsOne);
			track.Name = "test";		// Change track
			Assert.AreNotEqual(track.Name, "TestTrackOne");
		}

		[Test]
		public void Add_Get_Section() {

			SectionTypes StartGrid = SectionTypes.StartGrid;
			StartGrid = SectionTypes.Straight;
			Assert.AreNotEqual(StartGrid, SectionTypes.StartGrid);
		}
	}
}
