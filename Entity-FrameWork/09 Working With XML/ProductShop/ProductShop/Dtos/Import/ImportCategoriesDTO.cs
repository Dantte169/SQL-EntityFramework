﻿namespace ProductShop.Dtos.Import
{
    using System.Xml.Serialization;
    [XmlType("Category")]
    public class ImportCategoriesDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
    }
}