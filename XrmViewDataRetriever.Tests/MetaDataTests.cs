using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XrmViewDataRetriever.Services;

namespace XrmViewDataRetriever.Tests
{
    [TestClass]
    public class MetaDataTests
    {
        [TestMethod]
        public void GetEntityDefinition_Test()
        {
            var org = OrganizationHelper.AdminService;

            var service = new DynamicsMetaDataService(org);

            var entity = service.EntityDefinitionGet("account");
        }

        [TestMethod]
        public void GetViewDefinitions_Test()
        {
            var org = OrganizationHelper.AdminService;

            var service = new DynamicsMetaDataService(org);

            var views = service.EntityViewDefinitionsGet("account");
        }

        [TestMethod]
        public void GetViewDefinition_Test()
        {
            var org = OrganizationHelper.AdminService;

            var service = new DynamicsMetaDataService(org);

            var view = service.EntityViewDefinitionsGet("account").FirstOrDefault();

            var viewDefinition = service.EntityViewDefinitionGet(view.SavedQueryId);
        }

        [TestMethod]
        public void GetViewDefinitionObjectGraph_Test()
        {
            var org = OrganizationHelper.AdminService;

            var service = new DynamicsMetaDataService(org);

            var view = service.EntityViewDefinitionsGet("account").FirstOrDefault();

            var graph = service.FetchXmlObjectGraphGet(view.FetchXml);
        }

        [TestMethod]
        public void GetViewDefinitionDataTable_Test()
        {
            var org = OrganizationHelper.AdminService;

            var service = new DynamicsMetaDataService(org);

            var view = service.EntityViewDefinitionsGet("account").FirstOrDefault();

            var graph = service.FetchXmlDataTableDefinitionGet(view.FetchXml);
        }

    }
}