namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    public class WriterImportDto
    {
        [Required, MinLength(3), MaxLength(20)]
        public string Name { get; set; }
        [RegularExpression("[A-Z][a-z]+ [A-Z][a-z]+")]
        public string Pseudonym { get; set; }
    }
}
