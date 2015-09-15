using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace XrmViewDataRetriever.Models.MetaData
{
    public class XrmFetchXmlResponse
    {
        public XrmFetchXmlResponse()
        {
            this.EntityDefinitions = new List<XrmEntityDefinition>();
            this.Data = new DataTable();
        }

        public Guid SavedQueryId { get; set; }

        public XrmEntityDefinition TargetEntityDefinition => this.EntityDefinitions.FirstOrDefault();

        public List<XrmEntityDefinition> EntityDefinitions { get; set; }

        public DataTable Data { get; set; }

        public EntityCollection RawEntityData { get; set; }
    }
}