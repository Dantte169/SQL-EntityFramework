namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class PerformerSongsImportDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
