using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest
{
    [TestFixture]

    class Model_Competiton_Sections{

        private Competition _competition;

        [SetUp]
        public void Setup(){
            _competition = new Competition();
        }

        [Test]
        public void Competiton_Check_sections(){
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
            Assert.AreEqual(TestTrack.Sections.Count, 6);
        }


    }
}
