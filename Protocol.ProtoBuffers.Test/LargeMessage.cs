using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ProtoType;
using ProtoType.Examples;
using Protocol.ProtoBuffers;
using Protocol.ProtoBuffers.Test;

namespace Protocol.Test
{
    [TestClass]
    public class LargeMessage
    {
        static String[] externalResources = new String[]{
            @"Resources/bigText01.txt", @"Resources/bigText02.txt", @"Resources/bigText03.txt"
        };
        
        [TestMethod]
        public void TestLargeMessage1()
        {
            String strings = File.ReadAllText(externalResources[0]);

            SampleInfo sample = new SampleInfo(typeof(String), strings);
            SampleTest(sample);
        }

        [TestMethod]
        public void TestLargeMessage2()
        {
            String[] strings = new String[10];

            for(int i=0;i<strings.Length;i++)
            {
                strings[i] = File.ReadAllText(externalResources[i % externalResources.Length]);
            }

            SampleInfo sample = new SampleInfo(typeof(String[]), strings);
            SampleTest(sample);
        }

        [TestMethod]
        public void TestLargeMessage3()
        {
            var nested1 = new ComplexSimpleNestedType
            {
                ComplexSimple = new ComplexSimpleType
                {
                    DoubleValue = -1,
                    NestedTypes = null,
                },
                StringValue = File.ReadAllText(externalResources[0]),
            };

            var nested2 = new ComplexSimpleNestedType
            {
                ComplexSimple = new ComplexSimpleType
                {
                    DoubleValue = -1,
                    NestedTypes = new ComplexSimpleNestedType[] { nested1 },                   
                },
                StringValue = File.ReadAllText(externalResources[1]),
            };

            var nested3 = new ComplexSimpleNestedType
            {
                ComplexSimple = new ComplexSimpleType
                {
                    DoubleValue = -1,
                    NestedTypes = new ComplexSimpleNestedType[] { nested2 },
                },
                StringValue = File.ReadAllText(externalResources[2]),
            };

            ComplexSimpleType complex = new ComplexSimpleType
            {
                DoubleValue = -1.987654321,
                NestedTypes = new ComplexSimpleNestedType[] { nested1, nested2, nested3 }                
            };

            SampleInfo sample = new SampleInfo(typeof(ComplexSimpleType), complex);
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
