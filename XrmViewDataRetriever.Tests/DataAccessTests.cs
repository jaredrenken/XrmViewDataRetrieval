using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XrmViewDataRetriever.Services;

namespace XrmViewDataRetriever.Tests
{
    [TestClass]
    public class DataAccessTests
    {
        [TestMethod]
        public void GetViewDataBySavedQueryId_Test()
        {
            var metaDataService = new DynamicsMetaDataService(OrganizationHelper.AdminService);

            var dataAccessService = new DynamicsDataAccessHelperService(OrganizationHelper.UserService, metaDataService);

            var view = metaDataService.EntityViewDefinitionsGet("account").FirstOrDefault();

            var viewDefinition = metaDataService.EntityViewDefinitionGet(view.SavedQueryId);

            var data = dataAccessService.GetDataUsingSavedQuery(viewDefinition.SavedQueryId);
        }

        [TestMethod]
        public void GetViewDataByFetchXml_Test()
        {
            var metaDataService = new DynamicsMetaDataService(OrganizationHelper.AdminService);

            var dataAccessService = new DynamicsDataAccessHelperService(OrganizationHelper.UserService, metaDataService);

            var view = metaDataService.EntityViewDefinitionsGet("account").FirstOrDefault();

            var viewDefinition = metaDataService.EntityViewDefinitionGet(view.SavedQueryId);

            var data = dataAccessService.GetDataUsingFetchXml(viewDefinition.FetchXml);
        }
    }
}