using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneToManyMapBenchmark;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class OneToManyMapDictionaryTests
    {
        ////private static IOneToManyMap<TKey, TValue> InitializeOneToManyMap<TKey, TValue>(TKey key, TValue[] values)
        ////{
        ////    var oneToManyMap = new OneToManyMapDataTable<TKey, TValue>();
        ////    oneToManyMap.AddOneToManyMapping(key, values);
        ////    return oneToManyMap;
        ////}

        private static IOneToManyMap<string, string> InitializeOneToManyMap(string key, string[] values)
        {
            var oneToManyMap = new OneToManyMapSortedList<string, string>();
            oneToManyMap.AddOneToManyMapping(key, values);
            return oneToManyMap;
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void OneToManyMap_Indexer_WhenProvidedWithAValueThatsBeenMappedToAKey_ReturnsTheKey()
        {
            // Arrange
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";

            var oneToManyMapDictionary = InitializeOneToManyMap(expectedKey, new[] { value1, value2, value3 });

            // Act
            var key1 = oneToManyMapDictionary[value1];
            var key2 = oneToManyMapDictionary[value2];
            var key3 = oneToManyMapDictionary[value3];

            // Assert
            Assert.AreEqual(expectedKey, key1);
            Assert.AreEqual(expectedKey, key2);
            Assert.AreEqual(expectedKey, key3);
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void OneToManyMap_Indexer_WhenProvidedWithAValueNotMappedToAKey_Throws()
        {
            // Arrange
            var valueToLokkup = "NonMappedValue";
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";

            var oneToManyMapDictionary = InitializeOneToManyMap(expectedKey, new[] { value1, value2, value3 });

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
        [TestCategory("Class Test")]
        public void OneToManyMap_AddOneToManyMapping_WhenValueHasPriorMapping_Throws()
        {
            // Arrange
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";

            var oneToManyMapDictionary = InitializeOneToManyMap(expectedKey, new[] { value1, value2, value3 });

            // Act
            try
            {
                oneToManyMapDictionary.AddOneToManyMapping("This is a new Key", new[] { "XXXX", "YYYYY", value1 });
                Assert.Fail("We were expecting an Exception of type ValuesHasPriorMappingToKeyException to be thrown, but no Exception was thrown");
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
        [TestCategory("Class Test")]
        public void OneToManyMap_AddOneToManyMapping_WhenKeyExists_ShouldAddValuesToExistingKey()
        {
            // Arrange
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            var newValueMappedToExistingKey = "NewValue";

            var oneToManyMapDictionary = InitializeOneToManyMap(expectedKey, new[] { value1, value2, value3 });

            // Act
            oneToManyMapDictionary.AddOneToManyMapping(expectedKey, new[] { newValueMappedToExistingKey });

            // Assert
            Assert.AreEqual(expectedKey, oneToManyMapDictionary[newValueMappedToExistingKey]);
        }
    }
}
