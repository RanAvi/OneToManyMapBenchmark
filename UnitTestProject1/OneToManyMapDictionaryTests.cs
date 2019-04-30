using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneToManyMapBenchmark;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class OneToManyMapDictionaryTests
    {
        private static IOneToManyMap<TKey, TValue> InitializeOneToManyMap<TKey, TValue>(TKey key, TValue[] values)
        {
            var oneToManyMap = new OneToManyMapList<TKey, TValue>();
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

            var oneToManyMap = InitializeOneToManyMap(expectedKey, new[] { value1, value2, value3 });

            // Act
            var key1 = oneToManyMap[value1];
            var key2 = oneToManyMap[value2];
            var key3 = oneToManyMap[value3];

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
            var someIrrelevantKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            var valueWithNoKeyMapping = "NonMappedValue";

            var oneToManyMap = InitializeOneToManyMap(someIrrelevantKey, new[] { value1, value2, value3 });

            // Act
            try
            {
                var _ = oneToManyMap[valueWithNoKeyMapping];
                Assert.Fail("We were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValueNotMappedToKeyException e)
            {
                // Assert
                Assert.IsTrue(e.Message.Contains($"value: {valueWithNoKeyMapping}"));
                Assert.IsTrue(e.Message.Contains("has not been mapped to a Key"));
            }
        }


        [TestMethod]
        [TestCategory("Class Test")]
        public void OneToManyMap_AddOneToManyMapping_WhenValueHasPriorMapping_Throws()
        {
            // Arrange
            var preExistingKey = "This is Message A";
            var valueAlreadyMapped = "VA";
            var irrelevantvalue1 = "MD";
            var irrelevantvalue2 = "IN";

            var oneToManyMap = InitializeOneToManyMap(preExistingKey, new[] { valueAlreadyMapped, irrelevantvalue1, irrelevantvalue2 });

            // Act
            try
            {
                oneToManyMap.AddOneToManyMapping("This is a new Key", new[] { "XXXX", "YYYYY", valueAlreadyMapped });
                Assert.Fail("We were expecting an Exception of type ValuesHasPriorMappingToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValuesHasPriorMappingToKeyException e)
            {
                // Assert                
                Assert.IsTrue(e.Message.Contains($"value: {valueAlreadyMapped}"));
                Assert.IsTrue(e.Message.Contains("has a prior mapping"));
                Assert.IsTrue(e.Message.Contains(preExistingKey));
            }
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void OneToManyMap_AddOneToManyMapping_WhenOneOrMoreValuesHavePriorMapping_MapRemainsUnchanged()
        {
            // Arrange
            var preExistingKey = "This is Message A";
            var valueAlreadyMapped = "VA";
            var irrelevantvalue1 = "MD";
            var irrelevantvalue2 = "IN";
            var oneToManyMap = InitializeOneToManyMap(preExistingKey, new[] { valueAlreadyMapped, irrelevantvalue1, irrelevantvalue2 });
            var irrelevantValueToBeAdded1 = "XXXXXX";
            var irrelevantValueToBeAdded2 = "YYYYYY";

            // Act
            try
            {
                oneToManyMap.AddOneToManyMapping("This is a new Key", new[] { irrelevantValueToBeAdded1, irrelevantValueToBeAdded2, valueAlreadyMapped });
                Assert.Fail("We were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValuesHasPriorMappingToKeyException)
            {
                // Assert
                // Those values that were in the map should remain unchanged
                Assert.AreEqual(preExistingKey, oneToManyMap[irrelevantvalue1]);
                Assert.AreEqual(preExistingKey, oneToManyMap[irrelevantvalue2]);

                // Those values that were attempted to be added where not
                AssertValueIsNotMapped(oneToManyMap, irrelevantValueToBeAdded1);
                AssertValueIsNotMapped(oneToManyMap, irrelevantValueToBeAdded2);
            }
        }

        private void AssertValueIsNotMapped(IOneToManyMap<string, string> oneToManyMap, string valueNotMapped)
        {
            // Act
            try
            {
                var _ = oneToManyMap[valueNotMapped];
                Assert.Fail("We were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValueNotMappedToKeyException)
            {
                // Intentionally Ignoring this exception since we're expecting this exception
            }
        }

        [TestMethod]
        [TestCategory("Class Test")]
        public void OneToManyMap_AddOneToManyMapping_WhenKeyExists_ShouldAddValuesToExistingKey()
        {
            // Arrange
            var preExistingKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            var newValueMappedToExistingKey = "NewValue";

            var oneToManyMapDictionary = InitializeOneToManyMap(preExistingKey, new[] { value1, value2, value3 });

            // Act
            oneToManyMapDictionary.AddOneToManyMapping(preExistingKey, new[] { newValueMappedToExistingKey });

            // Assert
            Assert.AreEqual(preExistingKey, oneToManyMapDictionary[newValueMappedToExistingKey]);
        }
    }
}
