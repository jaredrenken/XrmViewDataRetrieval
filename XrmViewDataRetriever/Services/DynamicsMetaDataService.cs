using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using XrmViewDataRetriever.Models.MetaData;

namespace XrmViewDataRetriever.Services
{
    public class DynamicsMetaDataService : IDynamicsMetaDataService
    {
        private readonly IOrganizationService _organizationService;

        public DynamicsMetaDataService(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        public List<XrmSavedQueryDefinition> EntityViewDefinitionsGet(string entityName)
        {
            var xrmContext = new XrmServiceContext(_organizationService);

            var savedQueries = xrmContext.SavedQuerySet.Where(x => x.ReturnedTypeCode == entityName).ToList();

            var list = savedQueries.Select(MapSavedQueryToDefinition).ToList();

            return list;
        }

        public XrmEntityDefinition EntityDefinitionGet(string entityName)
        {
            var request = new RetrieveEntityRequest { LogicalName = entityName, EntityFilters = EntityFilters.Attributes };

            var response = (RetrieveEntityResponse)_organizationService.Execute(request);

            var metaData = response.EntityMetadata;

            var entityDefinition = new XrmEntityDefinition(metaData);

            return entityDefinition;
        }

        public fetch FetchXmlObjectGraphGet(string fetchXml)
        {
            var serializer = new XmlSerializer(typeof(fetch));
            var rdr = new StringReader(fetchXml);
            var resultingMessage = (fetch)serializer.Deserialize(rdr);

            return resultingMessage;
        }

        public Tuple<List<XrmEntityDefinition>, DataTable> FetchXmlDataTableDefinitionGet(string fetchXml)
        {
            //the first thing we need to do is get a list of all entities

            var entityDefinitions = new List<XrmEntityDefinition>();

            var fetch = FetchXmlObjectGraphGet(fetchXml);

            //get the targeted entity
            var primaryEntity = EntityDefinitionGet(fetch.entity.name);
            entityDefinitions.Add(primaryEntity);

            //get any linked entities
            //only get entities where there is an alias
            var table = new DataTable(primaryEntity.TableName);

            foreach (var attribute in fetch.entity.attribute)
            {
                //get the field
                var field = primaryEntity.CrmFields.Single(x => x.Key == attribute.name);

                var columns = field.CreateDataTableColumns();

                table.Columns.Add(columns.Item1);

                table.Columns.Add(columns.Item2);
            }

            if (fetch.entity.linkentity != null)
            {
                entityDefinitions.AddRange(fetch.entity.linkentity.Where(x => !string.IsNullOrEmpty(x.alias)).Select(entity => EntityDefinitionGet(entity.name)));

                //now go through each of the linked entities:
                foreach (var entity in fetch.entity.linkentity.Where(x => x.attribute != null))
                {
                    var entityDefinition = entityDefinitions.FirstOrDefault(x => x.TableName == entity.name);//there may be more than one entry here, but that's ok

                    foreach (var attribute in entity.attribute)
                    {
                        //get the field
                        var field = entityDefinition.CrmFields.Single(x => x.Key == attribute.name);

                        var columns = field.CreateDataTableColumns();

                        table.Columns.Add(columns.Item1);

                        table.Columns.Add(columns.Item2);
                    }
                }
            }

            return new Tuple<List<XrmEntityDefinition>, DataTable>(entityDefinitions, table);
        }

        public XrmSavedQueryDefinition EntityViewDefinitionGet(Guid id)
        {
            var xrmService = new XrmServiceContext(_organizationService);

            var query = xrmService.SavedQuerySet.Single(x => x.SavedQueryId == id);

            var savedQueryDefinition = MapSavedQueryToDefinition(query);

            return savedQueryDefinition;
        }

        private XrmSavedQueryDefinition MapSavedQueryToDefinition(SavedQuery query)
        {
            var returnedTypeCode = query.ReturnedTypeCode;

            var entity = EntityDefinitionGet(returnedTypeCode);

            var definition = new XrmSavedQueryDefinition
            {
                Name = query.Name,
                FetchXml = query.FetchXml,
                SavedQueryId = query.SavedQueryId.Value,
                LayoutXml = query.LayoutXml,
                Entity = entity.TableName,
                EntityDisplayName = entity.DisplayName
            };

            return definition;
        }

    }

}