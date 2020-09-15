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

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue() {
            Object result;
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
            result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack() {
            Object resultOne, resultTwo;
            SectionTypes[] testOneTrackSecions = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            };
            SectionTypes[] testTwoTrackSecions = {
                SectionTypes.StartGrid,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.Finish
            };
            Track TestOneTrack = new Track("TrackTrack One", testOneTrackSecions);
            _competition.Tracks.Enqueue(TestOneTrack);
            Track TestTwoTrack = new Track("TrackTrack Two", testTwoTrackSecions);
            _competition.Tracks.Enqueue(TestTwoTrack);
            resultOne = _competition.NextTrack();
            resultTwo = _competition.NextTrack();
            Assert.AreNotEqual(resultOne, resultTwo);
        }

    }
}
