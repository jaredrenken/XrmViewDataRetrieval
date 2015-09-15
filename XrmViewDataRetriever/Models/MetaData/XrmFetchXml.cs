using System.Xml.Serialization;

namespace XrmViewDataRetriever.Models.MetaData
{
    /// <remarks />
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class fetch
    {
        /// <remarks />
        public fetchEntity entity { get; set; }

        /// <remarks />
        [XmlAttribute]
        public decimal version { get; set; }

        /// <remarks />
        [XmlAttribute("output-format")]
        public string outputformat { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string mapping { get; set; }

        /// <remarks />
        [XmlAttribute]
        public bool distinct { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntity
    {
        /// <remarks />
        [XmlElement("attribute")]
        public fetchEntityAttribute[] attribute { get; set; }

        /// <remarks />
        public fetchEntityOrder order { get; set; }

        /// <remarks />
        public fetchEntityFilter filter { get; set; }

        /// <remarks />
        [XmlElement("link-entity")]
        public fetchEntityLinkentity[] linkentity { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string name { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityAttribute
    {
        /// <remarks />
        [XmlAttribute]
        public string name { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityOrder
    {
        /// <remarks />
        [XmlAttribute]
        public string attribute { get; set; }

        /// <remarks />
        [XmlAttribute]
        public bool descending { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityFilter
    {
        /// <remarks />
        [XmlElement("condition")]
        public fetchEntityFilterCondition[] condition { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string type { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityFilterCondition
    {
        /// <remarks />
        [XmlElement("value")]
        public byte[] value { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string attribute { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string @operator { get; set; }

        /// <remarks />
        [XmlAttribute("value")]
        public string value1 { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityLinkentity
    {
        /// <remarks />
        public fetchEntityLinkentityFilter filter { get; set; }

        /// <remarks />
        [XmlElement("attribute")]
        public fetchEntityLinkentityAttribute[] attribute { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string name { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string from { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string to { get; set; }

        /// <remarks />
        [XmlAttribute]
        public bool visible { get; set; }

        /// <remarks />
        [XmlIgnore]
        public bool visibleSpecified { get; set; }

        /// <remarks />
        [XmlAttribute("link-type")]
        public string linktype { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string alias { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityLinkentityFilter
    {
        /// <remarks />
        public fetchEntityLinkentityFilterCondition condition { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string type { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityLinkentityFilterCondition
    {
        /// <remarks />
        [XmlAttribute]
        public string attribute { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string @operator { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string uiname { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string uitype { get; set; }

        /// <remarks />
        [XmlAttribute]
        public string value { get; set; }
    }

    /// <remarks />
    [XmlType(AnonymousType = true)]
    public class fetchEntityLinkentityAttribute
    {
        /// <remarks />
        [XmlAttribute]
        public string name { get; set; }
    }
}