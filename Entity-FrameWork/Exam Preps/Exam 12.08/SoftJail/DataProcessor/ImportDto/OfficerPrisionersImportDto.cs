namespace SoftJail.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    [XmlType("Prisoner")]
    public class OfficerPrisionersImportDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
