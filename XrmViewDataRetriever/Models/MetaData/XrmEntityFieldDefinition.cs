using System;
using System.Data;
using Microsoft.Xrm.Sdk.Metadata;

namespace XrmViewDataRetriever.Models.MetaData
{
    public class XrmEntityFieldDefinition
    {
        private readonly AttributeMetadata _metadata;

        public XrmEntityFieldDefinition(AttributeMetadata metadata)
        {
            _metadata = metadata;
        }

        public string Key => _metadata.LogicalName;

        public string KeyClean
        {
            get
            {
                var cleanedColumnName = _metadata.LogicalName.Replace(" ", "_").Trim().Replace(".", "");

                return $"{_metadata.EntityLogicalName}_{cleanedColumnName}";
            }
        }

        public string KeyFormatted => $"{KeyClean}_Formatted";

        public bool UseFormatColumnAlways => ColumnType == typeof (Guid) || this.ColumnType == typeof (Guid?);

        public string FormatString
        {
            get
            {
                if (this.ColumnType == typeof (DateTime) || this.ColumnType == typeof (DateTime?))
                {
                    return "d";
                }

                if (this.CrmTypeCode == AttributeTypeCode.Money)
                {
                    return "c";
                }

                return null;
            }
        }

        public string DisplayName => _metadata.DisplayName.UserLocalizedLabel != null
            ? _metadata.DisplayName.UserLocalizedLabel.Label
            : _metadata.LogicalName;

        public AttributeTypeCode? CrmTypeCode => _metadata.AttributeType;

        public Type ColumnType
        {
            get
            {
                //this is the high volume data types. You may need to add additional ones.
                Type type = null;
                switch (_metadata.AttributeType)
                {
                    case AttributeTypeCode.Picklist:
                    case AttributeTypeCode.Integer:
                    case AttributeTypeCode.BigInt:
                        type = typeof (int);
                        break;
                    case AttributeTypeCode.Boolean:
                        type = typeof (bool);
                        break;
                    case AttributeTypeCode.DateTime:
                        type = typeof (DateTime);
                        break;
                    case AttributeTypeCode.Decimal:
                        type = typeof (decimal);
                        break;
                    case AttributeTypeCode.Double:
                        type = typeof (Double);
                        break;
                    case AttributeTypeCode.Money:
                        type = typeof (double);
                        break;
                    case AttributeTypeCode.Memo:
                    case AttributeTypeCode.String:
                        type = typeof (string);
                        break;
                    case AttributeTypeCode.Lookup:
                    case AttributeTypeCode.Owner:
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Uniqueidentifier:
                        type = typeof (Guid);
                        break;
                    default:
                        type = typeof (string);
                        break;
                }

                return type;
            }
        }

        public Type FormattedColumnType => typeof (string);

        public Tuple<DataColumn, DataColumn> CreateDataTableColumns()
        {
            //add the primary column
            var primaryColumn = new DataColumn(KeyClean, ColumnType) { AllowDBNull = true };

            //add the formatted column
            var formattedColumn = new DataColumn(KeyFormatted) { AllowDBNull = true };

            return new Tuple<DataColumn, DataColumn>(primaryColumn, formattedColumn);
        }
    }
}