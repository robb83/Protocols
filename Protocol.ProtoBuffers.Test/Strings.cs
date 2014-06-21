using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ProtoType;
using Protocol.ProtoBuffers;

namespace Protocol.Test
{
    [TestClass]
    public class Strings
    {        
        [TestMethod]
        public void TestNullString()
        {
            SampleInfo sample = new SampleInfo(typeof(String), null);

            SampleTest(sample);
        }

        [TestMethod]
        public void TestEmptyString()
        {
            SampleInfo sample = new SampleInfo(typeof(String), "");

            SampleTest(sample);
        }

        [TestMethod]
        public void TestSmallString()
        {
            SampleInfo sample = new SampleInfo(typeof(String), "Hello World!");

            SampleTest(sample);
        }

        [TestMethod]
        public void TestLargeString()
        {
            SampleInfo sample = new SampleInfo(typeof(String), "Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World! Hello World!");

            SampleTest(sample);
        }

        [TestMethod]
        public void TestSpecialCharString()
        {
            SampleInfo sample = new SampleInfo(typeof(String), "\u07DF\u07DF\u07DF\u07DF\u07DF\u07DF\u07DF");

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
