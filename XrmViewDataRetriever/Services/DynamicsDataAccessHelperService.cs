using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using XrmViewDataRetriever.Models.MetaData;

namespace XrmViewDataRetriever.Services
{
    public class DynamicsDataAccessHelperService : IDynamicsDataAccessHelperService
    {
        private readonly IOrganizationService _organizationService;
        private readonly IDynamicsMetaDataService _metaDataService;

        public DynamicsDataAccessHelperService(IOrganizationService organizationService, IDynamicsMetaDataService metaDataService)
        {
            _organizationService = organizationService;
            _metaDataService = metaDataService;
        }

        public Tuple<XrmEntityDefinition, DataTable> ConvertEntityCollectionToDataTable(EntityCollection dataCollection)
        {
            var entityDefinition = _metaDataService.EntityDefinitionGet(dataCollection.EntityName);

            var data = entityDefinition.CreateDataTable();

            foreach (var entityData in dataCollection.Entities)
            {
                var row = data.NewRow();

                foreach (var crmField in entityDefinition.CrmFields)
                {
                    if (!entityData.Attributes.ContainsKey(crmField.Key)) continue;

                    var entityReference = entityData.Attributes[crmField.Key] as EntityReference;

                    //add the normal value
                    if (entityReference != null)
                    {
                        row[crmField.KeyClean] = entityReference.Id;

                        row[crmField.KeyFormatted] = entityReference.Name;
                        continue;
                    }

                    var optionSetValue = entityData.Attributes[crmField.Key] as OptionSetValue;

                    if (optionSetValue != null)
                    {
                        row[crmField.KeyClean] = optionSetValue.Value;

                        if (entityData.FormattedValues.ContainsKey(crmField.Key))
                        {
                            row[crmField.KeyFormatted] = entityData.FormattedValues[crmField.Key];
                        }
                        else
                        {
                            row[crmField.KeyFormatted] = optionSetValue.Value;//TODO: get the friendly name
                        }

                        continue;
                    }

                    var moneyValue = entityData.Attributes[crmField.Key] as Money;

                    if (moneyValue != null)
                    {
                        row[crmField.KeyClean] = moneyValue.Value;

                        if (entityData.FormattedValues.ContainsKey(crmField.Key))
                        {
                            row[crmField.KeyFormatted] = entityData.FormattedValues[crmField.Key];
                        }
                        else
                        {
                            row[crmField.KeyFormatted] = moneyValue.Value;
                        }

                        continue;
                    }

                    row[crmField.KeyClean] = entityData[crmField.Key];

                    if (entityData.FormattedValues.ContainsKey(crmField.Key))
                    {
                        row[crmField.KeyFormatted] = entityData.FormattedValues[crmField.Key];
                    }
                    else
                    {
                        row[crmField.KeyFormatted] = entityData[crmField.Key];
                    }
                }

                data.Rows.Add(row);
            }

            return new Tuple<XrmEntityDefinition, DataTable>(entityDefinition, data);
        }

        public XrmFetchXmlResponse GetDataUsingFetchXml(string fetchXml)
        {
            var result = _organizationService.RetrieveMultiple(new FetchExpression(fetchXml));

            //get the data table definition
            var table = _metaDataService.FetchXmlDataTableDefinitionGet(fetchXml);

            //loop through each result
            foreach (var entityData in result.Entities)
            {
                AddEntityToDataTable(table.Item2, entityData, table.Item1);
            }

            return new XrmFetchXmlResponse {Data = table.Item2, EntityDefinitions = table.Item1, RawEntityData = result};
        }

        public XrmFetchXmlResponse GetDataUsingSavedQuery(Guid id)
        {
            var savedQuery = _metaDataService.EntityViewDefinitionGet(id);

            var data = this.GetDataUsingFetchXml(savedQuery.FetchXml);

            data.SavedQueryId = id;

            return data;
        }

        private void AddEntityToDataTable(DataTable table, Entity entity, List<XrmEntityDefinition> entityDefinitions)
        {
            var row = table.NewRow();

            var primaryEntityDefinition = entityDefinitions.Single(x => x.TableName == entity.LogicalName);
            
            foreach (var attribute in entity.Attributes)
            {
                var entityDefinition = primaryEntityDefinition;
                XrmEntityFieldDefinition crmField;
                EntityReference entityReference;
                //get the field definition
                var aliasedValue = attribute.Value as AliasedValue;

                if (aliasedValue != null)
                {
                    //this belongs to a different entity
                    entityDefinition = entityDefinitions.FirstOrDefault(x => x.TableName == aliasedValue.EntityLogicalName);
                    entityReference = aliasedValue.Value as EntityReference;
                    crmField = entityDefinition.CrmFields.Single(x => x.Key == aliasedValue.AttributeLogicalName);
                }
                else
                {
                    entityReference = attribute.Value as EntityReference;
                    crmField = entityDefinition.CrmFields.Single(x => x.Key == attribute.Key);
                }

                //if the attribute is not found, then skip it
                if (!table.Columns.Contains(crmField.KeyClean)) continue;

                //add the normal value
                if (entityReference != null)
                {
                    row[crmField.KeyClean] = entityReference.Id;

                    row[crmField.KeyFormatted] = entityReference.Name;
                    continue;
                }

                var optionSetValue = attribute.Value as OptionSetValue;

                if (optionSetValue != null)
                {
                    row[crmField.KeyClean] = optionSetValue.Value;

                    if (entity.FormattedValues.ContainsKey(crmField.Key))
                    {
                        row[crmField.KeyFormatted] = entity.FormattedValues[crmField.Key];
                    }
                    else
                    {
                        row[crmField.KeyFormatted] = optionSetValue.Value;//TODO: get the friendly name
                    }

                    continue;
                }

                var moneyValue = attribute.Value as Money;

                if (moneyValue != null)
                {
                    row[crmField.KeyClean] = moneyValue.Value;

                    if (entity.FormattedValues.ContainsKey(crmField.Key))
                    {
                        row[crmField.KeyFormatted] = entity.FormattedValues[crmField.Key];
                    }
                    else
                    {
                        row[crmField.KeyFormatted] = moneyValue.Value;
                    }

                    continue;
                }

                if (aliasedValue != null)
                {
                    if (entity.FormattedValues.ContainsKey(crmField.Key))
                    {
                        row[crmField.KeyFormatted] = aliasedValue.Value;
                    }
                    else
                    {
                        row[crmField.KeyFormatted] = aliasedValue.Value;
                    }

                    continue;
                }

                row[crmField.KeyClean] = attribute.Value;

                if (entity.FormattedValues.ContainsKey(crmField.Key))
                {
                    row[crmField.KeyFormatted] = entity.FormattedValues[crmField.Key];
                }
                else
                {
                    row[crmField.KeyFormatted] = entity[crmField.Key];
                }
                
            }

            table.Rows.Add(row);
        }
    }
}