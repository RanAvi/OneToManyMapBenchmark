using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OneToManyMapBenchmark
{
    internal sealed class OneToManyMapDataTable : IOneToManyMap<string, string>
    {
        private static DataTable messageDataTable;
        private static DataTable stateMessageDataTable;
        private static DataRelation stateMessageRelation;

        public string this[string value]
        {
            get { return GetStateMessage(value); }
        }

        public OneToManyMapDataTable()
        {
            InitializeSchema();
        }

        private static void InitializeSchema()
        {
            messageDataTable = new DataTable("Message");
            messageDataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            messageDataTable.Columns.Add(new DataColumn("Description", typeof(string)));
            messageDataTable.PrimaryKey = new[] { messageDataTable.Columns[0] };
            messageDataTable.Constraints.Add("IdxDescription", messageDataTable.Columns[1], false);


            stateMessageDataTable = new DataTable("StateMessage");
            stateMessageDataTable.Columns.Add(new DataColumn("State", typeof(string)));
            stateMessageDataTable.Columns.Add(new DataColumn("MessageId", typeof(int)));
            stateMessageDataTable.PrimaryKey = new[] { stateMessageDataTable.Columns[0] };

            var messageIdFK = new ForeignKeyConstraint("MessageIdFK", messageDataTable.Columns["Id"], stateMessageDataTable.Columns["MessageId"]);
            stateMessageDataTable.Constraints.Add(messageIdFK);

            var dataSet = new DataSet();
            dataSet.Tables.Add(messageDataTable);
            dataSet.Tables.Add(stateMessageDataTable);
            stateMessageRelation = new DataRelation("StateMessage_Message_Rel", messageDataTable.Columns[0], stateMessageDataTable.Columns[1]);
            dataSet.Relations.Add(stateMessageRelation);
        }

        private static string GetStateMessage(string value)
        {
            var stateMessageRow = stateMessageDataTable.Rows.Find(value);
            var messageRow = stateMessageRow.GetParentRow(stateMessageRelation);
            return (string)messageRow[1];
        }

        public void AddOneToManyMapping(string key, string[] values)
        {
            var messageRows = messageDataTable.Select($"Description = '{key}'");
            var messageId = 0;

            if (messageRows.Length == 0)
            {
                messageId = messageDataTable.Rows.Count + 1;
                var row = messageDataTable.NewRow();
                row[0] = messageId;
                row[1] = key;
                messageDataTable.Rows.Add(row);
            }
            else
            {
                messageId = (int)messageRows[0][0];
            }

            foreach (var state in values)
            {
                var row = stateMessageDataTable.NewRow();
                row[0] = state;
                row[1] = messageId;
                stateMessageDataTable.Rows.Add(row);
            }
        }
    }
}
