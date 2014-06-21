using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ProtoType;
using ProtoType.Examples;
using Protocol.ProtoBuffers;

namespace Protocol.Test
{
    [TestClass]
    public class Complex
    {
        [TestMethod]
        public void TestSimpleGenericNull()
        {
            SampleInfo sample = new SampleInfo(typeof(SimpleGenericClass<String>), null);
            SampleTest(sample);
        }

        [TestMethod]
        public void TestSimpleGeneric()
        {
            SampleInfo sample = new SampleInfo(typeof(SimpleGenericClass<String>), new SimpleGenericClass<String>
            {
                GenericValue = "Hello World!",
                IntValue = 1234
            });

            SampleTest(sample);
        }

        [TestMethod]
        public void TestSimpleDerived()
        {
            SampleInfo sample = new SampleInfo(typeof(SimpleDerivedClass), new SimpleDerivedClass
            {
                DateTime1 = DateTime.Now,
                IntValue = 1,
                IntValue1  = 4321,
                StringValue = Environment.Version.ToString(),
                StringValue1 = Environment.CurrentDirectory
            });

            SampleTest(sample);
        }

        [TestMethod]
        public void TestRecursiveDeclaration()
        {
            SampleInfo sample = new SampleInfo(typeof(RecursiveClass), new RecursiveClass
            {
                IntValue = 1234,
                RecusiveTag = new RecursiveClass
                {
                    IntValue = 4344,
                    RecusiveTag = null
                }
            });

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
