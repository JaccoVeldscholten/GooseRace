using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest {
	public class Model_Wings_Info {

		public Track track;

		[SetUp]
		public void Setup() {

		}

		[Test]
		public void Check_Broken_Wings() {
			Wings wing = new Wings();
			wing.IsBroken = true;
			Assert.IsTrue(wing.IsBroken);
		}

		[Test]
		public void Check_Speed_Wings() {
			Wings wing = new Wings();
			wing.Speed = 10;
			Assert.AreEqual(wing.Speed, 10);
		}

		[Test]
		public void Check_Quality_Wings() {
			Wings wing = new Wings();
			wing.Quality = 10;
			Assert.AreEqual(wing.Quality, 10);
		}
		[Test]
		public void Check_Per_Wings() {
			Wings wing = new Wings();
			wing.Performance = 10;
			Assert.AreEqual(wing.Performance, 10);
		}
	}
}
