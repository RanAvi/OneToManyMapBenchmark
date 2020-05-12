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
        ////    var oneToManyMap = new OneToManyMapDictionary<TKey, TValue>();
        ////    oneToManyMap.AddOneToManyMapping(key, values);
        ////    return oneToManyMap;
        ////}

        private static IOneToManyMap<string, string> InitializeOneToManyMap(Type oneToManyMapType,  string key, string[] values)
        {
            var oneToManyMap = (IOneToManyMap<string, string>)Activator.CreateInstance(oneToManyMapType);            
            oneToManyMap.AddOneToManyMapping(key, values);
            return oneToManyMap;
        }

        [TestMethod]
        [TestCategory("Class Test")]
        [DataRow(typeof(OneToManyMapDataTable<string, string>))]
        [DataRow(typeof(OneToManyMapDictionary<string, string>))]
        [DataRow(typeof(OneToManyMapList<string, string>))]
        [DataRow(typeof(OneToManyMapSortedList<string, string>))]
        public void Indexer_WhenProvidedWithAValueThatsBeenMappedToAKey_ReturnsTheKey(Type oneToManyMapType)
        {            
            // Arrange
            var expectedKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";

            var oneToManyMap = InitializeOneToManyMap(oneToManyMapType, "Some irrelevant Key", new[] { "ZA", "ZB", "ZC", "ZD" });
            oneToManyMap.AddOneToManyMapping(expectedKey, new[] { value1, value2, value3 });


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
        [DataRow(typeof(OneToManyMapDataTable<string, string>))]
        [DataRow(typeof(OneToManyMapDictionary<string, string>))]
        [DataRow(typeof(OneToManyMapList<string, string>))]
        [DataRow(typeof(OneToManyMapSortedList<string, string>))]
        public void Indexer_WhenProvidedWithAValueNotMappedToAKey_Throws(Type oneToManyMapType)
        {
            // Arrange
            var someIrrelevantKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            var valueWithNoKeyMapping = "NonMappedValue";

            var oneToManyMap = InitializeOneToManyMap(oneToManyMapType, someIrrelevantKey, new[] { value1, value2, value3 });

            // Act
            try
            {
                _ = oneToManyMap[valueWithNoKeyMapping];
                Assert.Fail("We were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValueNotMappedToKeyException e)
            {
                // Assert
                var expectedSubstringInExceptionMessage = "has not been mapped to a Key";
                Assert.IsTrue(e.Message.Contains($"value: {valueWithNoKeyMapping}"), $"An Exception of type ValueNotMappedToKeyException was thrown, however, the message was expected to contain the \"value\" {valueWithNoKeyMapping}, that has not been mapped to a key");
                Assert.IsTrue(e.Message.Contains(expectedSubstringInExceptionMessage), "An Exception of type ValueNotMappedToKeyException was thrown, however, the message was expected to contain the substring: {expectedSubstringInExceptionMessage}");
            }
        }

        [TestMethod]
        [TestCategory("Class Test")]
        [DataRow(typeof(OneToManyMapDataTable<string, string>))]
        [DataRow(typeof(OneToManyMapDictionary<string, string>))]
        [DataRow(typeof(OneToManyMapList<string, string>))]
        [DataRow(typeof(OneToManyMapSortedList<string, string>))]
        public void AddOneToManyMapping_WhenAddingANewValueToExistingKey_ShouldMapValueToKey(Type oneToManyMapType)
        {
            // Arrange
            var expectedKey = "This is Message A";
            var valueAlreadyMapped = "VA";
            var newValue = "MD";

            var oneToManyMap = InitializeOneToManyMap(oneToManyMapType, expectedKey, new[] { valueAlreadyMapped });

            // Act
            oneToManyMap.AddOneToManyMapping(expectedKey, new[] { newValue });
            
            // Assert                
            Assert.AreEqual(expectedKey, oneToManyMap[newValue], $"The newly added value: {newValue} was expected to be mapped to the key: {expectedKey}");
        }

        [TestMethod]
        [TestCategory("Class Test")]
        [DataRow(typeof(OneToManyMapDataTable<string, string>))]
        [DataRow(typeof(OneToManyMapDictionary<string, string>))]
        [DataRow(typeof(OneToManyMapList<string, string>))]
        [DataRow(typeof(OneToManyMapSortedList<string, string>))]
        public void AddOneToManyMapping_WhenValueHasPriorMapping_Throws(Type oneToManyMapType)
        {
            // Arrange
            var preExistingKey = "This is Message A";
            var valueAlreadyMapped = "VA";
            var irrelevantvalue1 = "MD";
            var irrelevantvalue2 = "IN";

            var oneToManyMap = InitializeOneToManyMap(oneToManyMapType, preExistingKey, new[] { valueAlreadyMapped, irrelevantvalue1, irrelevantvalue2 });

            // Act
            try
            {
                oneToManyMap.AddOneToManyMapping("This is a new Key", new[] { "XXXX", "YYYYY", valueAlreadyMapped });
                Assert.Fail("We were expecting an Exception of type ValuesHasPriorMappingToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValuesHasPriorMappingToKeyException e)
            {
                // Assert                
                Assert.IsTrue(e.Message.Contains($"value: {valueAlreadyMapped}"), "The Exception Message does not indicate what \"Value\" is already mapped");
                Assert.IsTrue(e.Message.Contains("has a prior mapping"));
                Assert.IsTrue(e.Message.Contains(preExistingKey), $"The Exception Message does not indicate the \"Key\" the Value: {valueAlreadyMapped} is already mapped to.");
            }
        }

        [TestMethod]
        [TestCategory("Class Test")]
        [DataRow(typeof(OneToManyMapDataTable<string, string>))]
        [DataRow(typeof(OneToManyMapDictionary<string, string>))]
        [DataRow(typeof(OneToManyMapList<string, string>))]
        [DataRow(typeof(OneToManyMapSortedList<string, string>))]
        public void AddOneToManyMapping_WhenOneOrMoreValuesHavePriorMapping_MapRemainsUnchanged(Type oneToManyMapType)
        {
            // Arrange
            var preExistingKey = "This is Message A";
            var valueAlreadyMapped = "VA";
            var irrelevantvalue1 = "MD";
            var irrelevantvalue2 = "IN";
            var oneToManyMap = InitializeOneToManyMap(oneToManyMapType, preExistingKey, new[] { valueAlreadyMapped, irrelevantvalue1, irrelevantvalue2 });
            var irrelevantValueToBeAdded1 = "XXXXXX";
            var irrelevantValueToBeAdded2 = "YYYYYY";

            // Act
            try
            {
                oneToManyMap.AddOneToManyMapping("This is a new Key", new[] { irrelevantValueToBeAdded1, irrelevantValueToBeAdded2, valueAlreadyMapped });
                Assert.Fail("We were expecting an Exception of type ValuesHasPriorMappingToKeyException to be thrown, but no Exception was thrown");
            }
            catch (ValuesHasPriorMappingToKeyException)
            {
                // Assert
                // Those values that were in the map should remain unchanged
                Assert.AreEqual(preExistingKey, oneToManyMap[irrelevantvalue1], $"The value: {irrelevantvalue1} was expected to be mapped to the pre-existing key: {preExistingKey}");
                Assert.AreEqual(preExistingKey, oneToManyMap[irrelevantvalue2], $"The value: {irrelevantvalue2} was expected to be mapped to the pre-existing key: {preExistingKey}");

                // Those values that were attempted to be added where not
                AssertValueIsNotMapped(oneToManyMap, irrelevantValueToBeAdded1);
                AssertValueIsNotMapped(oneToManyMap, irrelevantValueToBeAdded2);
            }
        }

        [TestMethod]
        [TestCategory("Class Test")]
        [DataRow(typeof(OneToManyMapDataTable<string, string>))]
        [DataRow(typeof(OneToManyMapDictionary<string, string>))]
        [DataRow(typeof(OneToManyMapList<string, string>))]
        [DataRow(typeof(OneToManyMapSortedList<string, string>))]
        public void AddOneToManyMapping_WhenKeyExists_ShouldAddValuesToExistingKey(Type oneToManyMapType)
        {
            // Arrange
            var preExistingKey = "This is Message A";
            var value1 = "VA";
            var value2 = "MD";
            var value3 = "IN";
            var newValueMappedToExistingKey = "NewValue";

            var oneToManyMapDictionary = InitializeOneToManyMap(oneToManyMapType, preExistingKey, new[] { value1, value2, value3 });

            // Act
            oneToManyMapDictionary.AddOneToManyMapping(preExistingKey, new[] { newValueMappedToExistingKey });

            // Assert
            Assert.AreEqual(preExistingKey, oneToManyMapDictionary[newValueMappedToExistingKey], $"The new value: {newValueMappedToExistingKey} was expected to be mapped to the pre-existing key: {preExistingKey}");
        }

        private void AssertValueIsNotMapped(IOneToManyMap<string, string> oneToManyMap, string valueNotMapped)
        {
            // Act
            try
            {
                var key = oneToManyMap[valueNotMapped];
                Assert.Fail($"It appears that the value: {valueNotMapped} is mapped to the key: {key}. However, we were expecting an Exception of type ValueNotMappedToKeyException to be thrown, but no Exception was thrown.");
            }
            catch (ValueNotMappedToKeyException)
            {
                // Intentionally Ignoring this exception since we're expecting this exception
            }
        }
    }
}
