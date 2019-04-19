using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneToManyMapBenchmark;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class OneToManyMapDictionaryTests
    {
        [TestMethod]
        public void OneToManyMapDictionary_Indexer_WhenProvidedWithAValueThatsBeenMappedToAKey_ReturnsTheKey()
        {
            // Arrange
            var expectedKey = "This is Message A";
            var state1 = "VA";
            var state2 = "MD";
            var state3 = "IN";
            OneToManyMapDictionary<string, string> oneToManyMapDictionary = new OneToManyMapDictionary<string, string>();
            oneToManyMapDictionary.AddOneToManyMapping(expectedKey, new[] { state1, state2, state3 });

            // Act
            var key1 = oneToManyMapDictionary[state1];
            var key2 = oneToManyMapDictionary[state2];
            var key3 = oneToManyMapDictionary[state3];

            // Assert
            Assert.AreEqual(expectedKey, key1);
            Assert.AreEqual(expectedKey, key2);
            Assert.AreEqual(expectedKey, key3);
        }

        [TestMethod]
        public void OneToManyMapDictionary_Indexer_WhenProvidedWithAValueNotMappedToAKey_Throws()
        {
            // Arrange
            var valueToLokkup = "NonMappedValue";
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            OneToManyMapDictionary<string, string> oneToManyMapDictionary = new OneToManyMapDictionary<string, string>();
            oneToManyMapDictionary.AddOneToManyMapping(expectedKey, new[] { value1, value2, value3 });

            // Act
            try
            {
                var key = oneToManyMapDictionary[valueToLokkup];
                Assert.Fail("We were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValueNotMappedToKeyException e)
            {
                // Assert
                Assert.IsTrue(e.Message.Contains($"value: {valueToLokkup}"));
                Assert.IsTrue(e.Message.Contains("has not been mapped to a Key"));
            }
        }


        [TestMethod]
        public void OneToManyMapDictionary_AddOneToManyMapping_WhenValueHasPriorMapping_Throws()
        {
            // Arrange
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            OneToManyMapDictionary<string, string> oneToManyMapDictionary = new OneToManyMapDictionary<string, string>();
            oneToManyMapDictionary.AddOneToManyMapping(expectedKey, new[] { value1, value2, value3 });

            // Act
            try
            {
                oneToManyMapDictionary.AddOneToManyMapping("This is a new Key", new[] { "XXXX", "YYYYY", value1 });
                Assert.Fail("We were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValuesHasPriorMappingToKeyException e)
            {
                // Assert                
                Assert.IsTrue(e.Message.Contains($"value: {value1}"));
                Assert.IsTrue(e.Message.Contains("has a prior mapping"));
                Assert.IsTrue(e.Message.Contains(expectedKey));
            }
        }

        [TestMethod]
        public void OneToManyMapDictionary_AddOneToManyMapping_WhenKeyExists_ShouldAddValuesToExistingKey()
        {
            // Arrange
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            var newValueMappedToExistingKey = "NewValue";
            OneToManyMapDictionary<string, string> oneToManyMapDictionary = new OneToManyMapDictionary<string, string>();
            oneToManyMapDictionary.AddOneToManyMapping(expectedKey, new[] { value1, value2, value3 });

            // Act
            oneToManyMapDictionary.AddOneToManyMapping(expectedKey, new[] { newValueMappedToExistingKey });

            // Assert
            Assert.AreEqual(expectedKey, oneToManyMapDictionary[newValueMappedToExistingKey]);
        }
    }
}
