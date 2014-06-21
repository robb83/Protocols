using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ProtoType;
using Protocol.ProtoBuffers;

namespace Protocol.Test
{
    [TestClass]
    public class NullablePrimitives
    {
        [TestMethod]
        public void TestDateTimeNullLocal()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime?), DateTime.Now);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestDateTimeNullUtc()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime?), DateTime.UtcNow);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestDateTimeNullUnspecified()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime?), new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified));

            SampleTest(sample);
        }

        [TestMethod]
        public void TestDateTimeNull()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime?), null);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestTimeSpanNullTenHoursAndHalf()
        {
            SampleInfo sample = new SampleInfo(typeof(TimeSpan?), TimeSpan.FromHours(10.5));

            SampleTest(sample);
        }

        [TestMethod]
        public void TestTimeSpanNull()
        {
            SampleInfo sample = new SampleInfo(typeof(TimeSpan?), null);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32NullZero()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32?), 0);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32NullOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32?), 1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32NullMaximum()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32?), Int32.MaxValue);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32NullMinusOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32?), -1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32Null()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32?), null);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64NullZero()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64?), (long)0);

            SampleTest(sample);
        }

        [TestMethod]
        public void TTestInt64NullOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64?), (long)1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64NullMaximum()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64?), Int64.MaxValue);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64NullMinusOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64?), (long)-1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64Null()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64?), null);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16NullZero()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16?), (short)0);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16NullOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16?), (short)1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16NullMaximum()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16?), Int16.MaxValue);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16NullMinusOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16?), (short)-1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16Null()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16?), null);

            SampleTest(sample);
        }

        private void SampleTest(SampleInfo sample)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Serializers.Serialize(stream, sample.Source);

                stream.Position = 0;

                var result = Serializers.Deserialize(stream, sample.ExpectedType);

                Assert.IsTrue(CompareHelper.IsEqualObject(sample.Source, result));
            }
        }
    }
}
