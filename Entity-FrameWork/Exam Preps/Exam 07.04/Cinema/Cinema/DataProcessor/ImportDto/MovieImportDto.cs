﻿namespace Cinema.DataProcessor.ImportDto
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class MovieImportDto
    {
        [Required, MinLength(3), MaxLength(20)]
        public string Title { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required, Range(1, 10)]
        public double Rating { get; set; }
        [Required, MinLength(3), MaxLength(20)]
        public string Director { get; set; }
    }
}
