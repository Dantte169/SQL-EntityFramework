﻿namespace MusicHub.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Performer
    {
        public Performer()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }
        [Required,MinLength(3),MaxLength(20)]
        public string FirstName { get; set; }
        [Required, MinLength(3), MaxLength(20)]
        public string LastName { get; set; }
        [Required,Range(18,70)]
        public int Age { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335s")]
        public decimal Networth { get; set; }
        public ICollection<SongPerformer> SongPerformers { get; set; }
    }
}
