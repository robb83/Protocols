using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ProtoType;
using Protocol.ProtoBuffers;

namespace Protocol.Test
{
    [TestClass]
    public class Primitives
    {
        [TestMethod]
        public void TestDateTimeLocal()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime), DateTime.Now);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestDateTimeUtc()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime), DateTime.UtcNow);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestDateTimeUnspecified()
        {
            SampleInfo sample = new SampleInfo(typeof(DateTime), new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified));

            SampleTest(sample);
        }

        [TestMethod]
        public void TestTimeSpan()
        {
            SampleInfo sample = new SampleInfo(typeof(TimeSpan), TimeSpan.FromHours(10.5));

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32Zero()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32), 0);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32One()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32), 1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32Maximum()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32), Int32.MaxValue);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt32MinusOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int32), -1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64Zero()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64), (long)0);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64One()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64), (long)1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64Maximum()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64), Int64.MaxValue);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt64MinusOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int64), (long)-1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16Zero()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16), (short)0);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16One()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16), (short)1);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16Maximum()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16), Int16.MaxValue);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestInt16MinusOne()
        {
            SampleInfo sample = new SampleInfo(typeof(Int16), (short)-1);

            SampleTest(sample);
        }
        
        [TestMethod]
        public void TestCustomEnum()
        {
            SampleInfo sample = new SampleInfo(typeof(CustomEnum), CustomEnum.Three);

            SampleTest(sample);
        }


        [TestMethod]
        public void TestCustomEnumArray()
        {
            SampleInfo sample = new SampleInfo(typeof(CustomEnum[]), new CustomEnum[]{ CustomEnum.Three, CustomEnum.Two, CustomEnum.One});

            SampleTest(sample);
        }

        [TestMethod]
        public void TestCustomByteEnum()
        {
            SampleInfo sample = new SampleInfo(typeof(CustomByteEnum), CustomByteEnum.Three);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestCustomLongEnum()
        {
            SampleInfo sample = new SampleInfo(typeof(CustomLongEnum), CustomLongEnum.Three);

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
