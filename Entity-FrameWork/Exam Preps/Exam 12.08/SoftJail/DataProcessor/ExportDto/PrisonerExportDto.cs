namespace SoftJail.DataProcessor.ExportDto
{
    using System;
    using System.Xml.Serialization;
    [XmlType("Prisoner")]
    public class PrisonerExportDto
    {
        [XmlElement("Id")]
        public int Id { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("IncarcerationDate")]
        public string IncarcerationDate { get; set; }
        [XmlArray("EncryptedMessages")]
        public MessageExportDto[] Messages { get; set; }
    }
}
