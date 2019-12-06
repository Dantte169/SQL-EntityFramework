namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MusicHub.Data.Models;

    public class ProducerAlbumsImportDto
    {
        [Required, MinLength(3), MaxLength(30)]
        public string Name { get; set; }
        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }
        [RegularExpression(@"\+359 [0-9]{3} [0-9]{3} [0-9]{3}")]
        public string PhoneNumber { get; set; }
        public AlbumImportDto[] Albums { get; set; }
    }
}
