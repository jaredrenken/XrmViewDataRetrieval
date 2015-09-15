using System;
using System.Data;
using Microsoft.Xrm.Sdk;
using XrmViewDataRetriever.Models.MetaData;

namespace XrmViewDataRetriever.Services
{
    public interface IDynamicsDataAccessHelperService
    {
        Tuple<XrmEntityDefinition, DataTable> ConvertEntityCollectionToDataTable(EntityCollection dataCollection);

        XrmFetchXmlResponse GetDataUsingFetchXml(string fetchXml);

        XrmFetchXmlResponse GetDataUsingSavedQuery(Guid id);
    }
}
