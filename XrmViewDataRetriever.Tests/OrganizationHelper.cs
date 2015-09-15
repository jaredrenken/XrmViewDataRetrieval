using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;

namespace XrmViewDataRetriever.Tests
{
    public static class OrganizationHelper
    {
        public static IOrganizationService AdminService
        {
            get
            {
                var connection = CrmConnection.Parse("username=[username here];password=[password here];url=[url here]");

                return new OrganizationService(connection);
            }
        }

        public static IOrganizationService UserService
        {
            get
            {
                var connection = CrmConnection.Parse("username=[username here];password=[password here];url=[url here]");

                return new OrganizationService(connection);
            }
        }
    }
}