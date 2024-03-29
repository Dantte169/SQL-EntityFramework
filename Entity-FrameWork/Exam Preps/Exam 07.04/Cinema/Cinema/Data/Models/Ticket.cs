﻿namespace Cinema.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Ticket
    {
        public Ticket()
        {

        }
        [Key]
        public int Id { get; set; }
        [Required, Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ProjectionId { get; set; }
        public Projection Projection { get; set; }
    }
}
