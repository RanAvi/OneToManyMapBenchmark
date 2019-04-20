using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OneToManyMapBenchmark
{
    internal sealed class OneToManyMapDataTable<TKey, TValue> : IOneToManyMap<TKey, TValue>
    {
        private static DataTable keyDataTable;
        private static DataTable valueDataTable;
        private static DataRelation keyValueRelation;

        public TKey this[TValue value]
        {
            get { return GetKey(value); }
        }

        public OneToManyMapDataTable()
        {
            InitializeSchema();
        }

        private static void InitializeSchema()
        {
            keyDataTable = new DataTable("Key");
            keyDataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            keyDataTable.Columns.Add(new DataColumn("Description", typeof(TKey)));
            keyDataTable.PrimaryKey = new[] { keyDataTable.Columns[0] };
            keyDataTable.Constraints.Add("IdxKey", keyDataTable.Columns[1], false);


            valueDataTable = new DataTable("Value");
            valueDataTable.Columns.Add(new DataColumn("Value", typeof(TValue)));
            valueDataTable.Columns.Add(new DataColumn("KeyId", typeof(int)));
            valueDataTable.PrimaryKey = new[] { valueDataTable.Columns[0] };

            var keyIdFK = new ForeignKeyConstraint("KeyIdFK", keyDataTable.Columns["Id"], valueDataTable.Columns["KeyId"]);
            valueDataTable.Constraints.Add(keyIdFK);

            var dataSet = new DataSet();
            dataSet.Tables.Add(keyDataTable);
            dataSet.Tables.Add(valueDataTable);
            keyValueRelation = new DataRelation("keyValueDataRelation", keyDataTable.Columns[0], valueDataTable.Columns[1]);
            dataSet.Relations.Add(keyValueRelation);
        }

        private static TKey GetKey(TValue value)
        {
            var keyValueRow = valueDataTable.Rows.Find(value);

            if (keyValueRow != null)
            {
                var keyRow = keyValueRow.GetParentRow(keyValueRelation);
                return (TKey)keyRow[1];
            }

            throw new ValueNotMappedToKeyException($"The value: {value} of type: {typeof(TValue)}, has not been mapped to a Key of type: {typeof(TKey)}");
        }

        public void AddOneToManyMapping(TKey key, TValue[] values)
        {
            EnsureValuesHaveNoPriorMapping(values);

            var keyRowsFound = keyDataTable.Select($"Description = '{key}'");
            var keyId = 0;

            if (keyRowsFound.Length == 0)
            {
                keyId = keyDataTable.Rows.Count + 1;
                var row = keyDataTable.NewRow();
                row[0] = keyId;
                row[1] = key;
                keyDataTable.Rows.Add(row);
            }
            else
            {
                keyId = (int)keyRowsFound[0][0];
            }

            foreach (var value in values)
            {
                var row = valueDataTable.NewRow();
                row[0] = value;
                row[1] = keyId;
                valueDataTable.Rows.Add(row);
            }
        }

        private void EnsureValuesHaveNoPriorMapping(TValue[] values)
        {
            foreach (var value in values)
            {
                var keyValueRow = valueDataTable.Rows.Find(value);
                if (keyValueRow != null)
                {
                    var keyRow = keyValueRow.GetParentRow(keyValueRelation);
                    var mappedKey = (TKey)keyRow[1];
                    throw new ValuesHasPriorMappingToKeyException($"The value: {value}, has a prior mapping to the key: {mappedKey}.");
                }
            }
        }
    }
}
