﻿namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    public class AlbumImportDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string ReleaseDate { get; set; }
    }
}