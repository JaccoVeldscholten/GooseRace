using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest {
    [TestFixture]
    class Model_Competition_NextTrackShould {
        private Competition _competition;

        [SetUp]
        public void SetUp() {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull() {
            Object result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack() {
            SectionTypes[] testSections = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
            };

            Track testTrack = new Track("TestTrack", testSections);
            _competition.Tracks.Enqueue(testTrack);
            Object result = _competition.NextTrack();
            Assert.AreEqual(testTrack, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue() {
            SectionTypes[] testSections = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
            };

            Track testTrack = new Track("TestTrack", testSections);
            _competition.Tracks.Enqueue(testTrack);
            Object result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack() {
            SectionTypes[] testSectionsOne = {
                SectionTypes.StartGrid,
                SectionTypes.LeftCorner,
            };

            SectionTypes[] testSectionsTwo = {
                SectionTypes.StartGrid,
                SectionTypes.RightCorner,
                SectionTypes.LeftCorner
            };
            
            Track testTrackOne = new Track("TestTrackOne", testSectionsOne);
            Track testTrackTwo = new Track("TestTrackTwo", testSectionsTwo);
            _competition.Tracks.Enqueue(testTrackOne);
            _competition.Tracks.Enqueue(testTrackTwo);

            Object result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.AreEqual(testTrackTwo, result);
        }
    }
}
