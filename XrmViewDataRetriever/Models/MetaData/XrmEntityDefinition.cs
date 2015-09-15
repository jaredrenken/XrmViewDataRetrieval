using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Xrm.Sdk.Metadata;

namespace XrmViewDataRetriever.Models.MetaData
{
    public class XrmEntityDefinition
    {
        public XrmEntityDefinition(EntityMetadata metadata)
        {
            var metadata1 = metadata;

            TableName = metadata1.LogicalName;

            DisplayName = metadata1.DisplayName.UserLocalizedLabel != null
                ? metadata1.DisplayName.UserLocalizedLabel.Label
                : metadata1.LogicalName;

            CrmFields =(from x in metadata.Attributes
                select new XrmEntityFieldDefinition(x));
        }

        public IEnumerable<XrmEntityFieldDefinition> CrmFields { get; private set; }

        public string EntityKeyName => $"{this.TableName.ToLower()}id";

        public string TableName { get; private set; }

        public string DisplayName { get; private set; }

        public DataTable CreateDataTable()
        {
            var table = new DataTable(TableName);

            foreach (var crmField in CrmFields)
            {
                //add the value column

                var realColumn = table.Columns.Add(crmField.KeyClean, crmField.ColumnType);
                realColumn.AllowDBNull = true;
                //add the formatted column
                var formattedColumn = table.Columns.Add(crmField.KeyFormatted);
                formattedColumn.AllowDBNull = true;
            }

            return table;
        }
        
    }

}