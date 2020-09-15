using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest  {
    [TestFixture]
    class Model_Competition_NextTrackShould {

        private Competition _competition;

        [SetUp]
        public void Setup() {
            _competition = new Competition();
        }
        
        [Test]
        public void NextTrack_EmptyQueue_ReturnNull() {
            Object result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        [Test]
        public void NextTrack_OneInQueue_ReturnTrack() {
            SectionTypes[] testTrackSecions = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            };
            Track TestTrack = new Track("TrackTrack", testTrackSecions);
            _competition.Tracks.Enqueue(TestTrack);
            Object result = _competition.NextTrack();
            Assert.AreEqual(TestTrack, result);
        }


    }
}
