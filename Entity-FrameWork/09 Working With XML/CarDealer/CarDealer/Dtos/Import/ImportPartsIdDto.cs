using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlRoot("parts")]
    public class ImportPartsIdDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}