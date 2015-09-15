using System;

namespace XrmViewDataRetriever.Models.MetaData
{
    public class XrmSavedQueryDefinition
    {
        public Guid SavedQueryId { get; set; }

        public string Entity { get; set; }

        public string Name { get; set; }

        public string FetchXml { get; set; }

        public string LayoutXml { get; set; }

        public string EntityDisplayName { get; set; }
    }
}