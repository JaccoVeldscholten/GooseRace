using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest {
    class Model_RaceData_Should {

		delegate void GoosesChangedEventHandler(object source, GoosesChangedEventArgs args);

		event GoosesChangedEventHandler GoosesChanged;

		[SetUp]
		public void Setup() {

		}

		[Test]
		public void GetTrack() {
			RaceData rd = new RaceData();
			rd.TrackName = "Track";
			object res = rd.TrackName;
			Assert.AreEqual(res, "Track");
		}

		[Test]
		public void GetEvent() {
			SectionTypes[] testSec = {
				SectionTypes.StartGrid,
			};

			IParticipant GoosePiet = new Goose("TestGoose", 0, new Wings() { Quality = 10, IsBroken = false, Performance = 100, Speed = 100 }, TeamColors.Blue);
			Track track = new Track("TestTrackOne", testSec);
			Assert.Pass();
		}

		protected virtual void OnGoosesChanged(Track track) {
			GoosesChanged?.Invoke(this, new GoosesChangedEventArgs() { Track = track });
		}
	}
}
