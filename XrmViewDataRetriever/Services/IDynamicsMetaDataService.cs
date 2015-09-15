using System;
using System.Collections.Generic;
using System.Data;
using XrmViewDataRetriever.Models.MetaData;

namespace XrmViewDataRetriever.Services
{
    public interface IDynamicsMetaDataService
    {
        XrmEntityDefinition EntityDefinitionGet(string entityName);
         
        List<XrmSavedQueryDefinition> EntityViewDefinitionsGet(string entityName);

        XrmSavedQueryDefinition EntityViewDefinitionGet(Guid id);

        fetch FetchXmlObjectGraphGet(string fetchXml);

        Tuple<List<XrmEntityDefinition>, DataTable> FetchXmlDataTableDefinitionGet(string fetchXml);
    }
}
