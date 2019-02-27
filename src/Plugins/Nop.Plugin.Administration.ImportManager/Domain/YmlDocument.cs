using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Nop.Plugin.Administration.ImportManager.Domain
{
    [Serializable()]
    [XmlRoot("yml_catalog")]
    public class YmlDocument
    {
#pragma warning disable IDE1006 // Naming Styles
        [XmlIgnore]
        private DateTime? date { get; set; }

        [XmlAttribute("date")]
        protected string dateString
        {
            get { return this.date?.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { this.date = DateTime.Parse(value); }
        }
        public Shop shop { get; set; }

        [Serializable]
        public class Shop
        {
            public string name { get; set; }
            public string company { get; set; }
            public string url { get; set; }
            [XmlArrayItem("currency", typeof(Currency))]
            public Currency[] currencies { get; set; }
            [XmlArrayItem("category", typeof(Category))]
            public Category[] categories { get; set; }
            [XmlArrayItem("offer", typeof(Offer))]
            public Offer[] offers { get; set; }

        }

        [Serializable]
        public class Currency
        {
            [XmlAttribute]
            public string id { get; set; }
            [XmlAttribute]
            public string rate { get; set; }
        }
        [Serializable]
        public class Category
        {
            [XmlAttribute]
            public int id { get; set; }
            [XmlAttribute]
            public int parentId { get; set; }
            [XmlText]
            public string Value { get; set; }
        }

        [Serializable]
        public class Offer
        {
            [XmlAttribute]
            public bool available { get; set; }
            [XmlAttribute]
            public int group_id { get; set; }
            [XmlAttribute]
            public int id { get; set; }
            public string name { get; set; }
            public string vendor { get; set; }
            public string vendorCode { get; set; }
            public string country_of_origin { get; set; }
            public decimal? price { get; set; }
            public string description { get; set; }
            [XmlElement("picture")]
            public string[] Pictures { get; set; }
            [XmlElement("param")]
            public Parameter[] Params { get; set; }

        }
        [Serializable]
        public class Parameter
        {
            [XmlAttribute]
            public string name { get; set; }
            [XmlAttribute]
            public string unit { get; set; }
            [XmlText]
            public string Value { get; set; }
        }
#pragma warning restore IDE1006 // Naming Styles

    }
}
