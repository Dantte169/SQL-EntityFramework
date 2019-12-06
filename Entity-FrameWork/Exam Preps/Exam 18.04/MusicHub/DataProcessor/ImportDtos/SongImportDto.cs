namespace MusicHub.DataProcessor.ImportDtos
{
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel.DataAnnotations;
    using MusicHub.Data.Models.Enums;

    [XmlType("Song")]
   public class SongImportDto
    {
        [Required, MinLength(3), MaxLength(20)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [XmlElement("Duration")]
        public string Duration { get; set; }
        [Required]
        [XmlElement("CreatedOn")]
        public string CreatedOn { get; set; }
        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; }
        [XmlElement("AlbumId")]
        public int AlbumId { get; set; }
        [Required]
        [XmlElement("WriterId")]
        public int WriterId { get; set; }
        [Required]
        [XmlElement("Price")]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
    }
}
