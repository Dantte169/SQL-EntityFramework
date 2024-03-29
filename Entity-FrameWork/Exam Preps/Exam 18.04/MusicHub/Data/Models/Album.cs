﻿namespace MusicHub.Data.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public decimal Price
            => this.Songs.Sum(p => p.Price);
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ICollection<Song> Songs { get; set; }
    }
}
