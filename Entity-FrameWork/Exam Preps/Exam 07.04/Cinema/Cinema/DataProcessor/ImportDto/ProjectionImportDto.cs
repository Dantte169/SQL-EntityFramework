﻿namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;
    [XmlType("Projection")]
    public class ProjectionImportDto
    {
        [Required]
        [XmlElement("MovieId")]
        public int MovieId { get; set; }
        [Required]
        [XmlElement("HallId")]
        public int HallId { get; set; }
        [Required]
        [XmlElement("DateTime")]
        public string DateTime { get; set; }
    }
}
